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
        private int _id;
        private int _potionsCount;
        private int _keysCount;

        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int PotionsCount
        {
            get => _potionsCount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _potionsCount = value;
            }
        }

        public int KeysCount
        {
            get => _keysCount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _keysCount = value;
            }
        }

        public Inventory(int id, int potionsCount, int keysCount)
        {
            Id = id;
            PotionsCount = potionsCount;
            KeysCount = keysCount;
        }

        public Inventory(int potionsCount, int keysCount)
        {
            PotionsCount = potionsCount;
            KeysCount = keysCount;
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
