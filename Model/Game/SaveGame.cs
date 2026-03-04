using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Game
{
    public class SaveGame
    {
        private const int MINIMUM_NAME_LENGTH = 3;
        private const int MAXIMUM_NAME_LENGTH = 100;

        private static int _nextId = 1;

        private int _id;
        private string _name;
        private DateTime _createdAt;
        private DateTime _lastSavedAt;
        private GameState _state;

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

        public DateTime CreatedAt
        {
            get => _createdAt;
            private set
            {
                _createdAt = value;
            }
        }

        public DateTime LastSavedAt
        {
            get => _lastSavedAt;
            private set
            {
                _lastSavedAt = value;
            }
        }

        public GameState State
        {
            get => _state;
            private set
            {
                if (ValidUtils.CheckIfNotNull(value))
                {
                    _state = value;
                }
            }
        }

        // Création "normale" : Id auto + dates auto
        public SaveGame(string name, GameState state)
        {
            Id = GenerateId();
            Name = name;

            CreatedAt = DateTime.Now;
            LastSavedAt = DateTime.Now;

            State = state;
        }

        // Load (DB plus tard ) : Id fourni + dates fournies + state fourni
        public static SaveGame Load(int id, string name, DateTime createdAt, DateTime lastSavedAt, GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            SaveGame save = new SaveGame(name, state);

            save.Id = id;
            EnsureNextIdIsAfterLoadedId(id);

            save.CreatedAt = createdAt;
            save.LastSavedAt = lastSavedAt;

            return save;
        }

        public void Rename(string name)
        {
            if (!ValidUtils.CheckEntryName(name, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
            {
                throw new ArgumentException($"Name doit être compris entre {MINIMUM_NAME_LENGTH} et {MAXIMUM_NAME_LENGTH} caractères.", nameof(name));
            }

            Name = name;
        }

        public void UpdateFrom(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            State = state;
            LastSavedAt = DateTime.Now;
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (_nextId <= loadedId)
            {
                _nextId = loadedId + 1;
            }
        }
    }
}
