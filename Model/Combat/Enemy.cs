using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Utilities.EntriesValidation;
using ProjetPOO.Utilities.Randomization;

namespace ProjetPOO.Model.Combat
{
    public class Enemy : Character
    {
        private const int LOW_DROP_CHANCE = 0;
        private const int HIGH_DROP_CHANCE = 100;
        private const int MINIMUM_ENEMY_NAME_LENGTH = 3;
        private const int MAXIMUM_ENEMY_NAME_LENGTH = 50;

        private static int _nextId = 1;

        private int _id;
        private int _scenarioId;

        private string? _enemyName;
        private int _enemyRaceId;
        private int _rewardExperience;

        private int _rewardGoldMin;
        private int _rewardGoldMax;

        private int _potionDropChance;
        private int _potionAmountMin;
        private int _potionAmountMax;

        private int _keyDropChance;
        private int _keyAmountMin;
        private int _keyAmountMax;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _scenarioId = value;
                }
            }
        }

        public string? EnemyName
        {
            get => _enemyName;
            private set
            {
                if (value == null || value == string.Empty || ValidUtils.CheckEntryName(value, MINIMUM_ENEMY_NAME_LENGTH, MAXIMUM_ENEMY_NAME_LENGTH))
                {
                    _enemyName = value;
                }
            }
        }

        public int EnemyRaceId
        {
            get => _enemyRaceId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _enemyRaceId = value;
                }
            }
        }


        public int RewardExperience
        {
            get => _rewardExperience;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _rewardExperience = value;
            }
        }

        public int RewardGoldMin
        {
            get => _rewardGoldMin;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _rewardGoldMin = value;
                }
            }
        }

        public int RewardGoldMax
        {
            get => _rewardGoldMax;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _rewardGoldMax = value;
                }
            }
        }

        public int PotionDropChance
        {
            get => _potionDropChance;
            private set
            {
                if (value >= LOW_DROP_CHANCE && value <= HIGH_DROP_CHANCE)
                {
                    _potionDropChance = value;
                }
            }
        }

        public int PotionAmountMin
        {
            get => _potionAmountMin;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _potionAmountMin = value;
                }
            }
        }

        public int PotionAmountMax
        {
            get => _potionAmountMax;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _potionAmountMax = value;
                }
            }
        }


        public int KeyDropChance
        {
            get => _keyDropChance;
            private set
            {
                if (value >= LOW_DROP_CHANCE && value <= HIGH_DROP_CHANCE)
                {
                    _keyDropChance = value;
                }
            }
        }

        public int KeyAmountMin
        {
            get => _keyAmountMin;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _keyAmountMin = value;
                }
            }
        }

        public int KeyAmountMax
        {
            get => _keyAmountMax;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _keyAmountMax = value;
                }
            }
        }

        // Constructeur "normal" (en mémoire)
        public Enemy(string? enemyName, int enemyRaceId, int maxHp, int attack, int defense,  int agility, int rewardExperience, int rewardGoldMin, int rewardGoldMax, int potionDropChance,int potionAmountMin, int potionAmountMax, int keyDropChance, int keyAmountMin,
            int keyAmountMax) : base(GetCharacterName(enemyName, enemyRaceId), maxHp, attack, defense, agility)
        {
            Id = GenerateId();

            ScenarioId = 0; // draft-friendly

            EnemyName = enemyName;
            EnemyRaceId = enemyRaceId;

            RewardExperience = rewardExperience;

            RewardGoldMin = rewardGoldMin;
            RewardGoldMax = rewardGoldMax;

            PotionDropChance = potionDropChance;
            PotionAmountMin = potionAmountMin;
            PotionAmountMax = potionAmountMax;

            KeyDropChance = keyDropChance;
            KeyAmountMin = keyAmountMin;
            KeyAmountMax = keyAmountMax;

            ValidateRewardRanges();
        }

        // Constructeur privé pour Load (évite de dupliquer la logique)
        private Enemy( int id,int scenarioId, string? enemyName, int enemyRaceId, int maxHp,int attack, int defense,int agility,int rewardExperience,int rewardGoldMin,int rewardGoldMax,int potionDropChance,int potionAmountMin,int potionAmountMax,int keyDropChance,int keyAmountMin,
            int keyAmountMax) : base(GetCharacterName(enemyName, enemyRaceId), maxHp, attack, defense, agility)
        {
            Id = id;
            ScenarioId = scenarioId;

            EnemyName = enemyName;
            EnemyRaceId = enemyRaceId;

            RewardExperience = rewardExperience;

            RewardGoldMin = rewardGoldMin;
            RewardGoldMax = rewardGoldMax;

            PotionDropChance = potionDropChance;
            PotionAmountMin = potionAmountMin;
            PotionAmountMax = potionAmountMax;

            KeyDropChance = keyDropChance;
            KeyAmountMin = keyAmountMin;
            KeyAmountMax = keyAmountMax;

            ValidateRewardRanges();
        }

        public void AssignToScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être >= 0.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
        }

        public void ClearScenario()
        {
            ScenarioId = 0;
        }

        private static int GenerateId()
        {
            int id = _nextId;
            _nextId++;
            return id;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (loadedId >= _nextId)
            {
                _nextId = loadedId + 1;
            }
        }

        // Constructeur pour Load (depuis la base de données)
        public static Enemy Load( int id, int scenarioId, string? enemyName,int enemyRaceId, int maxHp,int attack,int defense, int agility, int rewardExperience,int rewardGoldMin, int rewardGoldMax,int potionDropChance,int potionAmountMin, int potionAmountMax, int keyDropChance, int keyAmountMin, int keyAmountMax)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être >= 0.", nameof(scenarioId));
            }

            if (!ValidUtils.CheckIfPositiveNumber(enemyRaceId))
            {
                throw new ArgumentException("enemyRaceId doit être un nombre positif.", nameof(enemyRaceId));
            }

            Enemy enemy = new Enemy(id,scenarioId, enemyName,enemyRaceId, maxHp,attack, defense,agility,rewardExperience,rewardGoldMin,rewardGoldMax, potionDropChance,potionAmountMin, potionAmountMax,keyDropChance,keyAmountMin, keyAmountMax);

            EnsureNextIdIsAfterLoadedId(id);

            return enemy;
        }

        public Loot GetLoot()
        {

            int goldAmount = RandomUtils.GetRandomNumber(RewardGoldMin, RewardGoldMax);

            int potionAmount = 0;
            if (PotionDropChance > 0 && RandomUtils.CheckChance(PotionDropChance))
            {
                potionAmount = RandomUtils.GetRandomNumber(PotionAmountMin, PotionAmountMax);
            }

            int keyAmount = 0;
            if (KeyDropChance > 0 && RandomUtils.CheckChance(KeyDropChance))
            {
                keyAmount = RandomUtils.GetRandomNumber(KeyAmountMin, KeyAmountMax);
            }

            return new Loot(goldAmount, potionAmount, keyAmount);
        }

        public int GetExperienceReward()
        {
            return RewardExperience;
        }

        private void ValidateRewardRanges()
        {
            if (RewardGoldMin > RewardGoldMax)
            {
                throw new InvalidOperationException("RewardGoldMin ne peut pas être supérieur à RewardGoldMax.");
            }

            if (PotionAmountMin > PotionAmountMax)
            {
                throw new InvalidOperationException("PotionAmountMin ne peut pas être supérieur à PotionAmountMax.");
            }

            if (KeyAmountMin > KeyAmountMax)
            {
                throw new InvalidOperationException("KeyAmountMin ne peut pas être supérieur à KeyAmountMax.");
            }

            if (PotionDropChance > 0 && PotionAmountMax == 0)
            {
                throw new InvalidOperationException("PotionDropChance > 0 mais PotionAmountMax vaut 0 (loot potion impossible).");
            }

            if (KeyDropChance > 0 && KeyAmountMax == 0)
            {
                throw new InvalidOperationException("KeyDropChance > 0 mais KeyAmountMax vaut 0 (loot clé impossible).");
            }
        }

        private static string GetCharacterName(string? enemyName, int enemyRaceId)
        {
            if (!string.IsNullOrWhiteSpace(enemyName))
            {
                return enemyName;
            }

            return "Ennemi race " + enemyRaceId;
        }

        public void RenameEnemy(string? enemyName)
        {
            EnemyName = enemyName;
        }

        public void ChangeEnemyRace(int enemyRaceId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(enemyRaceId))
            {
                throw new ArgumentException("enemyRaceId doit être un nombre positif.", nameof(enemyRaceId));
            }

            EnemyRaceId = enemyRaceId;
        }

    }
}
