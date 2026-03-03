using ProjetPOO.Model.Game;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class Scene
    {
        private const int MINIMUM_TITLE_LENGTH = 3;
        private const int MAX_TITLE_LENGTH = 200;
        public static readonly string[] ALLOWED_PICTURE_FILE_FORMATS = { "jpg", "png" };
        private const int MINIMUM_TEXT_LENGTH = 10;

        private static int _nextId = 1;

        private int _id;
        private string _title;
        private string _text;
        private SceneType _type;
        private int _scenarioId;
        private List<Choice> _choices;

        private string? _pictureFileName;

        private int? _shopId;
        private int? _enemyId;
        private int? _fleeTargetSceneId;
        private int? _defeatTargetSceneId;
        private int? _victoryTargetSceneId;

        public Scene(string title, string text, SceneType type)
        {
            _choices = new List<Choice>();

            Id = GenerateId();
            Title = title;
            Text = text;
            Type = type;

            ScenarioId = 0;

            PictureFileName = null;
            ShopId = null;
            EnemyId = null;
            FleeTargetSceneId = null;
            DefeatTargetSceneId = null;
            VictoryTargetSceneId = null;
        }

        public Scene()
        {
            _choices = new List<Choice>();

            _title = string.Empty;
            _text = string.Empty;

            Id = GenerateId();
            Type = SceneType.Normal;

            ScenarioId = 0;

            PictureFileName = null;
            ShopId = null;
            EnemyId = null;
            FleeTargetSceneId = null;
            DefeatTargetSceneId = null;
            VictoryTargetSceneId = null;
        }


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

        public string Title
        {
            get => _title;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
                {
                    _title = value;
                }
                    
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (ValidUtils.CheckEntryDescription(value, MINIMUM_TEXT_LENGTH))
                {
                    _text = value;
                }
                    
            }
        }

        public SceneType Type
        {
            get => _type;
            private set
            {
              
                _type = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _scenarioId = value;
                }
                   
            }
        }

        public string? PictureFileName
        {
            get => _pictureFileName;
            private set
            {
                if (value == null)
                {
                    _pictureFileName = null;
                    return;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                if (ValidUtils.CheckFileFormat(value, ALLOWED_PICTURE_FILE_FORMATS))
                {
                    _pictureFileName = value;
                }
            }
        }

        public IReadOnlyList<Choice> Choices => _choices.AsReadOnly();

        public int? ShopId
        {
            get => _shopId;
            private set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                {
                    _shopId = value;
                }                   
            }
        }

        public int? EnemyId
        {
            get => _enemyId;
            private set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                {
                    _enemyId = value;
                }
            }
        }

        public int? FleeTargetSceneId
        {
            get => _fleeTargetSceneId;
            private set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                {
                    _fleeTargetSceneId = value;
                }
            }
        }

        public int? DefeatTargetSceneId
        {
            get => _defeatTargetSceneId;
            private set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                {
                    _defeatTargetSceneId = value;
                }
            }
        }

        public int? VictoryTargetSceneId
        {
            get => _victoryTargetSceneId;
            private set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                {
                    _victoryTargetSceneId = value;
                }
            }
        }

        public void AssignToScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfNonNegativeNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être >= 0.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
        }

        public void ClearScenario()
        {
            ScenarioId = 0;
        }

        public void ChangeType(SceneType newType)
        {
            if (!Enum.IsDefined(typeof(SceneType), newType))
            {
                throw new ArgumentException("Type de scène invalide.", nameof(newType));
            }

            Type = newType;
        }

        public void SetPicture(string pictureFileName)
        {
            if (string.IsNullOrWhiteSpace(pictureFileName))
            {
                throw new ArgumentException("pictureFileName ne peut pas être vide.", nameof(pictureFileName));
            }

            if (!ValidUtils.CheckFileFormat(pictureFileName, ALLOWED_PICTURE_FILE_FORMATS))
            {
                throw new ArgumentException("Format d'image non autorisé (jpg/png).", nameof(pictureFileName));
            }

            PictureFileName = pictureFileName;
        }

        public void ClearPicture()
        {
            PictureFileName = null;
        }

        public void SetShop(int? shopId)
        {
            ShopId = shopId;
        }

        public void SetCombat(int? enemyId, int? fleeTargetSceneId, int? defeatTargetSceneId, int? victoryTargetSceneId)
        {
            EnemyId = enemyId;
            FleeTargetSceneId = fleeTargetSceneId;
            DefeatTargetSceneId = defeatTargetSceneId;
            VictoryTargetSceneId = victoryTargetSceneId;
        }

        public void ClearCombat()
        {
            EnemyId = null;
            FleeTargetSceneId = null;
            DefeatTargetSceneId = null;
            VictoryTargetSceneId = null;
        }

        public void AddChoice(Choice choice)
        {
            if (choice == null)
            {
                throw new ArgumentNullException(nameof(choice));
            }

            bool alreadyExists = _choices.Any(c => c != null && c.Id == choice.Id);
            if (alreadyExists)
            {
                return;
            }

            _choices.Add(choice);

            choice.AssignToScene(Id);
        }

        public void RemoveChoice(int choiceId)
        {
            Choice? toRemove = _choices.FirstOrDefault(c => c != null && c.Id == choiceId);
            if (toRemove == null)
            {
                return;
            }

            _choices.Remove(toRemove);

            toRemove.ClearScene();
        }

        public List<Choice> GetAvailableChoices(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            List<Choice> available = new List<Choice>();

            if (Type == SceneType.End)
            {
                return available;
            }

            for (int i = 0; i < _choices.Count; i++)
            {
                Choice choice = _choices[i];

                if (choice == null)
                {
                    continue;
                }

                if (choice.IsAvailable(state))
                {
                    available.Add(choice);
                }
            }

            return available;
        }

        public bool IsTerminal()
        {
            return Type == SceneType.End;
        }

        // Validation de la scène pour la création/édition (draft)
        public bool ValidateSafe(out List<string> errors)
        {
            errors = new List<string>();

            if (!ValidUtils.CheckEntryName(Title, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
            {
                errors.Add($"Title doit être compris entre {MINIMUM_TITLE_LENGTH} et {MAX_TITLE_LENGTH} caractères.");
            }

            if (!ValidUtils.CheckEntryDescription(Text, MINIMUM_TEXT_LENGTH))
            {
                errors.Add($"Text doit contenir au moins {MINIMUM_TEXT_LENGTH} caractères.");
            }

            if (!Enum.IsDefined(typeof(SceneType), Type))
            {
                errors.Add("Type de scène invalide.");
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(ScenarioId))
            {
                errors.Add("ScenarioId doit être un nombre >= 0.");
            }

            if (PictureFileName != null && !ValidUtils.CheckFileFormat(PictureFileName, ALLOWED_PICTURE_FILE_FORMATS))
            {
                errors.Add($"Scene \"{Title}\" : format d'image non autorisé (jpg/png).");
            }

            // Validation des choix (safe)
            for (int i = 0; i < _choices.Count; i++)
            {
                Choice choice = _choices[i];

                if (choice == null)
                {
                    errors.Add($"Scene \"{Title}\" : un choix est null (liste corrompue).");
                    continue;
                }

                List<string> choiceErrors;
                bool ok = choice.ValidateSafe(out choiceErrors);

                if (!ok)
                {
                    foreach (string choiceError in choiceErrors)
                    {
                        errors.Add($"Scene \"{Title}\" - {choiceError}");
                    }
                }

                // En draft : si SceneId est défini, il doit correspondre à cette scène
                if (choice.SceneId > 0 && choice.SceneId != Id)
                {
                    errors.Add($"Scene \"{Title}\" : le choix \"{choice.Label}\" appartient à une autre scène (SceneId={choice.SceneId}).");
                }
            }

            return errors.Count == 0;
        }

        // Validation de la scène pour le jeu
        public bool ValidatePlayable(out List<string> errors)
        {
            bool baseOk = ValidateSafe(out errors);

            if (!ValidUtils.CheckIfPositiveNumber(ScenarioId))
            {
                errors.Add("Pour jouer, ScenarioId doit être > 0.");
            }

            // Règles strictes par type
            if (Type == SceneType.Normal)
            {
                if (_choices.Count == 0)
                {
                    errors.Add($"Scene \"{Title}\" : une scène Normal doit contenir au moins un choix.");
                }
            }
            else if (Type == SceneType.Shop)
            {
                if (ShopId == null)
                {
                    errors.Add($"Scene \"{Title}\" : ShopId est requis pour une scène Shop.");
                }
            }
            else if (Type == SceneType.Combat)
            {
                if (EnemyId == null)
                {
                    errors.Add($"Scene \"{Title}\" : EnemyId est requis pour une scène Combat.");
                }

                if (VictoryTargetSceneId == null)
                {
                    errors.Add($"Scene \"{Title}\" : VictoryTargetSceneId est requis pour une scène Combat.");
                }

                if (DefeatTargetSceneId == null)
                {
                    errors.Add($"Scene \"{Title}\" : DefeatTargetSceneId est requis pour une scène Combat.");
                }

                if (FleeTargetSceneId == null)
                {
                    errors.Add($"Scene \"{Title}\" : FleeTargetSceneId est requis pour une scène Combat.");
                }
            }
            else if (Type == SceneType.End)
            {
                if (_choices.Count > 0)
                {
                    errors.Add($"Scene \"{Title}\" : une scène End ne doit pas contenir de choix.");
                }
            }

            // En jouable, chaque choix doit être jouable et bien attaché à cette scène
            for (int i = 0; i < _choices.Count; i++)
            {
                Choice choice = _choices[i];
                if (choice == null)
                {
                    continue;
                }

                List<string> choiceErrors;
                bool ok = choice.ValidatePlayable(out choiceErrors);
                if (!ok)
                {
                    foreach (string choiceError in choiceErrors)
                    {
                        errors.Add($"Scene \"{Title}\" - {choiceError}");
                    }
                }

                if (choice.SceneId != Id)
                {
                    errors.Add($"Scene \"{Title}\" : le choix \"{choice.Label}\" doit avoir SceneId={Id} (actuel={choice.SceneId}).");
                }
            }

            return baseOk && errors.Count == 0;
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

    }
}
