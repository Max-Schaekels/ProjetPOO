using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Combat
{
    public abstract class Character
    {
        private const int MINIMUM_NAME_LENGTH = 3;
        private const int MAXIMUM_NAME_LENGTH = 50; 
        private const int MINIMUM_HP = 0;

        private string _name;
        private int _maxHp;
        private int _currentHp;
        private int _attack;
        private int _defense;
        private int _agility;

        public string Name
        {
            get => _name;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
                    _name = value;
            }
        }

        public int MaxHp
        {
            get => _maxHp;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _maxHp = value;

                   
                    if (_currentHp > _maxHp)
                    {
                        _currentHp = _maxHp;
                    }
                }
            }
        }

        public int CurrentHp
        {
            get => _currentHp;
            private set
            {
                if (ValidUtils.IsInRange(value, MINIMUM_HP, _maxHp))
                {
                    _currentHp = value;
                }
            }
        }

        public int Attack
        {
            get => _attack;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _attack = value;
            }
        }

        public int Defense
        {
            get => _defense;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _defense = value;
            }
        }

        public int Agility
        {
            get => _agility;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _agility = value;
            }
        }

        protected Character(string name, int maxHp, int attack, int defense, int agility)
        {
            _maxHp = 1;     // valeurs temporaires pour éviter incohérence au début
            _currentHp = 0;

            Name = name;
            MaxHp = maxHp;
            CurrentHp = MaxHp;

            Attack = attack;
            Defense = defense;
            Agility = agility;
        }

        public bool IsAlive()
        {
            throw new System.NotImplementedException();
        }


        public void ReceiveDamage(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void Heal(int amount)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseMaxHp(int amount)
        {
            throw new NotImplementedException();
        }

        public void IncreaseAttack(int amount)
        {
            throw new NotImplementedException();
        }

        public void IncreaseDefense(int amount)
        {
            throw new NotImplementedException();
        }

        public void IncreaseAgility(int amount)
        {
            throw new NotImplementedException();
        }
    }
}
