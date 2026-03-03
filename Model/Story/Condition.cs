using ProjetPOO.Model.Game;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Story
{
    public class Condition
    {
        private static int _nextId = 1;

        private int _id;
        private ConditionType _type;
        private int _minValue;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public ConditionType Type
        {
            get => _type;
            private set => _type = value;
        }


        public int MinValue
        {
            get => _minValue;
            private set
            {
                if ( ValidUtils.CheckIfPositiveNumber(value))
                    _minValue = value;
            }
        }

        public Condition(ConditionType type, int minValue = 1)
        {
            Id = GenerateId();
            Type = type;
            MinValue = minValue;
        }

        public Condition()
        {
            Id = GenerateId();
            Type = ConditionType.MinGold;
            MinValue = 1;
        }

        public bool Evaluate(GameState state)
        {
            if(state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
                
            if(state.PlayerInventory == null)
            {
                throw new InvalidOperationException("L'inventaire du joueur n'est pas initialisé.");
            }

            switch (Type)
            {
                case ConditionType.HasPotion:
                    return state.PlayerInventory.PotionsCount >= MinValue;
                case ConditionType.HasKey:
                    return state.PlayerInventory.KeysCount >= MinValue;
                case ConditionType.MinGold:
                    return state.Gold >= MinValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), "Type de condition inconnu.");
            }

        }

        public bool Validate(out List<string> errors)
        {
           errors = new List<string>();


            if (!Enum.IsDefined(typeof(ConditionType), Type))
            {
                errors.Add("Type de condition invalide.");
            }

            if (!ValidUtils.CheckIfPositiveNumber(MinValue))
            {
                if(Type == ConditionType.HasPotion)
                {
                    errors.Add("MinValue doit être au moins 1 pour HasPotion.");
                }
                else if(Type == ConditionType.HasKey)
                {
                    errors.Add("MinValue doit être au moins 1 pour HasKey.");
                }
                else if(Type == ConditionType.MinGold)
                {
                    errors.Add("MinValue doit être au moins 1 pour MinGold.");
                }
            }

            return errors.Count == 0;

        }

        public void ChangeMinValue(int newValue)
        {
            if (!ValidUtils.CheckIfPositiveNumber(newValue))
            {
                throw new ArgumentException("MinValue doit être un nombre positif (>= 1).", nameof(newValue));
            }

            MinValue = newValue;
        }

        public void ChangeType(ConditionType newType)
        {
            if (!Enum.IsDefined(typeof(ConditionType), newType))
            {
                throw new ArgumentException("Type de condition invalide.", nameof(newType));
            }

            Type = newType;
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

    }
}
