using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;
using ProjetPOO.Utilities.Randomization;

namespace ProjetPOO.Model.Combat
{
    public class PlayerCharacter : Character
    {
        private const int LOW_STAT_INCREASE = 1;
        private const int HIGH_STAT_INCREASE = 3;
        private const int BASE_EXPERIENCE_REQUIREMENT = 50;
        private const int EXPERIENCE_INCREMENT = 25;
        private int _id;
        private int _experience;
        private int _level;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int Experience
        {
            get => _experience;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _experience = value;
            }
        }

        public int Level
        {
            get => _level;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _level = value;
            }
        }

        public PlayerCharacter(string name, int maxHp, int attack, int defense, int agility, int experience = 0, int level = 1)
            : base(name, maxHp, attack, defense, agility)
        {
            Experience = experience;
            Level = level;
        }

        public void GainExperience(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                Experience += amount;

                while (CanLevelUp())
                {
                    LevelUp();
                }
            }
        }

        public bool CanLevelUp()
        {
            return Experience >= GetExperienceRequiredForLevel(Level + 1);
        }

        private void LevelUp()
        {
            int required = GetExperienceRequiredForLevel(Level + 1);
            Experience -= required;
            Level++;
            IncreaseMaxHp(RandomUtils.GetRandomNumber(LOW_STAT_INCREASE, HIGH_STAT_INCREASE));
            IncreaseAttack(RandomUtils.GetRandomNumber(LOW_STAT_INCREASE, HIGH_STAT_INCREASE));
            IncreaseDefense(RandomUtils.GetRandomNumber(LOW_STAT_INCREASE, HIGH_STAT_INCREASE));
            IncreaseAgility(RandomUtils.GetRandomNumber(LOW_STAT_INCREASE, HIGH_STAT_INCREASE));

        }

        private int GetExperienceRequiredForLevel(int level)
        {
            return BASE_EXPERIENCE_REQUIREMENT + (level - 2) * EXPERIENCE_INCREMENT;
        }
    }
   

}
