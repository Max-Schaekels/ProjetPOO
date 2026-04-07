using Microsoft.Maui.Controls;
using Newtonsoft.Json;
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
using static System.Formats.Asn1.AsnWriter;

namespace ProjetPOO.Utilities.DataAccess
{
    public class DataAccessJsonFile : DataAccess, IDataAccess
    {
        public DataAccessJsonFile(string filePath) : base(filePath)
        {
        }
        public DataAccessJsonFile(string filePath, string[] extensions) : base(filePath, extensions)
        {
        }
        public DataAccessJsonFile(DataFilesManager dfm) : base(dfm)
        {
        }


        public override ChoicesCollection GetAllChoices()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                ChoicesCollection chs = new ChoicesCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                chs = JsonConvert.DeserializeObject<ChoicesCollection>(jsonFile, settings);
                return chs;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override ConditionsCollection GetAllConditions()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CONDITIONS");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                ConditionsCollection cons = new ConditionsCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                cons = JsonConvert.DeserializeObject<ConditionsCollection>(jsonFile, settings);
                return cons;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override EffectsCollection GetAllEffects()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("EFFECTS");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                EffectsCollection  effs = new EffectsCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                effs = JsonConvert.DeserializeObject<EffectsCollection>(jsonFile, settings);
                return effs;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override EnemiesCollection GetAllEnemies()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("ENEMIES");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                EnemiesCollection enemies = new EnemiesCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                enemies = JsonConvert.DeserializeObject<EnemiesCollection>(jsonFile, settings);
                return enemies;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override PlayerCharactersCollection GetAllPlayerCharacterTemplates()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("PLAYERCHARACTERS");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                PlayerCharactersCollection players = new PlayerCharactersCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                players = JsonConvert.DeserializeObject<PlayerCharactersCollection>(jsonFile, settings);
                return players;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override List<Scenario> GetAllScenarios()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENARIOS");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                List<Scenario> scenarios = new List<Scenario>();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                scenarios = JsonConvert.DeserializeObject<List<Scenario>>(jsonFile, settings);
                return scenarios;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override ScenesCollection GetAllScenes()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENES");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                ScenesCollection scs = new ScenesCollection();
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                scs = JsonConvert.DeserializeObject<ScenesCollection>(jsonFile, settings);
                return scs;
            }
            else
            {
                return null;
            }
        }

        public override ShopsCollection GetAllShops()
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SHOPS");
            if (IsValidAccessPath)
            {
                string jsonFile = File.ReadAllText(AccessPath);
                ShopsCollection shops = new ShopsCollection();
                //settings are necessary to get also specific properties of the derivated class
                //and not only common properties of the base class (User)
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                shops = JsonConvert.DeserializeObject<ShopsCollection>(jsonFile, settings);
                return shops;
            }
            else
            {
                //Console.WriteLine("GetAllItems Failes -> File doesnt exist");
                return null;
            }
        }

        public override void UpdateAllScenarios(List<Scenario> scenarios)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENARIOS");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(scenarios, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllScenarios error can't update datasource file");
            }
        }

        public override void UpdateAllScenes(ScenesCollection scenes)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENES");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(scenes, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllScenes error can't update datasource file");
            }
        }

        public override void UpdateAllChoices(ChoicesCollection choices)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(choices, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllChoices error can't update datasource file");
            }
        }

        public override void UpdateAllConditions(ConditionsCollection conditions)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CONDITIONS");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(conditions, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllConditions error can't update datasource file");
            }
        }

        public override void UpdateAllEffects(EffectsCollection effects)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("EFFECTS");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(effects, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllEffects error can't update datasource file");
            }
        }

        public override void UpdateAllEnemies(EnemiesCollection enemies)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("ENEMIES");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(enemies, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllEnemies error can't update datasource file");
            }
        }

        public override void UpdateAllShops(ShopsCollection shops)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SHOPS");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(shops, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllShops error can't update datasource file");
            }
        }

        public override void UpdateAllPlayerCharacterTemplates(PlayerCharactersCollection playerCharacterTemplates)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("PLAYERCHARACTERS");
            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string json = JsonConvert.SerializeObject(playerCharacterTemplates, Newtonsoft.Json.Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
            }
            else
            {
                Console.WriteLine("UpdateAllPlayerCharacterTemplates error can't update datasource file");
            }
        }


        public override void UpdateChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public override void UpdateCondition(Model.Story.Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEffect(Model.Story.Effect effect)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEnemy(Enemy enemy)
        {
            throw new NotImplementedException();
        }

        public override void UpdatePlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            throw new NotImplementedException();
        }

        public override void UpdateScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void UpdateScene(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override void UpdateShop(Shop shop)
        {
            throw new NotImplementedException();
        }


        public override void AddChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public override void AddCondition(Model.Story.Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void AddEffect(Model.Story.Effect effect)
        {
            throw new NotImplementedException();
        }

        public override void AddEnemy(Enemy enemy)
        {
            throw new NotImplementedException();
        }

        public override void AddPlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            throw new NotImplementedException();
        }

        public override void AddScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void AddScene(Scene scene)
        {
            throw new NotImplementedException();
        }

        public override void AddShop(Shop shop)
        {
            throw new NotImplementedException();
        }

        public override void DeleteChoice(int choiceId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCondition(int conditionId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEffect(int effectId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteEnemy(int enemyId)
        {
            throw new NotImplementedException();
        }

        public override void DeletePlayerCharacterTemplate(int playerCharacterTemplateId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteScenario(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteScene(int sceneId)
        {
            throw new NotImplementedException();
        }

        public override void DeleteShop(int shopId)
        {
            throw new NotImplementedException();
        }

       

        public override Choice? GetChoiceById(int choiceId)
        {
            throw new NotImplementedException();
        }

        public override ChoicesCollection GetChoicesBySceneId(int sceneId)
        {
            throw new NotImplementedException();
        }

        public override Model.Story.Condition? GetConditionById(int conditionId)
        {
            throw new NotImplementedException();
        }

        public override ConditionsCollection GetConditionsByChoiceId(int choiceId)
        {
            throw new NotImplementedException();
        }

        public override Model.Story.Effect? GetEffectById(int effectId)
        {
            throw new NotImplementedException();
        }

        public override EffectsCollection GetEffectsByChoiceId(int choiceId)
        {
            throw new NotImplementedException();
        }

        public override EnemiesCollection GetEnemiesByScenarioId(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override Enemy? GetEnemyById(int enemyId)
        {
            throw new NotImplementedException();
        }

        public override PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId)
        {
            throw new NotImplementedException();
        }

        public override PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override Scenario? GetScenarioById(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override Scene? GetSceneById(int sceneId)
        {
            throw new NotImplementedException();
        }

        public override ScenesCollection GetScenesByScenarioId(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override Shop? GetShopById(int shopId)
        {
            throw new NotImplementedException();
        }

        public override ShopsCollection GetShopsByScenarioId(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override Scenario? LoadScenario(int scenarioId)
        {
            throw new NotImplementedException();
        }

        public override void SaveScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }


    }
}
