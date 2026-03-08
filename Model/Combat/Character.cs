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
            private set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
                    _name = value;
            }
        }

        public int MaxHp
        {
            get => _maxHp;
            protected set
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
            protected set
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
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _attack = value;
            }
        }

        public int Defense
        {
            get => _defense;
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _defense = value;
            }
        }

        public int Agility
        {
            get => _agility;
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _agility = value;
            }
        }

        protected Character(string name, int maxHp, int attack, int defense, int agility)
        {
            _maxHp = 1;     
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
            return CurrentHp > 0;
        }


        public void ReceiveDamage(int amount)
        {
            if (ValidUtils.CheckIfNonNegativeNumber(amount))
            {
                if (amount > CurrentHp)
                {
                    CurrentHp = 0;
                }
                else
                {
                    CurrentHp -= amount;
                }
            }
            
        }

        public void Heal(int amount)
        {
            if(ValidUtils.CheckIfPositiveNumber(amount))
            {
                if (CurrentHp + amount > MaxHp)
                {
                    CurrentHp = MaxHp;
                }
                else
                {
                    CurrentHp += amount;
                }
            }
        }

        public void IncreaseMaxHp(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                MaxHp += amount;
                CurrentHp += amount;
            }
        }

        public void IncreaseAttack(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                Attack += amount;
            }
        }

        public void IncreaseDefense(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                Defense += amount;
            }
        }

        public void IncreaseAgility(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                Agility += amount;
            }
        }

        public void Rename(string name)
        {
            if (!ValidUtils.CheckEntryName(name, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
            {
                throw new ArgumentException(
                    $"Name doit être compris entre {MINIMUM_NAME_LENGTH} et {MAXIMUM_NAME_LENGTH} caractères.",
                    nameof(name));
            }

            Name = name;
        }
    }
}
