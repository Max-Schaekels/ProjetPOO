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

        private int _id;
        private string _title;
        private string _text;
        private SceneType _type;
        private int _scenarioId;
        private List<Choice> _choices;
        private int? _shopId;
        private int? _enemyId;
        private int? _fleeTargetSceneId;
        private int? _defeatTargetSceneId;
        private int? _victoryTargetSceneId;

        public Scene(int id, string title, string text, SceneType type, int scenarioId, int? shopId = null, int? enemyId = null, int? fleeTargetSceneId = null, int? defeatTargetSceneId = null, int? victoryTargetSceneId = null)
        {
            Id = id;
            Title = title;
            Text = text;
            Type = type;
            ScenarioId = scenarioId;

            ShopId = shopId;
            EnemyId = enemyId;
            FleeTargetSceneId = fleeTargetSceneId;
            DefeatTargetSceneId = defeatTargetSceneId;
            VictoryTargetSceneId = victoryTargetSceneId;

            _choices = new List<Choice>();
        }

        public Scene()
        {
            _choices = new List<Choice>();
        }

        public int Id
        {
            get => _id;
            set
            {
                if(ValidUtils.CheckIfPositiveNumber(value)) 
                    _id = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
                    _title = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (ValidUtils.CheckEntryDescription(value, MINIMUM_TEXT_LENGTH))
                    _text = value;
            }
        }

        public SceneType Type
        {
            get => _type;
            set
            {
              
                _type = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _scenarioId = value;
            }
        }

        public IReadOnlyList<Choice> Choices => _choices.AsReadOnly();

        public int? ShopId
        {
            get => _shopId;
            set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                    _shopId = value;
            }
        }

        public int? EnemyId
        {
            get => _enemyId;
            set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                    _enemyId = value;
            }
        }

        public int? FleeTargetSceneId
        {
            get => _fleeTargetSceneId;
            set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                    _fleeTargetSceneId = value;
            }
        }

        public int? DefeatTargetSceneId
        {
            get => _defeatTargetSceneId;
            set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                    _defeatTargetSceneId = value;
            }
        }

        public int? VictoryTargetSceneId
        {
            get => _victoryTargetSceneId;
            set
            {
                if (value == null || ValidUtils.CheckIfPositiveNumber(value.Value))
                    _victoryTargetSceneId = value;
            }
        }

        public void AddChoice(Choice choice)
        {
            throw new NotImplementedException();
        }

        public void RemoveChoice(int choiceId)
        {
            throw new NotImplementedException();
        }

        public List<Choice> GetAvailableChoices(GameState state)
        {
            throw new NotImplementedException();
        }

        public bool IsTerminal()
        {
            throw new NotImplementedException();
        }

        public List<string> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
