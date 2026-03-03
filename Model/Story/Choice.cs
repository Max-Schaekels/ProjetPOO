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

        private static int _nextId = 1;

        private int _id;
        private string _label;
        private int _targetSceneId;
        private int _sceneId;
        private List<Condition> _conditions;
        private List<Effect> _effects;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _id = value;
                }
                    
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_LABEL_LENGTH, MAX_LABEL_LENGTH))
                {
                    _label = value;
                }
                   
            }
        }

        public int TargetSceneId
        {
            get => _targetSceneId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _targetSceneId = value;
                }
                   
            }
        }

        public int SceneId
        {
            get => _sceneId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _sceneId = value;
                }
                   
            }
        }

        public IReadOnlyList<Condition> Conditions => _conditions.AsReadOnly();
        public IReadOnlyList<Effect> Effects => _effects.AsReadOnly();


        public Choice(string label)
        {
            _conditions = new List<Condition>();
            _effects = new List<Effect>();

            Id = GenerateId();
            Label = label;

            SceneId = 0;
            TargetSceneId = 0;
        }

        public Choice(string label, int targetSceneId, int sceneId)
        {
            _conditions = new List<Condition>();
            _effects = new List<Effect>();

            Id = GenerateId();
            Label = label;

            TargetSceneId = targetSceneId;
            SceneId = sceneId;
        }

        public Choice()
        {
            _conditions = new List<Condition>();
            _effects = new List<Effect>();

            _label = string.Empty;

            Id = GenerateId();
            SceneId = 0;
            TargetSceneId = 0;
        }

        public void AssignToScene(int sceneId)
        {
            if (!ValidUtils.CheckIfNonNegativeNumber(sceneId))
            {
                throw new ArgumentException("sceneId doit être >= 0.", nameof(sceneId));
            }

            SceneId = sceneId;
        }

        public void ClearScene()
        {
            SceneId = 0;
        }

        public void SetTargetScene(int targetSceneId)
        {
            if (!ValidUtils.CheckIfNonNegativeNumber(targetSceneId))
            {
                throw new ArgumentException("targetSceneId doit être >= 0.", nameof(targetSceneId));
            }

            TargetSceneId = targetSceneId;
        }

        public void ClearTargetScene()
        {
            TargetSceneId = 0;
        }

        public bool IsAvailable(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (_conditions.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (Condition condition in _conditions)
                {
                    if (condition == null)
                    {
                        return false;
                    }
                    if (!condition.Evaluate(state))
                    {
                        return false;

                    }
                }
                return true;
            }
        }

        public void ApplyEffects(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            foreach (Effect effect in _effects)
            {
                if (effect != null)
                {
                    effect.Apply(state);
                }
            }
        }

        public bool ValidateSafe(out List<string> errors)
        {
            errors = new List<string>();

            if (!ValidUtils.CheckEntryName(Label, MINIMUM_LABEL_LENGTH, MAX_LABEL_LENGTH))
            {
                errors.Add($"Le label doit être compris entre {MINIMUM_LABEL_LENGTH} et {MAX_LABEL_LENGTH} caractères.");
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(SceneId))
            {
                errors.Add("Le SceneId doit être un nombre >= 0.");
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(TargetSceneId))
            {
                errors.Add("Le TargetSceneId doit être un nombre >= 0.");
            }

            if (SceneId > 0 && TargetSceneId > 0 && SceneId == TargetSceneId)
            {
                errors.Add("Le TargetSceneId doit être différent du SceneId.");
            }

            for (int i = 0; i < _conditions.Count; i++)
            {
                Condition condition = _conditions[i];

                if (condition == null)
                {
                    errors.Add($"Choice \"{Label}\" : une condition est null (liste corrompue).");
                    continue;
                }

                List<string> conditionErrors;
                bool conditionOk = condition.Validate(out conditionErrors);

                if (!conditionOk)
                {
                    string conditionInfo = GetConditionDebugLabel(condition);

                    foreach (string conditionError in conditionErrors)
                    {
                        errors.Add($"Choice \"{Label}\" - Condition {conditionInfo} : {conditionError}");
                    }
                }
            }

            for (int i = 0; i < _effects.Count; i++)
            {
                Effect effect = _effects[i];

                if (effect == null)
                {
                    errors.Add($"Choice \"{Label}\" : un effet est null (liste corrompue).");
                    continue;
                }

                List<string> effectErrors;
                bool effectOk = effect.Validate(out effectErrors);

                if (!effectOk)
                {
                    string effectInfo = GetEffectDebugLabel(effect);

                    foreach (string effectError in effectErrors)
                    {
                        errors.Add($"Choice \"{Label}\" - Effect {effectInfo} : {effectError}");
                    }
                }
            }

            return errors.Count == 0;
        }

        public bool ValidatePlayable(out List<string> errors)
        {
            bool baseOk = ValidateSafe(out errors);

            if (!ValidUtils.CheckIfPositiveNumber(SceneId))
            {
                errors.Add("Pour jouer, SceneId doit être > 0.");
            }

            if (!ValidUtils.CheckIfPositiveNumber(TargetSceneId))
            {
                errors.Add("Pour jouer, TargetSceneId doit être > 0.");
            }

            if (ValidUtils.CheckIfPositiveNumber(SceneId) &&
                ValidUtils.CheckIfPositiveNumber(TargetSceneId) &&
                SceneId == TargetSceneId)
            {
                errors.Add("Pour jouer, TargetSceneId doit être différent du SceneId.");
            }

            return baseOk && errors.Count == 0;
        }

        public void AddCondition(Condition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            _conditions.Add(condition);
        }

        public void RemoveCondition(int conditionId)
        {
            _conditions.RemoveAll(c => c != null && c.Id == conditionId);
        }

        public void AddEffect(Effect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }
            _effects.Add(effect);
        }

        public void RemoveEffect(int effectId)
        {
            _effects.RemoveAll(e => e != null && e.Id == effectId);
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

        private string GetConditionDebugLabel(Condition condition)
        {
            
            return $"{condition.Type} (MinValue={condition.MinValue})";
        }

        private string GetEffectDebugLabel(Effect effect)
        {
            
            if (effect.FlagKey != null)
            {
                return $"{effect.Type} (FlagKey={effect.FlagKey})";
            }

            if (effect.Amount.HasValue)
            {
                return $"{effect.Type} (Amount={effect.Amount.Value})";
            }

            return $"{effect.Type} (no data)";
        }

    }
}
