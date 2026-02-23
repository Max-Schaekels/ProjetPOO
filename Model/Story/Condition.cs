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
        private int _id;
        private ConditionType _type;
        private int? _minValue;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public ConditionType Type
        {
            get => _type;
            set => _type = value;
        }


        public int? MinValue
        {
            get => _minValue;
            set
            {
                if (value == null || ValidUtils.CheckIfNonNegativeNumber(value.Value))
                    _minValue = value;
            }
        }

        public Condition(int id, ConditionType type, int? minValue = null)
        {
            Id = id;
            Type = type;
            MinValue = minValue;
        }

        public bool Evaluate(GameState state)
        {
            throw new NotImplementedException();

        }

        public bool Validate(out List<string> errors)
        {
            throw new System.NotImplementedException();
        }

    }
}
