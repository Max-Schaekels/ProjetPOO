using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Combat
{
    public class Enemy : Character
    {
        private int _id;
        private EnemyType _type;
        private int _rewardGold;
        private int _rewardExperience;
        private int _rewardPotion;
        private int _rewardKey;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public EnemyType Type
        {
            get => _type;
            set => _type = value;
        }

        public int RewardGold
        {
            get => _rewardGold;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _rewardGold = value;
            }
        }

        public int RewardExperience
        {
            get => _rewardExperience;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _rewardExperience = value;
            }
        }
        public int RewardPotion
        {
            get => _rewardPotion;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _rewardPotion = value;
            }
        }
        public int RewardKey
        {
            get => _rewardKey;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _rewardKey = value;
            }
        }

        public Enemy(string name, int maxHp, int attack, int defense, int agility, EnemyType type, int rewardGold, int rewardExperience, int rewardPotion, int rewardKey)
            : base(name, maxHp, attack, defense, agility)
        {
            Type = type;
            RewardGold = rewardGold;
            RewardExperience = rewardExperience;
            RewardPotion = rewardPotion;
            RewardKey = rewardKey;
        }

        public Loot GetLoot()
        {
            throw new NotImplementedException();
        }


    }
}
