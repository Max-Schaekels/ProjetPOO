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
        private int _id;
        private string _name;
        private DateTime _createdAt;
        private DateTime _lastSavedAt;
        private GameState _state;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
                    _name = value;
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public DateTime LastSavedAt
        {
            get => _lastSavedAt;
            set => _lastSavedAt = value;
        }



        public GameState State
        {
            get => _state;
            set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _state = value;
            }
        }

        public SaveGame(int id, string name, DateTime createdAt, DateTime lastSavedAt, GameState state)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
            LastSavedAt = lastSavedAt;
            State = state;
        }

        public SaveGame(string name, DateTime createdAt, DateTime lastSavedAt, GameState state)
        {
            Name = name;
            CreatedAt = createdAt;
            LastSavedAt = lastSavedAt;
            State = state;
        }

        public void UpdateFrom(GameState state)
        {
            throw new NotImplementedException();
        }


    }
}
