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
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

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

        #region Choice region
        public override void AddChoice(Choice choice)
        {
            if (choice == null)
            {
                throw new ArgumentNullException(nameof(choice));
            }

            string query = @"INSERT INTO [Choice] (SceneId, Label, TargetSceneId) VALUES (@SceneId, @Label, @TargetSceneId)";

            SqlCommandHelper.ExecuteNonQuery(SqlConnection, query, new SqlParameter("@SceneId", SqlDataHelper.ToDbNullableId(choice.SceneId)), new SqlParameter("@Label", choice.Label), new SqlParameter("@TargetSceneId", SqlDataHelper.ToDbNullableId(choice.TargetSceneId)));
        }

        public override void DeleteChoice(int choiceId)
        {
            string deleteConditionsQuery = "DELETE FROM [Condition] WHERE ChoiceId = @ChoiceId";
            string deleteEffectsQuery = "DELETE FROM [Effect] WHERE ChoiceId = @ChoiceId";
            string deleteChoiceQuery = "DELETE FROM [Choice] WHERE Id = @ChoiceId";

            using (SqlTransaction transaction = SqlConnection.BeginTransaction())
            {
                try
                {
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteConditionsQuery, new SqlParameter("@ChoiceId", choiceId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEffectsQuery, new SqlParameter("@ChoiceId", choiceId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteChoiceQuery, new SqlParameter("@ChoiceId", choiceId));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

        public override void UpdateChoice(Choice choice)
        {
            if (choice == null)
            {
                throw new ArgumentNullException(nameof(choice));
            }

            string query = @"UPDATE [Choice] SET SceneId = @SceneId, Label = @Label, TargetSceneId = @TargetSceneId WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", choice.Id),
                new SqlParameter("@SceneId", SqlDataHelper.ToDbNullableId(choice.SceneId)),
                new SqlParameter("@Label", choice.Label),
                new SqlParameter("@TargetSceneId", SqlDataHelper.ToDbNullableId(choice.TargetSceneId))
            );
        }
        #endregion


        #region Condition region
        public override void AddCondition(Condition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            string query = @"INSERT INTO [Condition] (ChoiceId, ConditionTypeId, MinValue) VALUES (@ChoiceId, @ConditionTypeId, @MinValue)";

            SqlCommandHelper.ExecuteNonQuery(SqlConnection, query, new SqlParameter("@ChoiceId", SqlDataHelper.ToDbNullableId(condition.ChoiceId)), new SqlParameter("@ConditionTypeId", SqlDataHelper.ToDbConditionTypeId(condition.Type)), new SqlParameter("@MinValue", condition.MinValue));
        }

        public override void DeleteCondition(int conditionId)
        {
            string query = "DELETE FROM [Condition] WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", conditionId)
            );
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

        public override Condition? GetConditionById(int conditionId)
        {
            string query = "SELECT Id, ChoiceId, ConditionTypeId, MinValue FROM Condition WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", conditionId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Model.Story.Condition condition = Condition.Load(
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

        public override void UpdateCondition(Condition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            string query = @"UPDATE [Condition] SET ChoiceId = @ChoiceId, ConditionTypeId = @ConditionTypeId, MinValue = @MinValue WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", condition.Id),
                new SqlParameter("@ChoiceId", SqlDataHelper.ToDbNullableId(condition.ChoiceId)),
                new SqlParameter("@ConditionTypeId", SqlDataHelper.ToDbConditionTypeId(condition.Type)),
                new SqlParameter("@MinValue", condition.MinValue)
            );
        }
        #endregion


        #region Effect region
        public override void AddEffect(Effect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            string query = @"INSERT INTO [Effect] (ChoiceId, EffectTypeId, Amount, FlagKey) VALUES (@ChoiceId, @EffectTypeId, @Amount, @FlagKey)";

            SqlCommandHelper.ExecuteNonQuery(SqlConnection, query, new SqlParameter("@ChoiceId", SqlDataHelper.ToDbNullableId(effect.ChoiceId)), new SqlParameter("@EffectTypeId", SqlDataHelper.ToDbEffectTypeId(effect.Type)), new SqlParameter("@Amount", SqlDataHelper.ToDbNullableInt(effect.Amount)), new SqlParameter("@FlagKey", SqlDataHelper.ToDbNullableString(effect.FlagKey))
            );
        }

        public override void DeleteEffect(int effectId)
        {
            string query = "DELETE FROM [Effect] WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", effectId)
            );
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
                        Model.Story.Effect effect = Effect.Load(
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

        public override Effect? GetEffectById(int effectId)
        {
            string query = "SELECT Id, ChoiceId, EffectTypeId, Amount, FlagKey FROM Effect WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, SqlConnection))
            {
                command.Parameters.AddWithValue("@Id", effectId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Model.Story.Effect effect = Effect.Load(
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
                        Model.Story.Effect effect = Effect.Load(
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

        public override void UpdateEffect(Effect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            string query = @"UPDATE [Effect] SET ChoiceId = @ChoiceId, EffectTypeId = @EffectTypeId, Amount = @Amount, FlagKey = @FlagKey WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", effect.Id),
                new SqlParameter("@ChoiceId", SqlDataHelper.ToDbNullableId(effect.ChoiceId)),
                new SqlParameter("@EffectTypeId", SqlDataHelper.ToDbEffectTypeId(effect.Type)),
                new SqlParameter("@Amount", SqlDataHelper.ToDbNullableInt(effect.Amount)),
                new SqlParameter("@FlagKey", SqlDataHelper.ToDbNullableString(effect.FlagKey))
            );
        }
        #endregion


        #region Enemy region
        public override void AddEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            string query = @"INSERT INTO [Enemy] (ScenarioId, EnemyName, EnemyRaceId, MaxHp, Attack, Defense, Agility, RewardExperience, RewardGoldMin, RewardGoldMax, PotionDropChance, PotionAmountMin, PotionAmountMax, KeyDropChance, KeyAmountMin, KeyAmountMax) VALUES (@ScenarioId, @EnemyName, @EnemyRaceId, @MaxHp, @Attack, @Defense, @Agility, @RewardExperience, @RewardGoldMin, @RewardGoldMax, @PotionDropChance, @PotionAmountMin, @PotionAmountMax, @KeyDropChance, @KeyAmountMin, @KeyAmountMax)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(enemy.ScenarioId)),
                new SqlParameter("@EnemyName", SqlDataHelper.ToDbNullableString(enemy.EnemyName)),
                new SqlParameter("@EnemyRaceId", enemy.EnemyRaceId),
                new SqlParameter("@MaxHp", enemy.MaxHp),
                new SqlParameter("@Attack", enemy.Attack),
                new SqlParameter("@Defense", enemy.Defense),
                new SqlParameter("@Agility", enemy.Agility),
                new SqlParameter("@RewardExperience", enemy.RewardExperience),
                new SqlParameter("@RewardGoldMin", enemy.RewardGoldMin),
                new SqlParameter("@RewardGoldMax", enemy.RewardGoldMax),
                new SqlParameter("@PotionDropChance", enemy.PotionDropChance),
                new SqlParameter("@PotionAmountMin", enemy.PotionAmountMin),
                new SqlParameter("@PotionAmountMax", enemy.PotionAmountMax),
                new SqlParameter("@KeyDropChance", enemy.KeyDropChance),
                new SqlParameter("@KeyAmountMin", enemy.KeyAmountMin),
                new SqlParameter("@KeyAmountMax", enemy.KeyAmountMax)
            );
        }

        public override void DeleteEnemy(int enemyId)
        {
            string clearScenesQuery = @"UPDATE [Scene] SET EnemyId = NULL WHERE EnemyId = @EnemyId";

            string deleteEnemyQuery = "DELETE FROM [Enemy] WHERE Id = @EnemyId";

            using (SqlTransaction transaction = SqlConnection.BeginTransaction())
            {
                try
                {
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearScenesQuery, new SqlParameter("@EnemyId", enemyId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEnemyQuery, new SqlParameter("@EnemyId", enemyId));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

        public override void UpdateEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            string query = @"UPDATE [Enemy] SET ScenarioId = @ScenarioId, EnemyName = @EnemyName, EnemyRaceId = @EnemyRaceId, MaxHp = @MaxHp, Attack = @Attack, Defense = @Defense, Agility = @Agility, RewardExperience = @RewardExperience, RewardGoldMin = @RewardGoldMin, RewardGoldMax = @RewardGoldMax, PotionDropChance = @PotionDropChance, PotionAmountMin = @PotionAmountMin, PotionAmountMax = @PotionAmountMax, KeyDropChance = @KeyDropChance, KeyAmountMin = @KeyAmountMin, KeyAmountMax = @KeyAmountMax WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", enemy.Id),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(enemy.ScenarioId)),
                new SqlParameter("@EnemyName", SqlDataHelper.ToDbNullableString(enemy.EnemyName)),
                new SqlParameter("@EnemyRaceId", enemy.EnemyRaceId),
                new SqlParameter("@MaxHp", enemy.MaxHp),
                new SqlParameter("@Attack", enemy.Attack),
                new SqlParameter("@Defense", enemy.Defense),
                new SqlParameter("@Agility", enemy.Agility),
                new SqlParameter("@RewardExperience", enemy.RewardExperience),
                new SqlParameter("@RewardGoldMin", enemy.RewardGoldMin),
                new SqlParameter("@RewardGoldMax", enemy.RewardGoldMax),
                new SqlParameter("@PotionDropChance", enemy.PotionDropChance),
                new SqlParameter("@PotionAmountMin", enemy.PotionAmountMin),
                new SqlParameter("@PotionAmountMax", enemy.PotionAmountMax),
                new SqlParameter("@KeyDropChance", enemy.KeyDropChance),
                new SqlParameter("@KeyAmountMin", enemy.KeyAmountMin),
                new SqlParameter("@KeyAmountMax", enemy.KeyAmountMax)
            );
        }
        #endregion



        #region EnemyRace region
        public override void AddEnemyRace(EnemyRace enemyRace)
        {
            if (enemyRace == null)
            {
                throw new ArgumentNullException(nameof(enemyRace));
            }

            string query = @"INSERT INTO [EnemyRace] (ScenarioId, Name, Description) VALUES (@ScenarioId, @Name, @Description)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(enemyRace.ScenarioId)),
                new SqlParameter("@Name", enemyRace.Name),
                new SqlParameter("@Description", SqlDataHelper.ToDbNullableString(enemyRace.Description))
            );
        }

        public override void DeleteEnemyRace(int enemyRaceId)
        {
            string query = "DELETE FROM [EnemyRace] WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", enemyRaceId)
            );
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

        public override void UpdateEnemyRace(EnemyRace enemyRace)
        {
            if (enemyRace == null)
            {
                throw new ArgumentNullException(nameof(enemyRace));
            }

            string query = @"UPDATE [EnemyRace] SET ScenarioId = @ScenarioId, Name = @Name, Description = @Description WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", enemyRace.Id),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(enemyRace.ScenarioId)),
                new SqlParameter("@Name", enemyRace.Name),
                new SqlParameter("@Description", SqlDataHelper.ToDbNullableString(enemyRace.Description))
            );
        }
        #endregion


        #region PlayerCharacterTemplate region
        public override void AddPlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            if (playerCharacterTemplate == null)
            {
                throw new ArgumentNullException(nameof(playerCharacterTemplate));
            }

            string query = @"INSERT INTO [PlayerCharacterTemplate] (ScenarioId, Name, ClassName, RaceName, MaxHp, Attack, Defense, Agility, StartingExperience, StartingLevel) VALUES (@ScenarioId, @Name, @ClassName, @RaceName, @MaxHp, @Attack, @Defense, @Agility, @StartingExperience, @StartingLevel)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(playerCharacterTemplate.ScenarioId)),
                new SqlParameter("@Name", playerCharacterTemplate.Name),
                new SqlParameter("@ClassName", playerCharacterTemplate.ClassName),
                new SqlParameter("@RaceName", playerCharacterTemplate.RaceName),
                new SqlParameter("@MaxHp", playerCharacterTemplate.MaxHp),
                new SqlParameter("@Attack", playerCharacterTemplate.Attack),
                new SqlParameter("@Defense", playerCharacterTemplate.Defense),
                new SqlParameter("@Agility", playerCharacterTemplate.Agility),
                new SqlParameter("@StartingExperience", playerCharacterTemplate.StartingExperience),
                new SqlParameter("@StartingLevel", playerCharacterTemplate.StartingLevel)
            );
        }

        public override void DeletePlayerCharacterTemplate(int playerCharacterTemplateId)
        {
            string query = "DELETE FROM [PlayerCharacterTemplate] WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", playerCharacterTemplateId)
            );
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

        public override void UpdatePlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate)
        {
            if (playerCharacterTemplate == null)
            {
                throw new ArgumentNullException(nameof(playerCharacterTemplate));
            }

            string query = @"UPDATE [PlayerCharacterTemplate] SET ScenarioId = @ScenarioId, Name = @Name, ClassName = @ClassName, RaceName = @RaceName, MaxHp = @MaxHp, Attack = @Attack,  Defense = @Defense, Agility = @Agility, StartingExperience = @StartingExperience, StartingLevel = @StartingLevel WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", playerCharacterTemplate.Id),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(playerCharacterTemplate.ScenarioId)),
                new SqlParameter("@Name", playerCharacterTemplate.Name),
                new SqlParameter("@ClassName", playerCharacterTemplate.ClassName),
                new SqlParameter("@RaceName", playerCharacterTemplate.RaceName),
                new SqlParameter("@MaxHp", playerCharacterTemplate.MaxHp),
                new SqlParameter("@Attack", playerCharacterTemplate.Attack),
                new SqlParameter("@Defense", playerCharacterTemplate.Defense),
                new SqlParameter("@Agility", playerCharacterTemplate.Agility),
                new SqlParameter("@StartingExperience", playerCharacterTemplate.StartingExperience),
                new SqlParameter("@StartingLevel", playerCharacterTemplate.StartingLevel)
            );
        }
        #endregion


        #region Scenario region
        public override void AddScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            string query = @"INSERT INTO [Scenario] (Title, Description, StartSceneId) VALUES (@Title, @Description, @StartSceneId)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Title", scenario.Title),
                new SqlParameter("@Description", scenario.Description),
                new SqlParameter("@StartSceneId", SqlDataHelper.ToDbNullableId(scenario.StartSceneId))
            );
        }

        public override void DeleteScenario(int scenarioId)
        {
            string clearStartSceneQuery = @"UPDATE [Scenario] SET StartSceneId = NULL WHERE Id = @ScenarioId";

            string clearSceneTargetReferencesQuery = @"UPDATE [Scene] SET FleeTargetSceneId = NULL, DefeatTargetSceneId = NULL, VictoryTargetSceneId = NULL WHERE ScenarioId = @ScenarioId";

            string clearChoiceTargetReferencesQuery = @"UPDATE [Choice] SET TargetSceneId = NULL WHERE SceneId IN (SELECT Id FROM [Scene] WHERE ScenarioId = @ScenarioId)";

            string deleteConditionsQuery = @"DELETE FROM [Condition] WHERE ChoiceId IN (SELECT Id FROM [Choice] WHERE SceneId IN ( SELECT Id FROM [Scene] WHERE ScenarioId = @ScenarioId))";

            string deleteEffectsQuery = @"DELETE FROM [Effect] WHERE ChoiceId IN (SELECT Id FROM [Choice] WHERE SceneId IN (SELECT Id FROM [Scene] WHERE ScenarioId = @ScenarioId) )";

            string deleteChoicesQuery = @"DELETE FROM [Choice] WHERE SceneId IN (SELECT Id FROM [Scene] WHERE ScenarioId = @ScenarioId)";

            string deleteScenesQuery = "DELETE FROM [Scene] WHERE ScenarioId = @ScenarioId";
            string deleteEnemiesQuery = "DELETE FROM [Enemy] WHERE ScenarioId = @ScenarioId";
            string deleteEnemyRacesQuery = "DELETE FROM [EnemyRace] WHERE ScenarioId = @ScenarioId";
            string deleteShopsQuery = "DELETE FROM [Shop] WHERE ScenarioId = @ScenarioId";
            string deletePlayerCharactersQuery = "DELETE FROM [PlayerCharacterTemplate] WHERE ScenarioId = @ScenarioId";
            string deleteScenarioQuery = "DELETE FROM [Scenario] WHERE Id = @ScenarioId";

            using (SqlTransaction transaction = SqlConnection.BeginTransaction())
            {
                try
                {
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearStartSceneQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearSceneTargetReferencesQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearChoiceTargetReferencesQuery, new SqlParameter("@ScenarioId", scenarioId));

                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteConditionsQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEffectsQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteChoicesQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteScenesQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEnemiesQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEnemyRacesQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteShopsQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deletePlayerCharactersQuery, new SqlParameter("@ScenarioId", scenarioId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteScenarioQuery, new SqlParameter("@ScenarioId", scenarioId));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

        public override Scenario? GetScenarioById(int scenarioId)
        {
            return LoadScenario(scenarioId);
        }

        public override void UpdateScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            string query = @"UPDATE [Scenario] SET Title = @Title, Description = @Description, StartSceneId = @StartSceneId  WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", scenario.Id),
                new SqlParameter("@Title", scenario.Title),
                new SqlParameter("@Description", scenario.Description),
                new SqlParameter("@StartSceneId", SqlDataHelper.ToDbNullableId(scenario.StartSceneId))
            );
        }
        #endregion


        #region Scene region
        public override void AddScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            string query = @"INSERT INTO [Scene] (Title, Text, SceneTypeId, ScenarioId, PictureFileName, ShopId, EnemyId, FleeTargetSceneId, DefeatTargetSceneId, VictoryTargetSceneId) VALUES (@Title, @Text, @SceneTypeId, @ScenarioId, @PictureFileName, @ShopId, @EnemyId, @FleeTargetSceneId, @DefeatTargetSceneId, @VictoryTargetSceneId)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Title", scene.Title),
                new SqlParameter("@Text", scene.Text),
                new SqlParameter("@SceneTypeId", SqlDataHelper.ToDbSceneTypeId(scene.Type)),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(scene.ScenarioId)),
                new SqlParameter("@PictureFileName", SqlDataHelper.ToDbNullableString(scene.PictureFileName)),
                new SqlParameter("@ShopId", SqlDataHelper.ToDbNullableInt(scene.ShopId)),
                new SqlParameter("@EnemyId", SqlDataHelper.ToDbNullableInt(scene.EnemyId)),
                new SqlParameter("@FleeTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.FleeTargetSceneId)),
                new SqlParameter("@DefeatTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.DefeatTargetSceneId)),
                new SqlParameter("@VictoryTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.VictoryTargetSceneId))
            );
        }

        public override void DeleteScene(int sceneId)
        {
            string clearScenarioStartSceneQuery = @"UPDATE [Scenario] SET StartSceneId = NULL WHERE StartSceneId = @SceneId";

            string clearChoiceTargetSceneQuery = @"UPDATE [Choice] SET TargetSceneId = NULL WHERE TargetSceneId = @SceneId";

            string clearSceneTargetReferencesQuery = @"UPDATE [Scene] SET FleeTargetSceneId = CASE WHEN FleeTargetSceneId = @SceneId THEN NULL ELSE FleeTargetSceneId END, DefeatTargetSceneId = CASE WHEN DefeatTargetSceneId = @SceneId THEN NULL ELSE DefeatTargetSceneId END, VictoryTargetSceneId = CASE WHEN VictoryTargetSceneId = @SceneId THEN NULL ELSE VictoryTargetSceneId END WHERE FleeTargetSceneId = @SceneId OR DefeatTargetSceneId = @SceneId OR VictoryTargetSceneId = @SceneId";

            string deleteConditionsQuery = @"DELETE FROM [Condition] WHERE ChoiceId IN ( SELECT Id FROM [Choice] WHERE SceneId = @SceneId )";

            string deleteEffectsQuery = @"DELETE FROM [Effect] WHERE ChoiceId IN ( SELECT Id FROM [Choice] WHERE SceneId = @SceneId )";

            string deleteChoicesQuery = "DELETE FROM [Choice] WHERE SceneId = @SceneId";
            string deleteSceneQuery = "DELETE FROM [Scene] WHERE Id = @SceneId";

            using (SqlTransaction transaction = SqlConnection.BeginTransaction())
            {
                try
                {
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearScenarioStartSceneQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearChoiceTargetSceneQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearSceneTargetReferencesQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteConditionsQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteEffectsQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteChoicesQuery, new SqlParameter("@SceneId", sceneId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteSceneQuery, new SqlParameter("@SceneId", sceneId));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

        public override void UpdateScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            string query = @"UPDATE [Scene] SET Title = @Title, Text = @Text, SceneTypeId = @SceneTypeId, ScenarioId = @ScenarioId, PictureFileName = @PictureFileName, ShopId = @ShopId, EnemyId = @EnemyId, FleeTargetSceneId = @FleeTargetSceneId, DefeatTargetSceneId = @DefeatTargetSceneId, VictoryTargetSceneId = @VictoryTargetSceneId WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", scene.Id),
                new SqlParameter("@Title", scene.Title),
                new SqlParameter("@Text", scene.Text),
                new SqlParameter("@SceneTypeId", SqlDataHelper.ToDbSceneTypeId(scene.Type)),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(scene.ScenarioId)),
                new SqlParameter("@PictureFileName", SqlDataHelper.ToDbNullableString(scene.PictureFileName)),
                new SqlParameter("@ShopId", SqlDataHelper.ToDbNullableInt(scene.ShopId)),
                new SqlParameter("@EnemyId", SqlDataHelper.ToDbNullableInt(scene.EnemyId)),
                new SqlParameter("@FleeTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.FleeTargetSceneId)),
                new SqlParameter("@DefeatTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.DefeatTargetSceneId)),
                new SqlParameter("@VictoryTargetSceneId", SqlDataHelper.ToDbNullableInt(scene.VictoryTargetSceneId))
            );
        }
        #endregion


        #region Shop region
        public override void AddShop(Shop shop)
        {
            if (shop == null)
            {
                throw new ArgumentNullException(nameof(shop));
            }

            string query = @"INSERT INTO [Shop] (ScenarioId, Name, PotionPrice, KeyPrice) VALUES (@ScenarioId, @Name, @PotionPrice, @KeyPrice)";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(shop.ScenarioId)),
                new SqlParameter("@Name", shop.Name),
                new SqlParameter("@PotionPrice", shop.PotionPrice),
                new SqlParameter("@KeyPrice", shop.KeyPrice)
            );
        }

        public override void DeleteShop(int shopId)
        {
            string clearScenesQuery = @"UPDATE [Scene] SET ShopId = NULL WHERE ShopId = @ShopId";

            string deleteShopQuery = "DELETE FROM [Shop] WHERE Id = @ShopId";

            using (SqlTransaction transaction = SqlConnection.BeginTransaction())
            {
                try
                {
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, clearScenesQuery, new SqlParameter("@ShopId", shopId));
                    SqlCommandHelper.ExecuteNonQuery(SqlConnection, transaction, deleteShopQuery, new SqlParameter("@ShopId", shopId));

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

        public override void UpdateShop(Shop shop)
        {
            if (shop == null)
            {
                throw new ArgumentNullException(nameof(shop));
            }

            string query = @"UPDATE [Shop] SET ScenarioId = @ScenarioId, Name = @Name, PotionPrice = @PotionPrice, KeyPrice = @KeyPrice WHERE Id = @Id";

            SqlCommandHelper.ExecuteNonQuery(
                SqlConnection,
                query,
                new SqlParameter("@Id", shop.Id),
                new SqlParameter("@ScenarioId", SqlDataHelper.ToDbNullableId(shop.ScenarioId)),
                new SqlParameter("@Name", shop.Name),
                new SqlParameter("@PotionPrice", shop.PotionPrice),
                new SqlParameter("@KeyPrice", shop.KeyPrice)
            );
        } 
        #endregion

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

        #region UpdateAll
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
        #endregion
      
    }
}
