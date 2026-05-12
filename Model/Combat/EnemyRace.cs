using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class EnemyRace
    {
        private const int MINIMUM_NAME_LENGTH = 3;
        private const int MAXIMUM_NAME_LENGTH = 50;
        private const int MINIMUM_DESCRIPTION_LENGTH = 0;

        private static int _nextId = 1;

        private int _id;
        private int _scenarioId;
        private string _name;
        private string _description;

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

        public string Name
        {
            get => _name;
            private set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
                {
                    _name = value;
                }
            }
        }

        public string Description
        {
            get => _description;
            private set
            {
                if (value == null)
                {
                    _description = string.Empty;
                }
                else
                {
                    _description = value;
                }
            }
        }

        public EnemyRace(string name, string description)
        {
            Id = GenerateId();
            ScenarioId = 0;
            Name = name;
            Description = description;
        }

        private EnemyRace(int id, int scenarioId, string name, string description)
        {
            Id = id;
            ScenarioId = scenarioId;
            Name = name;
            Description = description;
        }

        public static EnemyRace Load(int id, int scenarioId, string name, string description)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être un nombre non négatif.", nameof(scenarioId));
            }

            EnemyRace enemyRace = new EnemyRace(id, scenarioId, name, description);

            EnsureNextIdIsAfterLoadedId(id);

            return enemyRace;
        }

        public void AssignToScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être un nombre positif.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
        }

        public void ClearScenario()
        {
            ScenarioId = 0;
        }

        public void Rename(string newName)
        {
            Name = newName;
        }

        public void ChangeDescription(string newDescription)
        {
            Description = newDescription;
        }

        private static int GenerateId()
        {
            int id = _nextId;
            _nextId++;
            return id;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (loadedId >= _nextId)
            {
                _nextId = loadedId + 1;
            }
        }
    }
}
