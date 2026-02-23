using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Combat
{
    public class PlayerCharacter : Character
    {
        private int _experience;
        private int _level;
        
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

        public PlayerCharacter(string name, int maxHp, int attackPower, int defense, int agility, int experience = 0, int level = 1)
            : base(name, maxHp, attackPower, defense, agility)
        {
            Experience = experience;
            Level = level;
        }

        public void GainExperience(int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool CanLevelUp()
        {
            throw new System.NotImplementedException();
        }

        private void LevelUp()
        {
            throw new System.NotImplementedException();
        }

        private int GetExperienceRequiredForLevel(int level)
        {
            throw new NotImplementedException();
        }
    }
   

}
