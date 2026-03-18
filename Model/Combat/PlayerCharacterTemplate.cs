using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class PlayerCharacterTemplate
    {
        private const int MINIMUM_NAME_LENGTH = 3;
        private const int MAXIMUM_NAME_LENGTH = 50;

        private static int _nextId = 1;

        private int _id;
        private int _scenarioId;
        private string _name;
        private int _maxHp;
        private int _attack;
        private int _defense;
        private int _agility;
        private int _startingExperience;
        private int _startingLevel;

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

        public int MaxHp
        {
            get => _maxHp;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _maxHp = value;
                }
            }
        }

        public int Attack
        {
            get => _attack;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _attack = value;
                }
            }
        }

        public int Defense
        {
            get => _defense;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _defense = value;
                }
            }
        }

        public int Agility
        {
            get => _agility;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _agility = value;
                }
            }
        }

        public int StartingExperience
        {
            get => _startingExperience;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _startingExperience = value;
                }
            }
        }

        public int StartingLevel
        {
            get => _startingLevel;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _startingLevel = value;
                }
            }
        }

        public PlayerCharacterTemplate( string name, int maxHp, int attack,  int defense, int agility, int startingExperience = 0, int startingLevel = 1)
        {
            Id = GenerateId();
            Name = name;
            MaxHp = maxHp;
            Attack = attack;
            Defense = defense;
            Agility = agility;
            StartingExperience = startingExperience;
            StartingLevel = startingLevel;
        }

        private PlayerCharacterTemplate(int id, int scenarioId, string name, int maxHp, int attack, int defense, int agility, int startingExperience,int startingLevel)
        {
            Id = id;
            ScenarioId = scenarioId;
            Name = name;
            MaxHp = maxHp;
            Attack = attack;
            Defense = defense;
            Agility = agility;
            StartingExperience = startingExperience;
            StartingLevel = startingLevel;
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

        public static PlayerCharacterTemplate Load( int id,int scenarioId,string name, int maxHp, int attack, int defense, int agility, int startingExperience, int startingLevel)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être un nombre non négatif.", nameof(scenarioId));
            }

            PlayerCharacterTemplate template = new PlayerCharacterTemplate( id,scenarioId,name,maxHp, attack,defense,agility,startingExperience,startingLevel);

            EnsureNextIdIsAfterLoadedId(id);

            return template;
        }

        public PlayerCharacterInstance CreateInstance()
        {
            return new PlayerCharacterInstance( Id, Name, MaxHp, Attack, Defense, Agility, StartingExperience, StartingLevel);
        }

        public void Rename(string name)
        {
            if (!ValidUtils.CheckEntryName(name, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
            {
                throw new ArgumentException(
                    $"Name doit être compris entre {MINIMUM_NAME_LENGTH} et {MAXIMUM_NAME_LENGTH} caractères.",
                    nameof(name));
            }

            Name = name;
        }

        public void SetScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(scenarioId))
            {
                throw new ArgumentException("Le scénario doit être valide.", nameof(scenarioId));
            }

            if (ScenarioId != 0 && ScenarioId != scenarioId)
            {
                throw new InvalidOperationException(
                    $"Le template appartient déjà à un autre scénario (ScenarioId={ScenarioId}, nouveau={scenarioId}).");
            }

            ScenarioId = scenarioId;
        }

        public void ClearScenario()
        {
            ScenarioId = 0;
        }
    }
}

