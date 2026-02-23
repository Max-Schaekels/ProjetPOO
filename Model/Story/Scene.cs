using ProjetPOO.Model.Game;
using ProjetPOO.Model.Story.Enums;

namespace ProjetPOO.Model.Story
{
    public class Scene
    {
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
                // TODO validations plus tard (ValidUtils.CheckIfPositiveNumber(value))
                _id = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                // TODO validations plus tard (nom non vide, longueur min, etc.)
                _title = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                // TODO validations plus tard (non vide, longueur min, etc.)
                _text = value;
            }
        }

        public SceneType Type
        {
            get => _type;
            set
            {
                // TODO validations plus tard (cohérence avec ShopId / EnemyId etc.)
                _type = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            set
            {
                // TODO validations plus tard (positif)
                _scenarioId = value;
            }
        }

        public IReadOnlyList<Choice> Choices => _choices.AsReadOnly();

        public int? ShopId
        {
            get => _shopId;
            set
            {
                // TODO validations plus tard (si Type == Shop → non null)
                _shopId = value;
            }
        }

        public int? EnemyId
        {
            get => _enemyId;
            set
            {
                // TODO validations plus tard (si Type == Combat → non null)
                _enemyId = value;
            }
        }

        public int? FleeTargetSceneId
        {
            get => _fleeTargetSceneId;
            set
            {
                // TODO validations plus tard
                _fleeTargetSceneId = value;
            }
        }

        public int? DefeatTargetSceneId
        {
            get => _defeatTargetSceneId;
            set
            {
                // TODO validations plus tard
                _defeatTargetSceneId = value;
            }
        }

        public int? VictoryTargetSceneId
        {
            get => _victoryTargetSceneId;
            set
            {
                // TODO validations plus tard
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
