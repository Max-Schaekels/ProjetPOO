using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void StartCombat(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (State.CurrentCombat != null)
            {
                throw new InvalidOperationException("Un combat est déjà en cours.");
            }

            PlayerCharacter playerCharacter = State.PlayerCharacter;
            CombatState combat = new CombatState(playerCharacter, enemy);

            State.CurrentCombat = combat;
        }

        public RoundReport PlayRound(CombatActionType playerAction)
        {
            if (State.CurrentCombat == null)
            {
                throw new InvalidOperationException("Aucun combat en cours.");
            }

            CombatState combat = State.CurrentCombat;
            RoundReport report = new RoundReport();

            int potionsBefore = State.PlayerInventory.PotionsCount;

            // Action joueur 
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

            // Si fin de combat après action joueur
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

                State.CurrentCombat = null;
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

                State.CurrentCombat = null;
            }

            return report;
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
    }
}
