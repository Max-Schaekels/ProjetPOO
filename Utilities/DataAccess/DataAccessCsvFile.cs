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
            List<string> listToRead = new List<string>();
            List<Scenario> scenarios = new List<Scenario>();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENARIOS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Scenario scenario = GetScenario(s);
                    if (scenario != null)
                    {
                        scenarios.Add(scenario);
                    }
                }

                return scenarios;
            }
            else
            {
                return null;
            }
        }

        public override Scenario? GetScenarioById(int scenarioId)
        {
            List<Scenario> scenarios = GetAllScenarios();

            if (scenarios == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            ScenesCollection scenes = new ScenesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Scene scene = GetScene(s);
                    if (scene != null)
                    {
                        scenes.Add(scene);
                    }
                }

                return scenes;
            }
            else
            {
                return null;
            }
        }

        public override ScenesCollection GetScenesByScenarioId(int scenarioId)
        {
            List<string> listToRead = new List<string>();
            ScenesCollection scenes = new ScenesCollection(scenarioId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Scene scene = GetScene(s);
                    if (scene != null && scene.ScenarioId == scenarioId)
                    {
                        scenes.AddScene(scene);
                    }
                }

                return scenes;
            }
            else
            {
                return null;
            }
        }

        public override Scene? GetSceneById(int sceneId)
        {
            ScenesCollection scenes = GetAllScenes();

            if (scenes == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            ChoicesCollection choices = new ChoicesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Choice choice = GetChoice(s);
                    if (choice != null)
                    {
                        choices.Add(choice);
                    }
                }

                return choices;
            }
            else
            {
                return null;
            }
        }

        public override ChoicesCollection GetChoicesBySceneId(int sceneId)
        {
            List<string> listToRead = new List<string>();
            ChoicesCollection choices = new ChoicesCollection(sceneId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Choice choice = GetChoice(s);
                    if (choice != null && choice.SceneId == sceneId)
                    {
                        choices.AddChoice(choice);
                    }
                }

                return choices;
            }
            else
            {
                return null;
            }
        }

        public override Choice? GetChoiceById(int choiceId)
        {
            ChoicesCollection choices = GetAllChoices();

            if (choices == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            ConditionsCollection conditions = new ConditionsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CONDITIONS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Condition condition = GetCondition(s);
                    if (condition != null)
                    {
                        conditions.Add(condition);
                    }
                }

                return conditions;
            }
            else
            {
                return null;
            }
        }

        public override ConditionsCollection GetConditionsByChoiceId(int choiceId)
        {
            List<string> listToRead = new List<string>();
            ConditionsCollection conditions = new ConditionsCollection(choiceId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CONDITIONS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Condition condition = GetCondition(s);
                    if (condition != null && condition.ChoiceId == choiceId)
                    {
                        conditions.AddCondition(condition);
                    }
                }

                return conditions;
            }
            else
            {
                return null;
            }
        }

        public override Condition? GetConditionById(int conditionId)
        {
            ConditionsCollection conditions = GetAllConditions();

            if (conditions == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            EffectsCollection effects = new EffectsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("EFFECTS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Effect effect = GetEffect(s);
                    if (effect != null)
                    {
                        effects.Add(effect);
                    }
                }

                return effects;
            }
            else
            {
                return null;
            }
        }

        public override EffectsCollection GetEffectsByChoiceId(int choiceId)
        {
            List<string> listToRead = new List<string>();
            EffectsCollection effects = new EffectsCollection(choiceId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("EFFECTS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Effect effect = GetEffect(s);
                    if (effect != null && effect.ChoiceId == choiceId)
                    {
                        effects.AddEffect(effect);
                    }
                }

                return effects;
            }
            else
            {
                return null;
            }
        }

        public override Effect? GetEffectById(int effectId)
        {
            EffectsCollection effects = GetAllEffects();

            if (effects == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            EnemiesCollection enemies = new EnemiesCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("ENEMIES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Enemy enemy = GetEnemy(s);
                    if (enemy != null)
                    {
                        enemies.Add(enemy);
                    }
                }

                return enemies;
            }
            else
            {
                return null;
            }
        }

        public override EnemiesCollection GetEnemiesByScenarioId(int scenarioId)
        {
            List<string> listToRead = new List<string>();
            EnemiesCollection enemies = new EnemiesCollection(scenarioId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("ENEMIES");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Enemy enemy = GetEnemy(s);
                    if (enemy != null && enemy.ScenarioId == scenarioId)
                    {
                        enemies.AddEnemy(enemy);
                    }
                }

                return enemies;
            }
            else
            {
                return null;
            }
        }

        public override Enemy? GetEnemyById(int enemyId)
        {
            EnemiesCollection enemies = GetAllEnemies();

            if (enemies == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            ShopsCollection shops = new ShopsCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SHOPS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Shop shop = GetShop(s);
                    if (shop != null)
                    {
                        shops.Add(shop);
                    }
                }

                return shops;
            }
            else
            {
                return null;
            }
        }

        public override ShopsCollection GetShopsByScenarioId(int scenarioId)
        {
            List<string> listToRead = new List<string>();
            ShopsCollection shops = new ShopsCollection(scenarioId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SHOPS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    Shop shop = GetShop(s);
                    if (shop != null && shop.ScenarioId == scenarioId)
                    {
                        shops.AddShop(shop);
                    }
                }

                return shops;
            }
            else
            {
                return null;
            }
        }

        public override Shop? GetShopById(int shopId)
        {
            ShopsCollection shops = GetAllShops();

            if (shops == null)
            {
                return null;
            }

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
            List<string> listToRead = new List<string>();
            PlayerCharactersCollection playerCharacters = new PlayerCharactersCollection();

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("PLAYERCHARACTERS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    PlayerCharacterTemplate playerCharacter = GetPlayerCharacterTemplate(s);
                    if (playerCharacter != null)
                    {
                        playerCharacters.Add(playerCharacter);
                    }
                }

                return playerCharacters;
            }
            else
            {
                return null;
            }
        }

        public override PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId)
        {
            List<string> listToRead = new List<string>();
            PlayerCharactersCollection playerCharacters = new PlayerCharactersCollection(scenarioId);

            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("PLAYERCHARACTERS");
            if (IsValidAccessPath)
            {
                listToRead = File.ReadAllLines(AccessPath).ToList();
                listToRead.RemoveAt(0);

                foreach (string s in listToRead)
                {
                    PlayerCharacterTemplate playerCharacter = GetPlayerCharacterTemplate(s);
                    if (playerCharacter != null && playerCharacter.ScenarioId == scenarioId)
                    {
                        playerCharacters.AddPlayer(playerCharacter);
                    }
                }

                return playerCharacters;
            }
            else
            {
                return null;
            }
        }

        public override PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId)
        {
            PlayerCharactersCollection playerCharacters = GetAllPlayerCharacterTemplates();

            if (playerCharacters == null)
            {
                return null;
            }

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
           throw new NotImplementedException();
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
