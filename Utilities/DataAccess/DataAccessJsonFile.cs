using Newtonsoft.Json;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Game;
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
        private EnemyRacesCollection? cachedEnemyRaces;

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
            cachedEnemyRaces = null;
        }

        /// <summary>
        /// Lit un fichier JSON correspondant au code fonction fourni et tente de le convertir en liste d'objets du type demandé.
        /// La méthode prend en charge deux formats : un objet wrapper contenant une propriété Values et une liste JSON directe.
        /// Si le fichier est vide ou si la désérialisation échoue, une liste vide est retournée afin d'éviter de bloquer le chargement.
        /// </summary>
        /// <typeparam name="T">Type des objets attendus dans le fichier JSON.</typeparam>
        /// <param name="codeFunction">Code fonction utilisé pour retrouver le chemin du fichier JSON dans le gestionnaire de fichiers.</param>
        /// <returns>Liste des objets lus depuis le fichier JSON, ou une liste vide si aucune donnée valide n'est trouvée.</returns>
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

        /// <summary>
        /// Écrit une liste d'objets dans le fichier JSON correspondant au code fonction fourni.
        /// Les données sont encapsulées dans un objet wrapper compatible avec la lecture effectuée par ReadJsonValues.
        /// Après l'écriture, le cache est vidé afin que les prochaines lectures utilisent les données mises à jour.
        /// </summary>
        /// <typeparam name="T">Type des objets à écrire dans le fichier JSON.</typeparam>
        /// <param name="codeFunction">Code fonction utilisé pour retrouver le chemin du fichier JSON dans le gestionnaire de fichiers.</param>
        /// <param name="values">Liste des objets à sérialiser dans le fichier JSON.</param>
        private void WriteJsonValues<T>(string codeFunction, List<T> values)
        {
            AccessPath = DataFilesManager.DataFiles.GetFilePathByCodeFunction(codeFunction);

            if (!IsValidAccessPath)
            {
                Console.WriteLine($"WriteJsonValues error can't update datasource file for {codeFunction}");
                return;
            }

            JsonCollectionDto<T> wrapper = new JsonCollectionDto<T>
            {
                Values = values
            };

            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText(AccessPath, json);

            ClearCache();
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
                return new ConditionsCollection();
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
                return new EffectsCollection();
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
                return new ChoicesCollection();
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
                return new ScenesCollection();
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
                return new EnemiesCollection();
            }

            EnemiesCollection enemies = new EnemiesCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                EnemyJsonDto dto = dtos[i];

                Enemy enemy = Enemy.Load( dto.Id, dto.ScenarioId, string.IsNullOrWhiteSpace(dto.EnemyName) ? null : dto.EnemyName, dto.EnemyRaceId,  dto.MaxHp, dto.Attack, dto.Defense, dto.Agility, dto.RewardExperience, dto.RewardGoldMin, dto.RewardGoldMax, dto.PotionDropChance,dto.PotionAmountMin, dto.PotionAmountMax, dto.KeyDropChance, dto.KeyAmountMin, dto.KeyAmountMax);

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
                return new ShopsCollection();
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
                return new PlayerCharactersCollection();
            }

            PlayerCharactersCollection players = new PlayerCharactersCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                PlayerCharacterTemplateJsonDto dto = dtos[i];

                PlayerCharacterTemplate player = PlayerCharacterTemplate.Load(dto.Id, dto.ScenarioId, GetSafeName(dto.Name, "Player chargé"), GetSafeName(dto.ClassName, "Aventurier"),GetSafeName(dto.RaceName, "Humain"), dto.MaxHp, dto.Attack, dto.Defense,dto.Agility,dto.StartingExperience,dto.StartingLevel);

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
                return new List<Scenario>();
            }

            List<Scenario> scenarios = new List<Scenario>();

            for (int i = 0; i < dtos.Count; i++)
            {
                ScenarioJsonDto dto = dtos[i];

                ScenesCollection scenes = GetScenesByScenarioId(dto.Id);
                EnemiesCollection enemies = GetEnemiesByScenarioId(dto.Id);
                EnemyRacesCollection enemyRaces = GetEnemyRacesByScenarioId(dto.Id);
                ShopsCollection shops = GetShopsByScenarioId(dto.Id);
                PlayerCharactersCollection players = GetPlayerCharacterTemplatesByScenarioId(dto.Id);

                Scenario scenario = Scenario.Load( dto.Id, GetSafeTitle(dto.Title),GetSafeDescription(dto.Description),dto.StartSceneId,scenes, enemies, enemyRaces, shops,players);
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
            List<ScenarioJsonDto> dtos = new List<ScenarioJsonDto>();

            for (int i = 0; i < scenarios.Count; i++)
            {
                Scenario scenario = scenarios[i];

                ScenarioJsonDto dto = new ScenarioJsonDto
                {
                    Id = scenario.Id,
                    Title = scenario.Title,
                    Description = scenario.Description,
                    StartSceneId = scenario.StartSceneId
                };

                dtos.Add(dto);
            }

            WriteJsonValues("SCENARIOS", dtos);
        }

        public override void UpdateAllScenes(ScenesCollection scenes)
        {
            List<SceneJsonDto> dtos = new List<SceneJsonDto>();

            for (int i = 0; i < scenes.Count; i++)
            {
                Scene scene = scenes[i];

                SceneJsonDto dto = new SceneJsonDto
                {
                    Id = scene.Id,
                    Title = scene.Title,
                    Text = scene.Text,
                    Type = scene.Type,
                    ScenarioId = scene.ScenarioId,
                    PictureFileName = scene.PictureFileName,
                    ShopId = scene.ShopId,
                    EnemyId = scene.EnemyId,
                    FleeTargetSceneId = scene.FleeTargetSceneId,
                    DefeatTargetSceneId = scene.DefeatTargetSceneId,
                    VictoryTargetSceneId = scene.VictoryTargetSceneId
                };

                dtos.Add(dto);
            }

            WriteJsonValues("SCENES", dtos);
        }

        public override void UpdateAllChoices(ChoicesCollection choices)
        {
            List<ChoiceJsonDto> dtos = new List<ChoiceJsonDto>();

            for (int i = 0; i < choices.Count; i++)
            {
                Choice choice = choices[i];

                ChoiceJsonDto dto = new ChoiceJsonDto
                {
                    Id = choice.Id,
                    Label = choice.Label,
                    TargetSceneId = choice.TargetSceneId,
                    SceneId = choice.SceneId
                };

                dtos.Add(dto);
            }

            WriteJsonValues("CHOICES", dtos);
        }

        public override void UpdateAllConditions(ConditionsCollection conditions)
        {
            List<ConditionJsonDto> dtos = new List<ConditionJsonDto>();

            for (int i = 0; i < conditions.Count; i++)
            {
                Condition condition = conditions[i];

                ConditionJsonDto dto = new ConditionJsonDto
                {
                    Id = condition.Id,
                    ChoiceId = condition.ChoiceId,
                    Type = condition.Type,
                    MinValue = condition.MinValue
                };

                dtos.Add(dto);
            }

            WriteJsonValues("CONDITIONS", dtos);
        }

        public override void UpdateAllEffects(EffectsCollection effects)
        {
            List<EffectJsonDto> dtos = new List<EffectJsonDto>();

            for (int i = 0; i < effects.Count; i++)
            {
                Effect effect = effects[i];

                EffectJsonDto dto = new EffectJsonDto
                {
                    Id = effect.Id,
                    ChoiceId = effect.ChoiceId,
                    Type = effect.Type,
                    Amount = effect.Amount,
                    FlagKey = effect.FlagKey
                };

                dtos.Add(dto);
            }

            WriteJsonValues("EFFECTS", dtos);
        }

        public override void UpdateAllEnemies(EnemiesCollection enemies)
        {
            List<EnemyJsonDto> dtos = new List<EnemyJsonDto>();

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                EnemyJsonDto dto = new EnemyJsonDto
                {
                    Id = enemy.Id,
                    ScenarioId = enemy.ScenarioId,
                    EnemyName = enemy.EnemyName,
                    EnemyRaceId = enemy.EnemyRaceId,
                    MaxHp = enemy.MaxHp,
                    Attack = enemy.Attack,
                    Defense = enemy.Defense,
                    Agility = enemy.Agility,
                    RewardExperience = enemy.RewardExperience,
                    RewardGoldMin = enemy.RewardGoldMin,
                    RewardGoldMax = enemy.RewardGoldMax,
                    PotionDropChance = enemy.PotionDropChance,
                    PotionAmountMin = enemy.PotionAmountMin,
                    PotionAmountMax = enemy.PotionAmountMax,
                    KeyDropChance = enemy.KeyDropChance,
                    KeyAmountMin = enemy.KeyAmountMin,
                    KeyAmountMax = enemy.KeyAmountMax
                };

                dtos.Add(dto);
            }

            WriteJsonValues("ENEMIES", dtos);
        }

        public override void UpdateAllShops(ShopsCollection shops)
        {
            List<ShopJsonDto> dtos = new List<ShopJsonDto>();

            for (int i = 0; i < shops.Count; i++)
            {
                Shop shop = shops[i];

                ShopJsonDto dto = new ShopJsonDto
                {
                    Id = shop.Id,
                    ScenarioId = shop.ScenarioId,
                    Name = shop.Name,
                    PotionPrice = shop.PotionPrice,
                    KeyPrice = shop.KeyPrice
                };

                dtos.Add(dto);
            }

            WriteJsonValues("SHOPS", dtos);
        }

        public override void UpdateAllPlayerCharacterTemplates(PlayerCharactersCollection playerCharacterTemplates)
        {
            List<PlayerCharacterTemplateJsonDto> dtos = new List<PlayerCharacterTemplateJsonDto>();

            for (int i = 0; i < playerCharacterTemplates.Count; i++)
            {
                PlayerCharacterTemplate playerCharacterTemplate = playerCharacterTemplates[i];

                PlayerCharacterTemplateJsonDto dto = new PlayerCharacterTemplateJsonDto
                {
                    Id = playerCharacterTemplate.Id,
                    ScenarioId = playerCharacterTemplate.ScenarioId,
                    Name = playerCharacterTemplate.Name,
                    ClassName = playerCharacterTemplate.ClassName,
                    RaceName = playerCharacterTemplate.RaceName,
                    MaxHp = playerCharacterTemplate.MaxHp,
                    Attack = playerCharacterTemplate.Attack,
                    Defense = playerCharacterTemplate.Defense,
                    Agility = playerCharacterTemplate.Agility,
                    StartingExperience = playerCharacterTemplate.StartingExperience,
                    StartingLevel = playerCharacterTemplate.StartingLevel
                };

                dtos.Add(dto);
            }

            WriteJsonValues("PLAYERCHARACTERS", dtos);
        }

        public override void UpdateChoice(Choice choice)
        {
            throw new NotImplementedException();
        }
        

        public override EnemyRacesCollection GetAllEnemyRaces()
        {
            if (cachedEnemyRaces != null)
            {
                return cachedEnemyRaces;
            }

            List<EnemyRaceJsonDto>? dtos = ReadJsonValues<EnemyRaceJsonDto>("ENEMYRACES");

            if (dtos == null)
            {
                return new EnemyRacesCollection();
            }

            EnemyRacesCollection enemyRaces = new EnemyRacesCollection();

            for (int i = 0; i < dtos.Count; i++)
            {
                EnemyRaceJsonDto dto = dtos[i];

                EnemyRace enemyRace = EnemyRace.Load(
                    dto.Id,
                    dto.ScenarioId,
                    GetSafeName(dto.Name, "Race ennemie"),
                    dto.Description ?? string.Empty);

                enemyRaces.Add(enemyRace);
            }

            cachedEnemyRaces = enemyRaces;
            return cachedEnemyRaces;
        }

        public override EnemyRacesCollection GetEnemyRacesByScenarioId(int scenarioId)
        {
            EnemyRacesCollection allEnemyRaces = GetAllEnemyRaces();

            if (allEnemyRaces == null)
            {
                return new EnemyRacesCollection(scenarioId);
            }

            EnemyRacesCollection enemyRaces = new EnemyRacesCollection(scenarioId);

            foreach (EnemyRace enemyRace in allEnemyRaces)
            {
                if (enemyRace.ScenarioId == 0 || enemyRace.ScenarioId == scenarioId)
                {
                    enemyRaces.Add(enemyRace);
                }
            }

            return enemyRaces;
        }

        public override EnemyRace? GetEnemyRaceById(int enemyRaceId)
        {
            EnemyRacesCollection enemyRaces = GetAllEnemyRaces();

            if (enemyRaces == null)
            {
                return null;
            }

            foreach (EnemyRace enemyRace in enemyRaces)
            {
                if (enemyRace.Id == enemyRaceId)
                {
                    return enemyRace;
                }
            }

            return null;
        }

        public override void AddEnemyRace(EnemyRace enemyRace)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEnemyRace(EnemyRace enemyRace)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllEnemyRaces(EnemyRacesCollection enemyRaces)
        {
            List<EnemyRaceJsonDto> dtos = new List<EnemyRaceJsonDto>();

            for (int i = 0; i < enemyRaces.Count; i++)
            {
                EnemyRace enemyRace = enemyRaces[i];

                EnemyRaceJsonDto dto = new EnemyRaceJsonDto
                {
                    Id = enemyRace.Id,
                    ScenarioId = enemyRace.ScenarioId,
                    Name = enemyRace.Name,
                    Description = enemyRace.Description
                };

                dtos.Add(dto);
            }

            WriteJsonValues("ENEMYRACES", dtos);
        }

        public override void DeleteEnemyRace(int enemyRaceId)
        {
            throw new NotImplementedException();
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

        public override Inventory? GetInventoryById(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public override int AddInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public override void UpdateInventory(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public override void DeleteInventory(int inventoryId)
        {
            throw new NotImplementedException();
        }

        public override PlayerCharacterInstance? GetPlayerCharacterInstanceById(int playerCharacterInstanceId)
        {
            throw new NotImplementedException();
        }

        public override int AddPlayerCharacterInstance(PlayerCharacterInstance playerCharacterInstance)
        {
            throw new NotImplementedException();
        }

        public override void UpdatePlayerCharacterInstance(PlayerCharacterInstance playerCharacterInstance)
        {
            throw new NotImplementedException();
        }

        public override void DeletePlayerCharacterInstance(int playerCharacterInstanceId)
        {
            throw new NotImplementedException();
        }

        public override GameState? GetGameStateById(int gameStateId)
        {
            throw new NotImplementedException();
        }

        public override int AddGameState(GameState gameState, int inventoryId, int playerCharacterInstanceId)
        {
            throw new NotImplementedException();
        }

        public override void UpdateGameState(GameState gameState)
        {
            throw new NotImplementedException();
        }

        public override void DeleteGameState(int gameStateId)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetGameStateFlagsByGameStateId(int gameStateId)
        {
            throw new NotImplementedException();
        }

        public override void AddGameStateFlag(int gameStateId, string flagKey)
        {
            throw new NotImplementedException();
        }

        public override void DeleteGameStateFlagsByGameStateId(int gameStateId)
        {
            throw new NotImplementedException();
        }

        public override List<SaveGame> GetAllSaveGames()
        {
            throw new NotImplementedException();
        }

        public override SaveGame? GetSaveGameById(int saveGameId)
        {
            throw new NotImplementedException();
        }

        public override int AddSaveGame(SaveGame saveGame, int gameStateId)
        {
            throw new NotImplementedException();
        }

        public override void UpdateSaveGame(SaveGame saveGame)
        {
            throw new NotImplementedException();
        }

        public override void DeleteSaveGame(int saveGameId)
        {
            throw new NotImplementedException();
        }
    }
}