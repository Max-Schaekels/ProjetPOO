using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.Utilities.DataAccess
{
    public class DataAccessCsvFile : DataAccess, IDataAccess
    {
        public DataAccessCsvFile(string filePath) : base(filePath)
        {
        }

        public DataAccessCsvFile(string filePath, string[] extensions) : base(filePath, extensions)
        {
        }

        public DataAccessCsvFile(DataFilesManager dfm) : base(dfm)
        {
            Extensions = new List<string>() { ".csv" };
        }

        public override List<Scenario> GetAllScenarios()
        {
            List<Scenario> scenarios = new List<Scenario>();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENARIOS");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    scenarios.Add(GetScenario(s));
                }
            }

            return scenarios;
        }

        public override Scenario? GetScenarioById(int scenarioId)
        {
            List<Scenario> scenarios = GetAllScenarios();

            foreach (Scenario scenario in scenarios)
            {
                if (scenario.Id == scenarioId)
                {
                    return scenario;
                }
            }

            return null;
        }

        public override ScenesCollection GetAllScenes()
        {
            ScenesCollection scenes = new ScenesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENES");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    scenes.Add(GetScene(s));
                }
            }

            return scenes;
        }

        public override Scene? GetSceneById(int sceneId)
        {
            ScenesCollection scenes = GetAllScenes();

            foreach (Scene scene in scenes)
            {
                if (scene.Id == sceneId)
                {
                    return scene;
                }
            }

            return null;
        }

        public override ChoicesCollection GetAllChoices()
        {
            ChoicesCollection choices = new ChoicesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    choices.Add(GetChoice(s));
                }
            }

            return choices;
        }

        public override Choice? GetChoiceById(int choiceId)
        {
            ChoicesCollection choices = GetAllChoices();

            foreach (Choice choice in choices)
            {
                if (choice.Id == choiceId)
                {
                    return choice;
                }
            }

            return null;
        }

        public override ConditionsCollection GetAllConditions()
        {
            ConditionsCollection conditions = new ConditionsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CONDITIONS");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    conditions.Add(GetCondition(s));
                }
            }

            return conditions;
        }

        public override Condition? GetConditionById(int conditionId)
        {
            ConditionsCollection conditions = GetAllConditions();

            foreach (Condition condition in conditions)
            {
                if (condition.Id == conditionId)
                {
                    return condition;
                }
            }

            return null;
        }

        public override EffectsCollection GetAllEffects()
        {
            EffectsCollection effects = new EffectsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("EFFECTS");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    effects.Add(GetEffect(s));
                }
            }

            return effects;
        }

        public override Effect? GetEffectById(int effectId)
        {
            EffectsCollection effects = GetAllEffects();

            foreach (Effect effect in effects)
            {
                if (effect.Id == effectId)
                {
                    return effect;
                }
            }

            return null;
        }

        public override EnemiesCollection GetAllEnemies()
        {
            EnemiesCollection enemies = new EnemiesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("ENEMIES");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    enemies.Add(GetEnemy(s));
                }
            }

            return enemies;
        }

        public override Enemy? GetEnemyById(int enemyId)
        {
            EnemiesCollection enemies = GetAllEnemies();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.Id == enemyId)
                {
                    return enemy;
                }
            }

            return null;
        }

        public override ShopsCollection GetAllShops()
        {
            ShopsCollection shops = new ShopsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SHOPS");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    shops.Add(GetShop(s));
                }
            }

            return shops;
        }

        public override Shop? GetShopById(int shopId)
        {
            ShopsCollection shops = GetAllShops();

            foreach (Shop shop in shops)
            {
                if (shop.Id == shopId)
                {
                    return shop;
                }
            }

            return null;
        }

        public override PlayerCharactersCollection GetAllPlayerCharacterTemplates()
        {
            PlayerCharactersCollection playerCharacters = new PlayerCharactersCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("PLAYERCHARACTERS");

            if (IsValidAccessPath)
            {
                List<string> listToRead = File.ReadAllLines(AccessPath).ToList();

                if (listToRead.Any())
                {
                    listToRead.RemoveAt(0);
                }

                foreach (string s in listToRead)
                {
                    playerCharacters.Add(GetPlayerCharacterTemplate(s));
                }
            }

            return playerCharacters;
        }

        public override PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId)
        {
            PlayerCharactersCollection playerCharacters = GetAllPlayerCharacterTemplates();

            foreach (PlayerCharacterTemplate playerCharacter in playerCharacters)
            {
                if (playerCharacter.Id == playerCharacterTemplateId)
                {
                    return playerCharacter;
                }
            }

            return null;
        }

        public override Scenario? LoadScenario(int scenarioId)
        {
            Scenario? scenario = GetScenarioById(scenarioId);

            if (scenario == null)
            {
                return null;
            }

            ScenesCollection scenes = GetAllScenes();
            ChoicesCollection choices = GetAllChoices();
            ConditionsCollection conditions = GetAllConditions();
            EffectsCollection effects = GetAllEffects();
            EnemiesCollection enemies = GetAllEnemies();
            ShopsCollection shops = GetAllShops();
            PlayerCharactersCollection playerCharacters = GetAllPlayerCharacterTemplates();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.ScenarioId == scenario.Id)
                {
                    scenario.Enemies.Add(enemy);
                }
            }

            foreach (Shop shop in shops)
            {
                if (shop.ScenarioId == scenario.Id)
                {
                    scenario.Shops.Add(shop);
                }
            }

            foreach (PlayerCharacterTemplate playerCharacter in playerCharacters)
            {
                if (playerCharacter.ScenarioId == scenario.Id)
                {
                    scenario.PlayerCharacters.Add(playerCharacter);
                }
            }

            foreach (Scene scene in scenes)
            {
                if (scene.ScenarioId == scenario.Id)
                {
                    foreach (Choice choice in choices)
                    {
                        if (choice.SceneId == scene.Id)
                        {
                            foreach (Condition condition in conditions)
                            {
                                if (condition.ChoiceId == choice.Id)
                                {
                                    choice.Conditions.Add(condition);
                                }
                            }

                            foreach (Effect effect in effects)
                            {
                                if (effect.ChoiceId == choice.Id)
                                {
                                    choice.Effects.Add(effect);
                                }
                            }

                            scene.Choices.Add(choice);
                        }
                    }

                    scenario.Scenes.Add(scene);
                }
            }

            return scenario;
        }

        public override void SaveScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void AddScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void UpdateScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void DeleteScenario(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override void AddScene(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override void UpdateScene(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override void DeleteScene(int sceneId)
        {
            throw new NotImplementedException();
        }

        public override void AddChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public override void UpdateChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public override void DeleteChoice(int choiceId)
        {
            throw new NotImplementedException();
        }

        public override void AddCondition(Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void UpdateCondition(Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCondition(int conditionId)
        {
            throw new NotImplementedException();
        }

        public override void AddEffect(Effect effect)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEffect(Effect effect)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEffect(int effectId)
        {
            throw new NotImplementedException();
        }

        public override void AddEnemy(Enemy enemy)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEnemy(Enemy enemy)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEnemy(int enemyId)
        {
            throw new NotImplementedException();
        }

        public override void AddShop(Shop shop)
        {
            throw new NotImplementedException();
        }

        public override void UpdateShop(Shop shop)
        {
            throw new NotImplementedException();
        }

        public override void DeleteShop(int shopId)
        {
            throw new NotImplementedException();
        }

        public override void AddPlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            throw new NotImplementedException();
        }

        public override void UpdatePlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            throw new NotImplementedException();
        }

        public override void DeletePlayerCharacterTemplate(int playerCharacterTemplateId)
        {
            throw new NotImplementedException();
        }

        private Scenario GetScenario(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Scene GetScene(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Choice GetChoice(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Condition GetCondition(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Effect GetEffect(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Enemy GetEnemy(string csvLine)
        {
            throw new NotImplementedException();
        }

        private Shop GetShop(string csvLine)
        {
            throw new NotImplementedException();
        }

        private PlayerCharacterTemplate GetPlayerCharacterTemplate(string csvLine)
        {
            throw new NotImplementedException();
        }
    }
}
