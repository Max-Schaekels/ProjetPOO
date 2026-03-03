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
    public class Effect
    {
        private const int MINIMUM_FLAG_KEY_LENGTH = 3;
        private const int MAXIMUM_FLAG_KEY_LENGTH = 100;

        private static int _nextId = 1;

        private int _id;
        private EffectType _type;
        private int? _amount;
        private string? _flagKey;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public EffectType Type
        {
            get => _type;
            private set => _type = value;
        }

        public int? Amount
        {
            get => _amount;
            private set
            {
                if (value == null || ValidUtils.CheckIfNonNegativeNumber(value.Value))
                    _amount = value;
            }
        }

        public string? FlagKey
        {
            get => _flagKey;
            private set
            {
                if (value == null || ValidUtils.CheckEntryName(value, MINIMUM_FLAG_KEY_LENGTH, MAXIMUM_FLAG_KEY_LENGTH))
                    _flagKey = value;
            }
        }

        public Effect(EffectType type, int? amount = null, string? flagKey = null)
        {
            Id = GenerateId();
            Type = type;
            Amount = amount;
            FlagKey = flagKey;
        }

        public Effect()
        {
            Id = GenerateId();
            Type = EffectType.AddGold;
            Amount = 1;
            FlagKey = null;
        }

        public void Apply(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            switch (Type)
            {
                case EffectType.AddGold:
                    state.AddGold(Amount!.Value);
                    break;

                case EffectType.RemoveGold:
                    int goldToRemove = Amount!.Value;

                    if (goldToRemove >= state.Gold)
                    {
                        state.Gold = 0;
                    }
                    else
                    {
                        state.Gold = state.Gold - goldToRemove;
                    }
                    break;

                case EffectType.Damage:
                    state.TakeDamage(Amount!.Value);
                    break;

                case EffectType.AddPotion:
                    state.PlayerInventory.AddPotion(Amount!.Value);
                    break;

                case EffectType.RemovePotion:
                    ConsumeMany(Amount!.Value, state.PlayerInventory.ConsumePotion);
                    break;

                case EffectType.AddKey:
                    state.PlayerInventory.AddKey(Amount!.Value);
                    break;

                case EffectType.RemoveKey:
                    ConsumeMany(Amount!.Value, state.PlayerInventory.ConsumeKey);
                    break;

                case EffectType.SetFlag:
                    state.SetFlag(FlagKey!);
                    break;

                case EffectType.RemoveFlag:
                    state.RemoveFlag(FlagKey!);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), "Type d'effet invalide.");
            }
        }

        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();


            if (!Enum.IsDefined(typeof(EffectType), Type))
            {
                errors.Add("Type d'effet invalide.");
            }

            switch (Type)
            {
                case EffectType.AddGold:
                    ValidateAmount(errors);
                    break;
                case EffectType.RemoveGold:
                    ValidateAmount(errors);
                    break;
                case EffectType.Damage:
                    ValidateAmount(errors);
                    break;
                case EffectType.AddPotion:
                    ValidateAmount(errors);
                    break;
                case EffectType.RemovePotion:
                    ValidateAmount(errors);
                    break;
                case EffectType.AddKey:
                    ValidateAmount(errors);
                    break;
                case EffectType.RemoveKey:
                    ValidateAmount(errors);
                    break;

                case EffectType.SetFlag:
                    ValidateFlag(errors);
                    break;
                case EffectType.RemoveFlag:
                    ValidateFlag(errors);
                    break;

                default:
                    errors.Add("Type d'effet invalide.");
                    break;
            }

            return errors.Count == 0;
        }

        public void ChangeType(EffectType newType)
        {
            if (!Enum.IsDefined(typeof(EffectType), newType))
            {
                throw new ArgumentException("Type d'effet invalide.", nameof(newType));
            }

            Type = newType;
        }

        public void ChangeAmount(int? newAmount)
        {
            if (newAmount != null && !ValidUtils.CheckIfNonNegativeNumber(newAmount.Value))
            {
                throw new ArgumentException("Amount doit être >= 0.", nameof(newAmount));
            }

            Amount = newAmount;
        }

        public void ClearAmount()
        {
            Amount = null;
        }

        public void ChangeFlagKey(string? newFlagKey)
        {
            if (newFlagKey != null && !ValidUtils.CheckEntryName(newFlagKey, MINIMUM_FLAG_KEY_LENGTH, MAXIMUM_FLAG_KEY_LENGTH))
            {
                throw new ArgumentException($"FlagKey doit être compris entre {MINIMUM_FLAG_KEY_LENGTH} et {MAXIMUM_FLAG_KEY_LENGTH} caractères.", nameof(newFlagKey));
            }

            FlagKey = newFlagKey;
        }

        public void ClearFlagKey()
        {
            FlagKey = null;
        }

        private void ValidateAmount(List<string> errors)
        {
            if (Amount == null)
            {
                errors.Add($"{Type} : Amount est requis pour cet effet.");
            }
            else if (!ValidUtils.CheckIfPositiveNumber(Amount.Value))
            {
                errors.Add($"{Type} : Amount doit être un nombre positif (>= 1).");
            }

            if (FlagKey != null)
            {
                errors.Add($"{Type} : FlagKey doit être null pour cet effet.");
            }
        }

        private void ValidateFlag(List<string> errors)
        {
            if (FlagKey == null)
            {
                errors.Add($"{Type} : FlagKey est requis pour cet effet.");
            }

            if (Amount != null)
            {
                errors.Add($"{Type} : Amount doit être null pour cet effet.");
            }
        }

        private void ConsumeMany(int amount, Func<bool> consumeFunc)
        {
            for (int i = 0; i < amount; i++)
            {
                bool consumed = consumeFunc();
                if (!consumed)
                {
                    break;
                }
            }
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

    }
}
