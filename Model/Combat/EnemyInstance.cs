using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Utilities.EntriesValidation;
using ProjetPOO.Utilities.Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class EnemyInstance : Character
    {
        private int _templateId;
        private EnemyType _type;
        private int _rewardExperience;

        private int _rewardGoldMin;
        private int _rewardGoldMax;

        private int _potionDropChance;
        private int _potionAmountMin;
        private int _potionAmountMax;

        private int _keyDropChance;
        private int _keyAmountMin;
        private int _keyAmountMax;

        public int TemplateId
        {
            get => _templateId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _templateId = value;
                }
            }
        }

        public EnemyType Type
        {
            get => _type;
            private set => _type = value;
        }

        public int RewardExperience
        {
            get => _rewardExperience;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _rewardExperience = value;
                }
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
                if (value >= 0 && value <= 100)
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
                if (value >= 0 && value <= 100)
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

        public EnemyInstance(Enemy template)
            : base(template.Name, template.MaxHp, template.Attack, template.Defense, template.Agility)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            TemplateId = template.Id;
            Type = template.Type;
            RewardExperience = template.RewardExperience;

            RewardGoldMin = template.RewardGoldMin;
            RewardGoldMax = template.RewardGoldMax;

            PotionDropChance = template.PotionDropChance;
            PotionAmountMin = template.PotionAmountMin;
            PotionAmountMax = template.PotionAmountMax;

            KeyDropChance = template.KeyDropChance;
            KeyAmountMin = template.KeyAmountMin;
            KeyAmountMax = template.KeyAmountMax;

            ValidateRewardRanges();
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
                throw new InvalidOperationException("PotionDropChance > 0 mais PotionAmountMax vaut 0.");
            }

            if (KeyDropChance > 0 && KeyAmountMax == 0)
            {
                throw new InvalidOperationException("KeyDropChance > 0 mais KeyAmountMax vaut 0.");
            }
        }
    }
}
