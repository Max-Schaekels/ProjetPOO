using Newtonsoft.Json;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.DataAccess.JsonDtos;
using ProjetPOO.Utilities.Interfaces;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.Utilities.DataAccess
{
    public class DataAccessJsonFile : DataAccess, IDataAccess
    {
        private ConditionsCollection? cachedConditions;
        private EffectsCollection? cachedEffects;
        private ChoicesCollection? cachedChoices;
        private ScenesCollection? cachedScenes;
        private EnemiesCollection? cachedEnemies;
        private ShopsCollection? cachedShops;
        private PlayerCharactersCollection? cachedPlayerCharacters;
        private List<Scenario>? cachedScenarios;

        public DataAccessJsonFile(string filePath) : base(filePath)
        {
        }

        public DataAccessJsonFile(string filePath, string[] extensions) : base(filePath, extensions)
        {
        }

        public DataAccessJsonFile(DataFilesManager dfm) : base(dfm)
        {
        }

        private void ClearCache()
        {
            cachedConditions = null;
            cachedEffects = null;
            cachedChoices = null;
            cachedScenes = null;
            cachedEnemies = null;
            cachedShops = null;
            cachedPlayerCharacters = null;
            cachedScenarios = null;
        }

        private List<T>? ReadJsonValues<T>(string codeFunction)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction(codeFunction);

            if (!IsValidAccessPath)
            {
                return null;
            }

            string jsonFile = File.ReadAllText(AccessPath);

            if (string.IsNullOrWhiteSpace(jsonFile))
            {
                return new List<T>();
            }

            try
            {
                JsonCollectionDto<T>? wrapper = JsonConvert.DeserializeObject<JsonCollectionDto<T>>(jsonFile);

                if (wrapper != null && wrapper.Values != null)
                {
                    return wrapper.Values;
                }
            }
            catch
            {
                // On tente ensuite une désérialisation en liste directe.
            }

            try
            {
                List<T>? directList = JsonConvert.DeserializeObject<List<T>>(jsonFile);

                if (directList != null)
                {
                    return directList;
                }
            }
            catch
            {
                // Si les deux formats échouent, on renvoie une liste vide.
            }

            return new List<T>();
        }

        private string GetSafeTitle(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Titre chargé";
            }

            return value;
        }

        private string GetSafeDescription(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Description chargée automatiquement pour le scénario.";
            }

            return value;
        }

        private string GetSafeSceneText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Texte chargé automatiquement.";
            }

            return value;
        }

        private string GetSafeLabel(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Choix chargé";
            }

            return value;
        }

        private string GetSafeName(string? value, string fallback)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return fallback;
            }

            return value;
        }

        public override ConditionsCollection GetAllConditions()
        {
            if (cachedConditions != null)
            {
                return cachedConditions;
            }

            List<ConditionJsonDto>? dtos = ReadJsonValues<ConditionJsonDto>("CONDITIONS");

            if (dtos == null)
            {
                return null;
            }

            ConditionsCollection conditions = new ConditionsCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                ConditionJsonDto dto = dtos[i];
                Condition condition = Condition.Load(dto.Id, dto.ChoiceId, dto.Type, dto.MinValue);
                conditions.Add(condition);
            }

            cachedConditions = conditions;
            return cachedConditions;
        }

        public override EffectsCollection GetAllEffects()
        {
            if (cachedEffects != null)
            {
                return cachedEffects;
            }

            List<EffectJsonDto>? dtos = ReadJsonValues<EffectJsonDto>("EFFECTS");

            if (dtos == null)
            {
                return null;
            }

            EffectsCollection effects = new EffectsCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                EffectJsonDto dto = dtos[i];
                Effect effect = Effect.Load(dto.Id, dto.ChoiceId, dto.Type, dto.Amount, dto.FlagKey);
                effects.Add(effect);
            }

            cachedEffects = effects;
            return cachedEffects;
        }

        public override ChoicesCollection GetAllChoices()
        {
            if (cachedChoices != null)
            {
                return cachedChoices;
            }

            List<ChoiceJsonDto>? dtos = ReadJsonValues<ChoiceJsonDto>("CHOICES");

            if (dtos == null)
            {
                return null;
            }

            ChoicesCollection choices = new ChoicesCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                ChoiceJsonDto dto = dtos[i];

                ConditionsCollection conditions = GetConditionsByChoiceId(dto.Id);
                EffectsCollection effects = GetEffectsByChoiceId(dto.Id);

                Choice choice = Choice.Load( dto.Id,GetSafeLabel(dto.Label), dto.TargetSceneId,dto.SceneId,conditions,effects);

                choices.Add(choice);
            }

            cachedChoices = choices;
            return cachedChoices;
        }

        public override ScenesCollection GetAllScenes()
        {
            if (cachedScenes != null)
            {
                return cachedScenes;
            }

            List<SceneJsonDto>? dtos = ReadJsonValues<SceneJsonDto>("SCENES");

            if (dtos == null)
            {
                return null;
            }

            ScenesCollection scenes = new ScenesCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                SceneJsonDto dto = dtos[i];

                ChoicesCollection choices = GetChoicesBySceneId(dto.Id);

                Scene scene = Scene.Load(dto.Id, GetSafeTitle(dto.Title),GetSafeSceneText(dto.Text),dto.Type,dto.ScenarioId,dto.PictureFileName, dto.ShopId,dto.EnemyId, dto.FleeTargetSceneId, dto.DefeatTargetSceneId,dto.VictoryTargetSceneId,choices);

                scenes.Add(scene);
            }

            cachedScenes = scenes;
            return cachedScenes;
        }

        public override EnemiesCollection GetAllEnemies()
        {
            if (cachedEnemies != null)
            {
                return cachedEnemies;
            }

            List<EnemyJsonDto>? dtos = ReadJsonValues<EnemyJsonDto>("ENEMIES");

            if (dtos == null)
            {
                return null;
            }

            EnemiesCollection enemies = new EnemiesCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                EnemyJsonDto dto = dtos[i];

                Enemy enemy = Enemy.Load(dto.Id,dto.ScenarioId, GetSafeName(dto.Name, "Enemy chargé"), dto.MaxHp,dto.Attack, dto.Defense,dto.Agility,dto.Type,dto.RewardExperience, dto.RewardGoldMin, dto.RewardGoldMax,dto.PotionDropChance,dto.PotionAmountMin,dto.PotionAmountMax,dto.KeyDropChance,dto.KeyAmountMin, dto.KeyAmountMax);

                enemies.Add(enemy);
            }

            cachedEnemies = enemies;
            return cachedEnemies;
        }

        public override ShopsCollection GetAllShops()
        {
            if (cachedShops != null)
            {
                return cachedShops;
            }

            List<ShopJsonDto>? dtos = ReadJsonValues<ShopJsonDto>("SHOPS");

            if (dtos == null)
            {
                return null;
            }

            ShopsCollection shops = new ShopsCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                ShopJsonDto dto = dtos[i];

                Shop shop = Shop.Load( dto.Id,dto.ScenarioId,GetSafeName(dto.Name, "Shop chargé"), dto.PotionPrice, dto.KeyPrice);

                shops.Add(shop);
            }

            cachedShops = shops;
            return cachedShops;
        }

        public override PlayerCharactersCollection GetAllPlayerCharacterTemplates()
        {
            if (cachedPlayerCharacters != null)
            {
                return cachedPlayerCharacters;
            }

            List<PlayerCharacterTemplateJsonDto>? dtos = ReadJsonValues<PlayerCharacterTemplateJsonDto>("PLAYERCHARACTERS");

            if (dtos == null)
            {
                return null;
            }

            PlayerCharactersCollection players = new PlayerCharactersCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                PlayerCharacterTemplateJsonDto dto = dtos[i];

                PlayerCharacterTemplate player = PlayerCharacterTemplate.Load(dto.Id, dto.ScenarioId, GetSafeName(dto.Name, "Player chargé"), dto.MaxHp, dto.Attack, dto.Defense,dto.Agility,dto.StartingExperience,dto.StartingLevel);

                players.Add(player);
            }

            cachedPlayerCharacters = players;
            return cachedPlayerCharacters;
        }

        public override List<Scenario> GetAllScenarios()
        {
            if (cachedScenarios != null)
            {
                return cachedScenarios;
            }

            List<ScenarioJsonDto>? dtos = ReadJsonValues<ScenarioJsonDto>("SCENARIOS");

            if (dtos == null)
            {
                return null;
            }

            List<Scenario> scenarios = new List<Scenario>();

            for (int i = 0; i < dtos.Count; i++)
            {
                ScenarioJsonDto dto = dtos[i];

                ScenesCollection scenes = GetScenesByScenarioId(dto.Id);
                EnemiesCollection enemies = GetEnemiesByScenarioId(dto.Id);
                ShopsCollection shops = GetShopsByScenarioId(dto.Id);
                PlayerCharactersCollection players = GetPlayerCharacterTemplatesByScenarioId(dto.Id);

                Scenario scenario = Scenario.Load( dto.Id, GetSafeTitle(dto.Title),GetSafeDescription(dto.Description),dto.StartSceneId,scenes, enemies, shops,players);

                scenarios.Add(scenario);
            }

            cachedScenarios = scenarios;
            return cachedScenarios;
        }

        public override Choice? GetChoiceById(int choiceId)
        {
            ChoicesCollection choices = GetAllChoices();

            if (choices == null)
            {
                return null;
            }

            return choices.GetById(choiceId);
        }

        public override ChoicesCollection GetChoicesBySceneId(int sceneId)
        {
            ChoicesCollection allChoices = GetAllChoices();

            if (allChoices == null)
            {
                return new ChoicesCollection(sceneId);
            }

            ChoicesCollection choices = new ChoicesCollection(sceneId);

            for (int i = 0; i < allChoices.Count; i++)
            {
                Choice choice = allChoices[i];

                if (choice != null && choice.SceneId == sceneId)
                {
                    choices.AddChoice(choice);
                }
            }

            return choices;
        }

        public override Condition? GetConditionById(int conditionId)
        {
            ConditionsCollection conditions = GetAllConditions();

            if (conditions == null)
            {
                return null;
            }

            return conditions.GetById(conditionId);
        }

        public override ConditionsCollection GetConditionsByChoiceId(int choiceId)
        {
            ConditionsCollection allConditions = GetAllConditions();

            if (allConditions == null)
            {
                return new ConditionsCollection(choiceId);
            }

            ConditionsCollection conditions = new ConditionsCollection(choiceId);

            for (int i = 0; i < allConditions.Count; i++)
            {
                Condition condition = allConditions[i];

                if (condition != null && condition.ChoiceId == choiceId)
                {
                    conditions.AddCondition(condition);
                }
            }

            return conditions;
        }

        public override Effect? GetEffectById(int effectId)
        {
            EffectsCollection effects = GetAllEffects();

            if (effects == null)
            {
                return null;
            }

            return effects.GetById(effectId);
        }

        public override EffectsCollection GetEffectsByChoiceId(int choiceId)
        {
            EffectsCollection allEffects = GetAllEffects();

            if (allEffects == null)
            {
                return new EffectsCollection(choiceId);
            }

            EffectsCollection effects = new EffectsCollection(choiceId);

            for (int i = 0; i < allEffects.Count; i++)
            {
                Effect effect = allEffects[i];

                if (effect != null && effect.ChoiceId == choiceId)
                {
                    effects.AddEffect(effect);
                }
            }

            return effects;
        }

        public override EnemiesCollection GetEnemiesByScenarioId(int scenarioId)
        {
            EnemiesCollection allEnemies = GetAllEnemies();

            if (allEnemies == null)
            {
                return new EnemiesCollection(scenarioId);
            }

            EnemiesCollection enemies = new EnemiesCollection(scenarioId);

            for (int i = 0; i < allEnemies.Count; i++)
            {
                Enemy enemy = allEnemies[i];

                if (enemy != null && enemy.ScenarioId == scenarioId)
                {
                    enemies.AddEnemy(enemy);
                }
            }

            return enemies;
        }

        public override Enemy? GetEnemyById(int enemyId)
        {
            EnemiesCollection enemies = GetAllEnemies();

            if (enemies == null)
            {
                return null;
            }

            return enemies.GetById(enemyId);
        }

        public override PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId)
        {
            PlayerCharactersCollection players = GetAllPlayerCharacterTemplates();

            if (players == null)
            {
                return null;
            }

            return players.GetById(playerCharacterTemplateId);
        }

        public override PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId)
        {
            PlayerCharactersCollection allPlayers = GetAllPlayerCharacterTemplates();

            if (allPlayers == null)
            {
                return new PlayerCharactersCollection(scenarioId);
            }

            PlayerCharactersCollection players = new PlayerCharactersCollection(scenarioId);

            for (int i = 0; i < allPlayers.Count; i++)
            {
                PlayerCharacterTemplate player = allPlayers[i];

                if (player != null && player.ScenarioId == scenarioId)
                {
                    players.AddPlayer(player);
                }
            }

            return players;
        }

        public override Scenario? GetScenarioById(int scenarioId)
        {
            List<Scenario> scenarios = GetAllScenarios();

            if (scenarios == null)
            {
                return null;
            }

            for (int i = 0; i < scenarios.Count; i++)
            {
                Scenario scenario = scenarios[i];

                if (scenario != null && scenario.Id == scenarioId)
                {
                    return scenario;
                }
            }

            return null;
        }

        public override Scene? GetSceneById(int sceneId)
        {
            ScenesCollection scenes = GetAllScenes();

            if (scenes == null)
            {
                return null;
            }

            return scenes.GetById(sceneId);
        }

        public override ScenesCollection GetScenesByScenarioId(int scenarioId)
        {
            ScenesCollection allScenes = GetAllScenes();

            if (allScenes == null)
            {
                return new ScenesCollection(scenarioId);
            }

            ScenesCollection scenes = new ScenesCollection(scenarioId);

            for (int i = 0; i < allScenes.Count; i++)
            {
                Scene scene = allScenes[i];

                if (scene != null && scene.ScenarioId == scenarioId)
                {
                    scenes.AddScene(scene);
                }
            }

            return scenes;
        }

        public override Shop? GetShopById(int shopId)
        {
            ShopsCollection shops = GetAllShops();

            if (shops == null)
            {
                return null;
            }

            return shops.GetById(shopId);
        }

        public override ShopsCollection GetShopsByScenarioId(int scenarioId)
        {
            ShopsCollection allShops = GetAllShops();

            if (allShops == null)
            {
                return new ShopsCollection(scenarioId);
            }

            ShopsCollection shops = new ShopsCollection(scenarioId);

            for (int i = 0; i < allShops.Count; i++)
            {
                Shop shop = allShops[i];

                if (shop != null && shop.ScenarioId == scenarioId)
                {
                    shops.AddShop(shop);
                }
            }

            return shops;
        }

        public override Scenario? LoadScenario(int scenarioId)
        {
            return GetScenarioById(scenarioId);
        }

        public override void UpdateAllScenarios(List<Scenario> scenarios)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("SCENARIOS");

            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(scenarios, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(scenes, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(choices, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(conditions, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(effects, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(enemies, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(shops, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
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
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(playerCharacterTemplates, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
            }
            else
            {
                Console.WriteLine("UpdateAllPlayerCharacterTemplates error can't update datasource file");
            }
        }

        public override void UpdateChoice(Choice choice)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction("CHOICES");

            if (IsValidAccessPath)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = JsonConvert.SerializeObject(choice, Formatting.Indented, settings);
                File.WriteAllText(AccessPath, json);
                ClearCache();
            }
            else
            {
                Console.WriteLine("UpdateChoice error can't update datasource file");
            }
        }

        public override void UpdateCondition(Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEffect(Effect effect)
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

        public override void AddCondition(Condition condition)
        {
            throw new NotImplementedException();
        }

        public override void AddEffect(Effect effect)
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

        public override void SaveScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }
    }
}