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
        private int _id;
        private EffectType _type;
        private int? _amount;
        private string? _flagKey;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public EffectType Type
        {
            get => _type;
            set => _type = value;
        }

        public int? Amount
        {
            get => _amount;
            set
            {
                if (value == null || ValidUtils.CheckIfNonNegativeNumber(value.Value))
                    _amount = value;
            }
        }

        public string? FlagKey
        {
            get => _flagKey;
            set
            {
                if (value == null || ValidUtils.CheckEntryName(value, MINIMUM_FLAG_KEY_LENGTH, MAXIMUM_FLAG_KEY_LENGTH))
                    _flagKey = value;
            }
        }

        public Effect(int id, EffectType type, int? amount = null, string? flagKey = null)
        {
            Id = id;
            Type = type;
            Amount = amount;
            FlagKey = flagKey;
        }

        public void Apply(GameState state)
        {
            throw new NotImplementedException();
        }

        public bool Validate(out List<string> errors)
        {
            throw new NotImplementedException();
        }

    }
}
