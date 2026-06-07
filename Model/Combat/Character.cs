using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Combat
{
    public abstract class Character : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name
        {
            get => _name;
            private set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH) && _name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int MaxHp
        {
            get => _maxHp;
            protected set
            {
                if (ValidUtils.CheckIfPositiveNumber(value) && _maxHp != value)
                {
                    _maxHp = value;
                    OnPropertyChanged(nameof(MaxHp));

                    if (_currentHp > _maxHp)
                    {
                        CurrentHp = _maxHp;
                    }
                }
            }
        }

        public int CurrentHp
        {
            get => _currentHp;
            protected set
            {
                if (ValidUtils.IsInRange(value, MINIMUM_HP, _maxHp) && _currentHp != value)
                {
                    _currentHp = value;
                    OnPropertyChanged(nameof(CurrentHp));
                }
            }
        }

        public int Attack
        {
            get => _attack;
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value) && _attack != value)
                {
                    _attack = value;
                    OnPropertyChanged(nameof(Attack));
                }
            }
        }

        public int Defense
        {
            get => _defense;
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value) && _defense != value)
                {
                    _defense = value;
                    OnPropertyChanged(nameof(Defense));
                }
            }
        }

        public int Agility
        {
            get => _agility;
            protected set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value) && _agility != value)
                {
                    _agility = value;
                    OnPropertyChanged(nameof(Agility));
                }
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

        /// <summary>
        /// Checks if the character is still alive.
        /// </summary>
        /// <returns>True if current health points are greater than zero, otherwise false.</returns>
        public bool IsAlive()
        {
            return CurrentHp > 0;
        }

        /// <summary>
        /// Applies damage to the character after reducing it with defense.
        /// Current health points cannot go below zero.
        /// </summary>
        /// <param name="amount">Raw damage amount before defense reduction.</param>
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

        /// <summary>
        /// Restores health points to the character.
        /// Current health points cannot exceed maximum health points.
        /// </summary>
        /// <param name="amount">Amount of health points to restore.</param>
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
