using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Inventory
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
            throw new System.NotImplementedException();
        }

        public bool HasKey()
        {
            throw new System.NotImplementedException();
        }

        public void AddPotion(int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool ConsumePotion()
        {
            throw new System.NotImplementedException();
        }

        public void AddKey(int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool ConsumeKey()
        {
            throw new System.NotImplementedException();
        }
    }
}
