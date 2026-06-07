using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Gameplay
{
    public class Inventory : INotifyPropertyChanged
    {
        private static int _nextId = 1;

        private int _id;
        private int _potionsCount;
        private int _keysCount;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int PotionsCount
        {
            get => _potionsCount;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value) && _potionsCount != value)
                {
                    _potionsCount = value;
                    OnPropertyChanged(nameof(PotionsCount));
                }
            }
        }

        public int KeysCount
        {
            get => _keysCount;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value) && _keysCount != value)
                {
                    _keysCount = value;
                    OnPropertyChanged(nameof(KeysCount));
                }
            }
        }

        // Constructeur "normal" (en mémoire)
        public Inventory(int potionsCount, int keysCount)
        {
            Id = GenerateId();
            PotionsCount = potionsCount;
            KeysCount = keysCount;
        }

        // Constructeur privé pour Load
        private Inventory(int id, int potionsCount, int keysCount)
        {
            Id = id;
            PotionsCount = potionsCount;
            KeysCount = keysCount;
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

        // Constructeur pour Load (depuis la base de données)
        public static Inventory Load(int id, int potionsCount, int keysCount)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            Inventory inventory = new Inventory(id, potionsCount, keysCount);

            EnsureNextIdIsAfterLoadedId(id);

            return inventory;
        }


        public bool HasPotion()
        {
            return PotionsCount > 0;
        }

        public bool HasKey()
        {
            return KeysCount > 0;
        }

        /// <summary>
        /// Adds one potion to the inventory.
        /// </summary>
        public void AddPotion(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                PotionsCount = PotionsCount + amount;
            }

        }

        /// <summary>
        /// Uses one potion if available.
        /// </summary>
        /// <returns>True if a potion was used, otherwise false.</returns>
        public bool ConsumePotion()
        {
            if (HasPotion())
            {
                PotionsCount = PotionsCount - 1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds one key to the inventory.
        /// </summary>
        public void AddKey(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                KeysCount = KeysCount + amount;
            }
        }

        /// <summary>
        /// Uses one key if available.
        /// </summary>
        /// <returns>True if a key was used, otherwise false.</returns>
        public bool ConsumeKey()
        {
            if (HasKey())
            {
                KeysCount = KeysCount - 1;
                return true;
            }
            return false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
