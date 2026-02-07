using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Inventory
{
    public class Inventory
    {
        private int _id;
        private int _potionsCount;
        private int _keysCount;

        public Inventory(int id, int potionCount, int keysCount)
        {
            _id = id;
            _potionsCount = potionCount;
            _keysCount = keysCount;
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
