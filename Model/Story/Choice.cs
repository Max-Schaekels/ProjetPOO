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
        private ConditionsCollection _conditions;
        private EffectsCollection _effects;

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
            private set
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

        public ConditionsCollection Conditions
        {
            get => _conditions;
            private set
            {
                if(value != null)
                {
                    _conditions = value;
                }
            }
        }
        public EffectsCollection Effects
        {
            get => _effects;
            private set
            {
                if(value != null)
                {
                    _effects = value;
                }
            }
        }


        public Choice(string label)
        {
            

            Id = GenerateId();
            Conditions = new ConditionsCollection(Id);
            Effects = new EffectsCollection(Id);
            Label = label;

            SceneId = 0;
            TargetSceneId = 0;
        }

        public Choice(string label, int targetSceneId, int sceneId)
        {


            Id = GenerateId();
            Conditions = new ConditionsCollection(Id);
            Effects = new EffectsCollection(Id);
            Label = label;

            TargetSceneId = targetSceneId;
            SceneId = sceneId;
        }

        public Choice()
        {

            _label = string.Empty;

            Id = GenerateId();
            Conditions = new ConditionsCollection(Id);
            Effects = new EffectsCollection(Id);
            SceneId = 0;
            TargetSceneId = 0;
        }

        // Constructeur privé pour Load
        private Choice(int id)
        {

            _label = string.Empty;

            Id = id;
            Conditions = new ConditionsCollection(Id);
            Effects = new EffectsCollection(Id);
            SceneId = 0;
            TargetSceneId = 0;
        }

        // Constructeur pour Load (depuis la base de données) avec vérification des données chargées
        public static Choice Load( int id, string label, int targetSceneId, int sceneId,ConditionsCollection? conditions = null, EffectsCollection? effects = null)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            Choice choice = new Choice(id);

            EnsureNextIdIsAfterLoadedId(id);

            choice.Label = label;
            choice.TargetSceneId = targetSceneId;
            choice.SceneId = sceneId;

            if (conditions != null)
            {
                choice.Conditions = new ConditionsCollection(choice.Id);

                for (int i = 0; i < conditions.Count; i++)
                {
                    Condition condition = conditions[i];
                    if (condition == null)
                    {
                        continue;
                    }

                    if (choice.Conditions.ContainsId(condition.Id))
                    {
                        continue;
                    }

                    choice.Conditions.AddCondition(condition);
                }
            }

            if (effects != null)
            {
                choice.Effects = new EffectsCollection(choice.Id);

                for (int i = 0; i < effects.Count; i++)
                {
                    Effect effect = effects[i];
                    if (effect == null)
                    {
                        continue;
                    }

                    if (choice.Effects.ContainsId(effect.Id))
                    {
                        continue;
                    }

                    choice.Effects.AddEffect(effect);
                }
            }

            return choice;
        }

        public void Rename(string label)
        {
            if (!ValidUtils.CheckEntryName(label, MINIMUM_LABEL_LENGTH, MAX_LABEL_LENGTH))
            {
                throw new ArgumentException($"Le label doit être compris entre {MINIMUM_LABEL_LENGTH} et {MAX_LABEL_LENGTH} caractères.", nameof(label));
            }

            Label = label;
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

            if (Conditions.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (Condition condition in Conditions)
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

            foreach (Effect effect in Effects)
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

            for (int i = 0; i < Conditions.Count; i++)
            {
                Condition condition = Conditions[i];

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

            for (int i = 0; i < Effects.Count; i++)
            {
                Effect effect = Effects[i];

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



        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (loadedId >= _nextId)
            {
                _nextId = loadedId + 1;
            }
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
