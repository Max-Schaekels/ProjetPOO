using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Game
{
    public class Loot
    {
        private int _id;
        private int _goldAmount;
        private int _potionAmount;
        private int _keysAmount;

        public Loot(int id, int goldAmount, int potionAmount, int keysAmount)
        {
            _id = id;
            _goldAmount = goldAmount;
            _potionAmount = potionAmount;
            _keysAmount = keysAmount;
        }

        public void Apply(GameState state)
        {
            throw new System.NotImplementedException();
        }
    }
}
