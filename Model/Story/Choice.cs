using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class Choice
    {
        private const int MINIMUM_LABEL_LENGTH = 3;
        private const int MAX_LABEL_LENGTH = 200;
        private int _id;
        private string _label;
        private int _targetSceneId;
        private int _sceneId;
        private List<Condition> _conditions;
        private List<Effect> _effects;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_LABEL_LENGTH, MAX_LABEL_LENGTH))
                    _label = value;
            }
        }

        public int TargetSceneId
        {
            get => _targetSceneId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _targetSceneId = value;
            }
        }

        public int SceneId
        {
            get => _sceneId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _sceneId = value;
            }
        }

        public IReadOnlyList<Condition> Conditions => _conditions.AsReadOnly();
        public IReadOnlyList<Effect> Effects => _effects.AsReadOnly();


        public Choice(int id, string label, int targetSceneId, int sceneId)
        {
            Id = id;
            Label = label;
            TargetSceneId = targetSceneId;
            SceneId = sceneId;
            _conditions = new List<Condition>();
            _effects = new List<Effect>();
        }

        public Choice()
        {
            _conditions = new List<Condition>();
            _effects = new List<Effect>();
        }

        public bool IsAvailable(GameState state)
        {
            throw new NotImplementedException();
        }

        public void ApplyEffects(GameState state)
        {
            throw new NotImplementedException();
        }

        public bool Validate(out List<string> errors)
        {
            throw new System.NotImplementedException();
        }

        public void AddCondition(Condition condition)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCondition(int conditionId)
        {
            throw new System.NotImplementedException();
        }

        public void AddEffect(Effect effect)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(int effectId)
        {
            throw new System.NotImplementedException();
        }   

    }
}
