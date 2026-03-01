using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Game
{
    public class RoundReport
    {
        private int _playerDamage;
        private int _enemyDamage;

        private bool _potionAttempted;
        private bool _potionConsumed;

        private bool _fleeAttempted;
        private bool _fledSuccessfully;

        private int _experienceGained;
        private string _lootDescription;

        private CombatResult _result;

        public int PlayerDamage
        {
            get => _playerDamage;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _playerDamage = value;
                }
            }
        }

        public int EnemyDamage
        {
            get => _enemyDamage;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _enemyDamage = value;
                }
            }
        }

        public bool PotionAttempted
        {
            get => _potionAttempted;
            private set
            {
                _potionAttempted = value;
            }
        }

        public bool PotionConsumed
        {
            get => _potionConsumed;
            private set
            {
                _potionConsumed = value;
            }
        }

        public bool FleeAttempted
        {
            get => _fleeAttempted;
            private set
            {
                _fleeAttempted = value;
            }
        }

        public bool FledSuccessfully
        {
            get => _fledSuccessfully;
            private set
            {
                _fledSuccessfully = value;
            }
        }

        public int ExperienceGained
        {
            get => _experienceGained;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _experienceGained = value;
                }
            }
        }

        public string LootDescription
        {
            get => _lootDescription;
            private set
            {
                
                if (ValidUtils.CheckIfNotNullOrEmpty(value))
                {
                    _lootDescription = value;
                }
            }
        }

        public CombatResult Result
        {
            get => _result;
            private set
            {
                _result = value;
            }
        }

        public RoundReport()
        {          
            LootDescription = string.Empty;
            Result = CombatResult.InProgress;
        }

        public void SetPlayerDamage(int damage)
        {
            PlayerDamage = damage;
        }

        public void SetEnemyDamage(int damage)
        {
            EnemyDamage = damage;
        }

        public void MarkPotionAttempted()
        {
            PotionAttempted = true;
        }

        public void MarkPotionConsumed()
        {
            PotionConsumed = true;
        }

        public void MarkFleeAttempted()
        {
            FleeAttempted = true;
        }

        public void MarkFledSuccessfully()
        {
            FledSuccessfully = true;
        }

        public void SetResult(CombatResult result)
        {
            Result = result;
        }

        public void SetRewards(int experience, string lootDescription)
        {
            ExperienceGained = experience;
            LootDescription = lootDescription;
        }
    }
}
