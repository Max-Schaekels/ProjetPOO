using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Gameplay
{
    public class Inventory
    {
        private static int _nextId = 1;

        private int _id;
        private int _potionsCount;
        private int _keysCount;

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
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _potionsCount = value;
            }
        }

        public int KeysCount
        {
            get => _keysCount;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _keysCount = value;
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

        public void AddPotion(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                PotionsCount = PotionsCount + amount;
            }

        }

        public bool ConsumePotion()
        {
            if (HasPotion())
            {
                PotionsCount = PotionsCount - 1;
                return true;
            }
            return false;
        }

        public void AddKey(int amount)
        {
            if (ValidUtils.CheckIfPositiveNumber(amount))
            {
                KeysCount = KeysCount + amount;
            }
        }

        public bool ConsumeKey()
        {
            if (HasKey())
            {
                KeysCount = KeysCount - 1;
                return true;
            }
            return false;
        }
    }
}
