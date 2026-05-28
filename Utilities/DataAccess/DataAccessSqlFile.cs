using Microsoft.Data.SqlClient;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.DataAccess.Helpers;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess
{
    public class DataAccessSqlFile : DataAccess, IDataAccess
    {

        public DataAccessSqlFile(DataFilesManager dataFilesManager, IAlertService alertService) : base(dataFilesManager)
        {
            try
            {
                AccessPath = DataFilesManager.DataFiles.GetValueByCodeFunction("CONNECTION_STRING");

                SqlConnection = new SqlConnection(AccessPath);
                SqlConnection.Open();

                System.Diagnostics.Debug.WriteLine("Connexion SQL réussie vers la base ProjetPOO.");
            }
            catch (Exception exception)
            {
                alertService.ShowAlert("Erreur SQL", exception.Message);
                throw;
            }
        }

        public SqlConnection SqlConnection { get; private set; }

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

        public override void AddEnemyRace(EnemyRace enemyRace)
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

        public override void DeleteEnemyRace(int enemyRaceId)
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

        public override ChoicesCollection GetAllChoices()
        {
            ChoicesCollection choices = new ChoicesCollection();

            string query = "SELECT Id, SceneId, Label, TargetSceneId FROM Choice ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int choiceId = SqlDataHelper.ReadInt(reader, "Id");

                        ConditionsCollection conditions = GetConditionsByChoiceId(choiceId);
                        EffectsCollection effects = GetEffectsByChoiceId(choiceId);

                        Choice choice = Choice.Load(
                            choiceId,
                            SqlDataHelper.ReadString(reader, "Label"),
                            SqlDataHelper.ReadNullableId(reader, "TargetSceneId"),
                            SqlDataHelper.ReadNullableId(reader, "SceneId"),
                            conditions,
                            effects
                        );

                        choices.AddChoice(choice);
                    }
                }
            }

            return choices;
        }

        public override ConditionsCollection GetAllConditions()
        {
            ConditionsCollection conditions = new ConditionsCollection();

            string query = "SELECT Id, ChoiceId, ConditionTypeId, MinValue FROM Condition ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Model.Story.Condition condition = Model.Story.Condition.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadConditionType(reader, "ConditionTypeId"),
                            SqlDataHelper.ReadInt(reader, "MinValue")
                        );

                        conditions.AddCondition(condition);
                    }
                }
            }

            return conditions;
        }

        public override EffectsCollection GetAllEffects()
        {
            EffectsCollection effects = new EffectsCollection();

            string query = "SELECT Id, ChoiceId, EffectTypeId, Amount, FlagKey FROM Effect ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Model.Story.Effect effect = Model.Story.Effect.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadEffectType(reader, "EffectTypeId"),
                            SqlDataHelper.ReadNullableInt(reader, "Amount"),
                            SqlDataHelper.ReadOptionalString(reader, "FlagKey")
                        );

                        effects.AddEffect(effect);
                    }
                }
            }

            return effects;
        }

        public override EnemiesCollection GetAllEnemies()
        {
            EnemiesCollection enemies = new EnemiesCollection();

            string query = "SELECT Id, ScenarioId, EnemyName, EnemyRaceId, MaxHp, Attack, Defense, Agility, RewardExperience, RewardGoldMin, RewardGoldMax, PotionDropChance, PotionAmountMin, PotionAmountMax, KeyDropChance, KeyAmountMin, KeyAmountMax FROM Enemy ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Enemy enemy = Enemy.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "EnemyName"),
                            SqlDataHelper.ReadInt(reader, "EnemyRaceId"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "RewardExperience"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMin"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMax"),
                            SqlDataHelper.ReadInt(reader, "PotionDropChance"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMin"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMax"),
                            SqlDataHelper.ReadInt(reader, "KeyDropChance"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMin"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMax")
                        );

                        enemies.AddEnemy(enemy);
                    }
                }
            }

            return enemies;
        }

        public override EnemyRacesCollection GetAllEnemyRaces()
        {
            EnemyRacesCollection enemyRaces = new EnemyRacesCollection();

            string query = "SELECT Id, ScenarioId, Name, Description FROM EnemyRace ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EnemyRace enemyRace = EnemyRace.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadNullableString(reader, "Description")
                        );

                        enemyRaces.Add(enemyRace);
                    }
                }
            }

            return enemyRaces;
        }

        public override PlayerCharactersCollection GetAllPlayerCharacterTemplates()
        {
            PlayerCharactersCollection playerCharacters = new PlayerCharactersCollection();

            string query = "SELECT Id, ScenarioId, Name, ClassName, RaceName, MaxHp, Attack, Defense, Agility, StartingExperience, StartingLevel FROM PlayerCharacterTemplate ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerCharacterTemplate playerCharacter = PlayerCharacterTemplate.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadString(reader, "ClassName"),
                            SqlDataHelper.ReadString(reader, "RaceName"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "StartingExperience"),
                            SqlDataHelper.ReadInt(reader, "StartingLevel")
                        );

                        playerCharacters.AddPlayer(playerCharacter);
                    }
                }
            }

            return playerCharacters;
        }

        public override List<Scenario> GetAllScenarios()
        {
            List<int> scenarioIds = new List<int>();
            List<Scenario> scenarios = new List<Scenario>();

            string query = "SELECT Id FROM Scenario ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        scenarioIds.Add(SqlDataHelper.ReadInt(reader, "Id"));
                    }
                }
            }

            for (int i = 0; i < scenarioIds.Count; i++)
            {
                Scenario? scenario = LoadScenario(scenarioIds[i]);

                if (scenario != null)
                {
                    scenarios.Add(scenario);
                }
            }

            return scenarios;
        }

        public override ScenesCollection GetAllScenes()
        {
            ScenesCollection scenes = new ScenesCollection();

            string query = "SELECT Id, Title, Text, SceneTypeId, ScenarioId, PictureFileName, ShopId, EnemyId, FleeTargetSceneId, DefeatTargetSceneId, VictoryTargetSceneId FROM Scene ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int sceneId = SqlDataHelper.ReadInt(reader, "Id");

                        ChoicesCollection choices = GetChoicesBySceneId(sceneId);

                        Scene scene = Scene.Load(
                            sceneId,
                            SqlDataHelper.ReadString(reader, "Title"),
                            SqlDataHelper.ReadString(reader, "Text"),
                            SqlDataHelper.ReadSceneType(reader, "SceneTypeId"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "PictureFileName"),
                            SqlDataHelper.ReadNullableInt(reader, "ShopId"),
                            SqlDataHelper.ReadNullableInt(reader, "EnemyId"),
                            SqlDataHelper.ReadNullableInt(reader, "FleeTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "DefeatTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "VictoryTargetSceneId"),
                            choices
                        );

                        scenes.AddScene(scene);
                    }
                }
            }

            return scenes;
        }

        public override ShopsCollection GetAllShops()
        {
            ShopsCollection shops = new ShopsCollection();

            string query = "SELECT Id, ScenarioId, Name, PotionPrice, KeyPrice FROM Shop ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Shop shop = Shop.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadInt(reader, "PotionPrice"),
                            SqlDataHelper.ReadInt(reader, "KeyPrice")
                        );

                        shops.AddShop(shop);
                    }
                }
            }

            return shops;
        }

        public override Choice? GetChoiceById(int choiceId)
        {
            string query = "SELECT Id, SceneId, Label, TargetSceneId FROM Choice WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", choiceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ConditionsCollection conditions = GetConditionsByChoiceId(choiceId);
                        EffectsCollection effects = GetEffectsByChoiceId(choiceId);

                        Choice choice = Choice.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadString(reader, "Label"),
                            SqlDataHelper.ReadNullableId(reader, "TargetSceneId"),
                            SqlDataHelper.ReadNullableId(reader, "SceneId"),
                            conditions,
                            effects
                        );

                        return choice;
                    }
                }
            }

            return null;
        }

        public override ChoicesCollection GetChoicesBySceneId(int sceneId)
        {
            ChoicesCollection choices = new ChoicesCollection(sceneId);

            string query = "SELECT Id, SceneId, Label, TargetSceneId FROM Choice WHERE SceneId = @SceneId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@SceneId", sceneId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int choiceId = SqlDataHelper.ReadInt(reader, "Id");

                        ConditionsCollection conditions = GetConditionsByChoiceId(choiceId);
                        EffectsCollection effects = GetEffectsByChoiceId(choiceId);

                        Choice choice = Choice.Load(
                            choiceId,
                            SqlDataHelper.ReadString(reader, "Label"),
                            SqlDataHelper.ReadNullableId(reader, "TargetSceneId"),
                            SqlDataHelper.ReadNullableId(reader, "SceneId"),
                            conditions,
                            effects
                        );

                        choices.AddChoice(choice);
                    }
                }
            }

            return choices;
        }

        public override Model.Story.Condition? GetConditionById(int conditionId)
        {
            string query = "SELECT Id, ChoiceId, ConditionTypeId, MinValue FROM Condition WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", conditionId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Model.Story.Condition condition = Model.Story.Condition.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadConditionType(reader, "ConditionTypeId"),
                            SqlDataHelper.ReadInt(reader, "MinValue")
                        );

                        return condition;
                    }
                }
            }

            return null;
        }

        public override ConditionsCollection GetConditionsByChoiceId(int choiceId)
        {
            ConditionsCollection conditions = new ConditionsCollection(choiceId);

            string query = "SELECT Id, ChoiceId, ConditionTypeId, MinValue FROM Condition WHERE ChoiceId = @ChoiceId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ChoiceId", choiceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Model.Story.Condition condition = Model.Story.Condition.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadConditionType(reader, "ConditionTypeId"),
                            SqlDataHelper.ReadInt(reader, "MinValue")
                        );

                        conditions.AddCondition(condition);
                    }
                }
            }

            return conditions;
        }

        public override Model.Story.Effect? GetEffectById(int effectId)
        {
            string query = "SELECT Id, ChoiceId, EffectTypeId, Amount, FlagKey FROM Effect WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", effectId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Model.Story.Effect effect = Model.Story.Effect.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadEffectType(reader, "EffectTypeId"),
                            SqlDataHelper.ReadNullableInt(reader, "Amount"),
                            SqlDataHelper.ReadOptionalString(reader, "FlagKey")
                        );

                        return effect;
                    }
                }
            }

            return null;
        }

        public override EffectsCollection GetEffectsByChoiceId(int choiceId)
        {
            EffectsCollection effects = new EffectsCollection(choiceId);

            string query = "SELECT Id, ChoiceId, EffectTypeId, Amount, FlagKey FROM Effect WHERE ChoiceId = @ChoiceId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ChoiceId", choiceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Model.Story.Effect effect = Model.Story.Effect.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ChoiceId"),
                            SqlDataHelper.ReadEffectType(reader, "EffectTypeId"),
                            SqlDataHelper.ReadNullableInt(reader, "Amount"),
                            SqlDataHelper.ReadOptionalString(reader, "FlagKey")
                        );

                        effects.AddEffect(effect);
                    }
                }
            }

            return effects;
        }

        public override EnemiesCollection GetEnemiesByScenarioId(int scenarioId)
        {
            EnemiesCollection enemies = new EnemiesCollection(scenarioId);

            string query = "SELECT Id, ScenarioId, EnemyName, EnemyRaceId, MaxHp, Attack, Defense, Agility, RewardExperience, RewardGoldMin, RewardGoldMax, PotionDropChance, PotionAmountMin, PotionAmountMax, KeyDropChance, KeyAmountMin, KeyAmountMax FROM Enemy WHERE ScenarioId = @ScenarioId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ScenarioId", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Enemy enemy = Enemy.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "EnemyName"),
                            SqlDataHelper.ReadInt(reader, "EnemyRaceId"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "RewardExperience"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMin"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMax"),
                            SqlDataHelper.ReadInt(reader, "PotionDropChance"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMin"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMax"),
                            SqlDataHelper.ReadInt(reader, "KeyDropChance"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMin"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMax")
                        );

                        enemies.AddEnemy(enemy);
                    }
                }
            }

            return enemies;
        }

        public override Enemy? GetEnemyById(int enemyId)
        {
            string query = "SELECT Id, ScenarioId, EnemyName, EnemyRaceId, MaxHp, Attack, Defense, Agility, RewardExperience, RewardGoldMin, RewardGoldMax, PotionDropChance, PotionAmountMin, PotionAmountMax, KeyDropChance, KeyAmountMin, KeyAmountMax FROM Enemy WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", enemyId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Enemy enemy = Enemy.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "EnemyName"),
                            SqlDataHelper.ReadInt(reader, "EnemyRaceId"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "RewardExperience"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMin"),
                            SqlDataHelper.ReadInt(reader, "RewardGoldMax"),
                            SqlDataHelper.ReadInt(reader, "PotionDropChance"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMin"),
                            SqlDataHelper.ReadInt(reader, "PotionAmountMax"),
                            SqlDataHelper.ReadInt(reader, "KeyDropChance"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMin"),
                            SqlDataHelper.ReadInt(reader, "KeyAmountMax")
                        );

                        return enemy;
                    }
                }
            }

            return null;
        }

        public override EnemyRace? GetEnemyRaceById(int enemyRaceId)
        {
            string query = "SELECT Id, ScenarioId, Name, Description FROM EnemyRace WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", enemyRaceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        EnemyRace enemyRace = EnemyRace.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadNullableString(reader, "Description")
                        );

                        return enemyRace;
                    }
                }
            }

            return null;
        }

        public override EnemyRacesCollection GetEnemyRacesByScenarioId(int scenarioId)
        {
            EnemyRacesCollection enemyRaces = new EnemyRacesCollection(scenarioId);

            string query = "SELECT Id, ScenarioId, Name, Description FROM EnemyRace WHERE ScenarioId = @ScenarioId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ScenarioId", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EnemyRace enemyRace = EnemyRace.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadNullableString(reader, "Description")
                        );

                        enemyRaces.Add(enemyRace);
                    }
                }
            }

            return enemyRaces;
        }

        public override PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId)
        {
            string query = "SELECT Id, ScenarioId, Name, ClassName, RaceName, MaxHp, Attack, Defense, Agility, StartingExperience, StartingLevel FROM PlayerCharacterTemplate WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", playerCharacterTemplateId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        PlayerCharacterTemplate playerCharacter = PlayerCharacterTemplate.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadString(reader, "ClassName"),
                            SqlDataHelper.ReadString(reader, "RaceName"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "StartingExperience"),
                            SqlDataHelper.ReadInt(reader, "StartingLevel")
                        );

                        return playerCharacter;
                    }
                }
            }

            return null;
        }

        public override PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId)
        {
            PlayerCharactersCollection playerCharacters = new PlayerCharactersCollection(scenarioId);

            string query = "SELECT Id, ScenarioId, Name, ClassName, RaceName, MaxHp, Attack, Defense, Agility, StartingExperience, StartingLevel FROM PlayerCharacterTemplate WHERE ScenarioId = @ScenarioId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ScenarioId", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerCharacterTemplate playerCharacter = PlayerCharacterTemplate.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadString(reader, "ClassName"),
                            SqlDataHelper.ReadString(reader, "RaceName"),
                            SqlDataHelper.ReadInt(reader, "MaxHp"),
                            SqlDataHelper.ReadInt(reader, "Attack"),
                            SqlDataHelper.ReadInt(reader, "Defense"),
                            SqlDataHelper.ReadInt(reader, "Agility"),
                            SqlDataHelper.ReadInt(reader, "StartingExperience"),
                            SqlDataHelper.ReadInt(reader, "StartingLevel")
                        );

                        playerCharacters.AddPlayer(playerCharacter);
                    }
                }
            }

            return playerCharacters;
        }

        public override Scenario? GetScenarioById(int scenarioId)
        {
            return LoadScenario(scenarioId);
        }

        public override Scene? GetSceneById(int sceneId)
        {
            string query = "SELECT Id, Title, Text, SceneTypeId, ScenarioId, PictureFileName, ShopId, EnemyId, FleeTargetSceneId, DefeatTargetSceneId, VictoryTargetSceneId FROM Scene WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", sceneId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ChoicesCollection choices = GetChoicesBySceneId(sceneId);

                        Scene scene = Scene.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadString(reader, "Title"),
                            SqlDataHelper.ReadString(reader, "Text"),
                            SqlDataHelper.ReadSceneType(reader, "SceneTypeId"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "PictureFileName"),
                            SqlDataHelper.ReadNullableInt(reader, "ShopId"),
                            SqlDataHelper.ReadNullableInt(reader, "EnemyId"),
                            SqlDataHelper.ReadNullableInt(reader, "FleeTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "DefeatTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "VictoryTargetSceneId"),
                            choices
                        );

                        return scene;
                    }
                }
            }

            return null;
        }

        public override ScenesCollection GetScenesByScenarioId(int scenarioId)
        {
            ScenesCollection scenes = new ScenesCollection(scenarioId);

            string query = "SELECT Id, Title, Text, SceneTypeId, ScenarioId, PictureFileName, ShopId, EnemyId, FleeTargetSceneId, DefeatTargetSceneId, VictoryTargetSceneId FROM Scene WHERE ScenarioId = @ScenarioId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ScenarioId", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int sceneId = SqlDataHelper.ReadInt(reader, "Id");

                        ChoicesCollection choices = GetChoicesBySceneId(sceneId);

                        Scene scene = Scene.Load(
                            sceneId,
                            SqlDataHelper.ReadString(reader, "Title"),
                            SqlDataHelper.ReadString(reader, "Text"),
                            SqlDataHelper.ReadSceneType(reader, "SceneTypeId"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadOptionalString(reader, "PictureFileName"),
                            SqlDataHelper.ReadNullableInt(reader, "ShopId"),
                            SqlDataHelper.ReadNullableInt(reader, "EnemyId"),
                            SqlDataHelper.ReadNullableInt(reader, "FleeTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "DefeatTargetSceneId"),
                            SqlDataHelper.ReadNullableInt(reader, "VictoryTargetSceneId"),
                            choices
                        );

                        scenes.AddScene(scene);
                    }
                }
            }

            return scenes;
        }

        public override Shop? GetShopById(int shopId)
        {
            string query = "SELECT Id, ScenarioId, Name, PotionPrice, KeyPrice FROM Shop WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", shopId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Shop shop = Shop.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadInt(reader, "PotionPrice"),
                            SqlDataHelper.ReadInt(reader, "KeyPrice")
                        );

                        return shop;
                    }
                }
            }

            return null;
        }

        public override ShopsCollection GetShopsByScenarioId(int scenarioId)
        {
            ShopsCollection shops = new ShopsCollection(scenarioId);

            string query = "SELECT Id, ScenarioId, Name, PotionPrice, KeyPrice FROM Shop WHERE ScenarioId = @ScenarioId ORDER BY Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@ScenarioId", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Shop shop = Shop.Load(
                            SqlDataHelper.ReadInt(reader, "Id"),
                            SqlDataHelper.ReadNullableId(reader, "ScenarioId"),
                            SqlDataHelper.ReadString(reader, "Name"),
                            SqlDataHelper.ReadInt(reader, "PotionPrice"),
                            SqlDataHelper.ReadInt(reader, "KeyPrice")
                        );

                        shops.AddShop(shop);
                    }
                }
            }

            return shops;
        }

        public override Scenario? LoadScenario(int scenarioId)
        {
            string query = "SELECT Id, Title, Description, StartSceneId FROM Scenario WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", scenarioId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = SqlDataHelper.ReadInt(reader, "Id");
                        string title = SqlDataHelper.ReadString(reader, "Title");
                        string description = SqlDataHelper.ReadString(reader, "Description");
                        int startSceneId = SqlDataHelper.ReadNullableId(reader, "StartSceneId");

                        reader.Close();

                        ScenesCollection scenes = GetScenesByScenarioId(id);
                        EnemyRacesCollection enemyRaces = GetEnemyRacesByScenarioId(id);
                        EnemiesCollection enemies = GetEnemiesByScenarioId(id);
                        ShopsCollection shops = GetShopsByScenarioId(id);
                        PlayerCharactersCollection playerCharacters = GetPlayerCharacterTemplatesByScenarioId(id);

                        Scenario scenario = Scenario.Load(
                            id,
                            title,
                            description,
                            startSceneId,
                            scenes,
                            enemies,
                            enemyRaces,
                            shops,
                            playerCharacters
                        );

                        return scenario;
                    }
                }
            }

            return null;
        }

        public override void SaveScenario(Scenario scenario)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllChoices(ChoicesCollection choices)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllConditions(ConditionsCollection conditions)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllEffects(EffectsCollection effects)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllEnemies(EnemiesCollection enemies)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllEnemyRaces(EnemyRacesCollection enemyRaces)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllPlayerCharacterTemplates(PlayerCharactersCollection playerCharacterTemplates)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllScenarios(List<Scenario> scenarios)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllScenes(ScenesCollection scenes)
        {
            throw new NotImplementedException();
        }

        public override void UpdateAllShops(ShopsCollection shops)
        {
            throw new NotImplementedException();
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

        public override void UpdateEnemyRace(EnemyRace enemyRace)
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
    }
}
