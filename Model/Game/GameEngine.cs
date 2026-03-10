using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using System;

namespace ProjetPOO.Model.Game
{
    public class GameEngine
    {
        private readonly GameState _state;

        public GameState State => _state;

        public GameEngine(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            _state = state;
        }


        // Démarrage / navigation


        public void StartScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (scenario.StartSceneId <= 0)
            {
                throw new InvalidOperationException("Le scénario n'a pas de StartSceneId (0).");
            }

            Scene? startScene = scenario.GetSceneById(scenario.StartSceneId);
            if (startScene == null)
            {
                throw new InvalidOperationException($"StartSceneId={scenario.StartSceneId} n'existe pas dans le scénario.");
            }

            State.SetScenario(scenario.Id);
            State.MoveToScene(startScene.Id);

            EnterCurrentScene(scenario);
        }

        public Scene GetCurrentScene(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (State.ScenarioId > 0 && scenario.Id != State.ScenarioId)
            {
                throw new InvalidOperationException($"Le GameState est lié au scénario {State.ScenarioId}, pas au scénario {scenario.Id}.");
            }

            if (State.CurrentSceneId <= 0)
            {
                throw new InvalidOperationException("CurrentSceneId n'est pas défini (0).");
            }

            Scene? scene = scenario.GetSceneById(State.CurrentSceneId);
            if (scene == null)
            {
                throw new InvalidOperationException($"La scène courante Id={State.CurrentSceneId} n'existe pas dans le scénario.");
            }

            return scene;
        }

        
        // Retourne les choix disponibles pour la scène courante (basé sur les conditions).
       
        public List<Choice> GetAvailableChoices(Scenario scenario)
        {
            Scene scene = GetCurrentScene(scenario);
            return scene.GetAvailableChoices(State);
        }

        /// <summary>
        /// Joue un choix depuis la scène courante :
        /// - vérifie appartenance + disponibilité
        /// - applique effets
        /// - navigue vers la TargetScene
        /// - déclenche EnterCurrentScene (combat auto si nécessaire)
        /// </summary>
        public void PlayChoice(Scenario scenario, int choiceId)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (State.IsInCombat())
            {
                throw new InvalidOperationException("Impossible de jouer un choix narratif pendant un combat.");
            }

            Scene currentScene = GetCurrentScene(scenario);

            Choice? choice = currentScene.Choices.FirstOrDefault(c => c != null && c.Id == choiceId);
            if (choice == null)
            {
                throw new InvalidOperationException($"ChoiceId={choiceId} n'existe pas dans la scène courante.");
            }

            if (choice.SceneId > 0 && choice.SceneId != currentScene.Id)
            {
                throw new InvalidOperationException("Le choix ne correspond pas à la scène courante (SceneId incohérent).");
            }

            if (!choice.IsAvailable(State))
            {
                throw new InvalidOperationException("Ce choix n'est pas disponible (conditions non remplies).");
            }

            if (choice.TargetSceneId <= 0)
            {
                throw new InvalidOperationException("Le choix n'a pas de TargetSceneId valide (>0).");
            }

            Scene? targetScene = scenario.GetSceneById(choice.TargetSceneId);
            if (targetScene == null)
            {
                throw new InvalidOperationException($"TargetSceneId={choice.TargetSceneId} n'existe pas dans le scénario.");
            }

            // appliquer effets
            choice.ApplyEffects(State);

            // navigation
            State.MoveToScene(targetScene.Id);

            // déclencher le comportement de la scène (combat auto, etc.)
            EnterCurrentScene(scenario);
        }


        // Entrée dans une scène

        /// <summary>
        /// À appeler après chaque MoveToScene : déclenche les comportements automatiques
        /// (ex: démarrer combat si scène Combat).
        /// </summary>
        public void EnterCurrentScene(Scenario scenario)
        {
            Scene scene = GetCurrentScene(scenario);

            if (scene.Type == SceneType.Combat)
            {
                if (scene.EnemyId == null)
                {
                    throw new InvalidOperationException("Scène Combat invalide : EnemyId est null.");
                }

                Enemy enemy = ResolveEnemy(scenario, scene.EnemyId.Value);

                if (!State.IsInCombat())
                {
                    StartCombat(enemy);
                }
            }
            else if (scene.Type == SceneType.Shop)
            {
                if (scene.ShopId == null)
                {
                    throw new InvalidOperationException("Scène Shop invalide : ShopId est null.");
                }

                ResolveShop(scenario, scene.ShopId.Value);

                if (State.IsInCombat())
                {
                    State.EndCombat();
                }
            }
            else
            {
                // Si on entre dans une scène non-combat, on s'assure d'être hors combat
                if (State.IsInCombat())
                {
                    State.EndCombat();
                }
            }

        }

        // Combat

        public void StartCombat(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (State.IsInCombat())
            {
                throw new InvalidOperationException("Un combat est déjà en cours.");
            }

            PlayerCharacter playerCharacter = State.PlayerCharacter;
            CombatState combat = new CombatState(playerCharacter, enemy);

            State.StartCombat(combat);
        }

        public RoundReport PlayRound(Scenario scenario, CombatActionType playerAction)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (State.CurrentCombat == null)
            {
                throw new InvalidOperationException("Aucun combat en cours.");
            }

            CombatState combat = State.CurrentCombat;
            RoundReport report = new RoundReport();

            int potionsBefore = State.PlayerInventory.PotionsCount;

            if (playerAction == CombatActionType.UseItem)
            {
                report.MarkPotionAttempted();
            }

            if (playerAction == CombatActionType.Flee)
            {
                report.MarkFleeAttempted();
            }

            int enemyHpBefore = combat.Enemy.CurrentHp;

            combat.ResolvePlayerAction(playerAction, State);

            int enemyHpAfter = combat.Enemy.CurrentHp;

            int damageByPlayer = enemyHpBefore - enemyHpAfter;
            if (damageByPlayer < 0)
            {
                damageByPlayer = 0;
            }

            report.SetPlayerDamage(damageByPlayer);

            int potionsAfter = State.PlayerInventory.PotionsCount;

            if (report.PotionAttempted && potionsAfter < potionsBefore)
            {
                report.MarkPotionConsumed();
            }

            // Fin de combat après action joueur ?
            if (combat.Result != CombatResult.InProgress)
            {
                report.SetResult(combat.Result);

                if (combat.Result == CombatResult.Fled)
                {
                    report.MarkFledSuccessfully();
                }

                if (combat.Result == CombatResult.Victory)
                {
                    ApplyVictoryRewards(combat, report);
                }

                ResolveCombatExit(scenario, combat.Result);

                return report;
            }

            // --- Action ennemi ---
            if (!combat.IsPlayerTurn)
            {
                int playerHpBeforeEnemy = combat.Player.CurrentHp;

                combat.ResolveEnemyAction(State);

                int playerHpAfterEnemy = combat.Player.CurrentHp;

                int enemyDamage = playerHpBeforeEnemy - playerHpAfterEnemy;
                if (enemyDamage < 0)
                {
                    enemyDamage = 0;
                }

                report.SetEnemyDamage(enemyDamage);
            }

            report.SetResult(combat.Result);

            // Fin possible après l’ennemi
            if (combat.Result != CombatResult.InProgress)
            {
                if (combat.Result == CombatResult.Victory)
                {
                    ApplyVictoryRewards(combat, report);
                }

                ResolveCombatExit(scenario, combat.Result);

            }

            return report;
        }

        private void ResolveCombatExit(Scenario scenario, CombatResult result)
        {
            Scene scene = GetCurrentScene(scenario);

            State.EndCombat();

            if (scene.Type != SceneType.Combat)
            {
                return;
            }

            int? targetId = null;

            if (result == CombatResult.Victory)
            {
                targetId = scene.VictoryTargetSceneId;
            }
            else if (result == CombatResult.Defeat)
            {
                targetId = scene.DefeatTargetSceneId;
            }
            else if (result == CombatResult.Fled)
            {
                targetId = scene.FleeTargetSceneId;
            }

            if (targetId == null || targetId.Value <= 0)
            {
                throw new InvalidOperationException("Scène combat : target de sortie invalide (null ou <= 0).");
            }

            Scene? targetScene = scenario.GetSceneById(targetId.Value);
            if (targetScene == null)
            {
                throw new InvalidOperationException($"Scène combat : targetId={targetId.Value} n'existe pas dans le scénario.");
            }

            State.MoveToScene(targetScene.Id);

            // Entrée dans la nouvelle scène (ex: si c'est une autre scène combat)
            EnterCurrentScene(scenario);
        }

        private void ApplyVictoryRewards(CombatState combat, RoundReport report)
        {
            Enemy enemy = combat.Enemy;

            int experience = enemy.GetExperienceReward();
            State.PlayerCharacter.GainExperience(experience);

            Loot loot = enemy.GetLoot();
            State.GrantLoot(loot);

            string lootDescription = loot.GetDescription();
            report.SetRewards(experience, lootDescription);
        }

        private Enemy ResolveEnemy(Scenario scenario, int enemyId)
        {
            Enemy? enemy = scenario.GetEnemyById(enemyId);
            if (enemy == null)
            {
                throw new InvalidOperationException($"EnemyId={enemyId} introuvable dans le scénario.");
            }

            return enemy;
        }

        // Shop

        public Shop GetCurrentShop(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            Scene scene = GetCurrentScene(scenario);

            if (scene.Type != SceneType.Shop)
            {
                throw new InvalidOperationException("La scène courante n'est pas une scène Shop.");
            }

            if (scene.ShopId == null)
            {
                throw new InvalidOperationException("La scène Shop courante n'a pas de ShopId.");
            }

            return ResolveShop(scenario, scene.ShopId.Value);
        }

        public bool BuyPotionInCurrentShop(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (State.IsInCombat())
            {
                throw new InvalidOperationException("Impossible d'acheter dans une boutique pendant un combat.");
            }

            Shop shop = GetCurrentShop(scenario);

            return shop.BuyPotion(State);
        }

        public bool BuyKeyInCurrentShop(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            if (State.IsInCombat())
            {
                throw new InvalidOperationException("Impossible d'acheter dans une boutique pendant un combat.");
            }

            Shop shop = GetCurrentShop(scenario);

            return shop.BuyKey(State);
        }

        private Shop ResolveShop(Scenario scenario, int shopId)
        {
            Shop? shop = scenario.GetShopById(shopId);
            if (shop == null)
            {
                throw new InvalidOperationException($"ShopId={shopId} introuvable dans le scénario.");
            }

            return shop;
        }
    }
}