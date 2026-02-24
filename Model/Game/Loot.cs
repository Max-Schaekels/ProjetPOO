using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Game
{
    public class Loot
    {
        
        private int _goldAmount;
        private int _potionAmount;
        private int _keysAmount;

        

        public int GoldAmount
        {
            get => _goldAmount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _goldAmount = value;
            }
        }

        public int PotionAmount
        {
            get => _potionAmount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _potionAmount = value;
            }
        }

        public int KeysAmount
        {
            get => _keysAmount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _keysAmount = value;
            }
        }

        public Loot( int goldAmount, int potionAmount, int keysAmount)
        {
            GoldAmount = goldAmount;
            PotionAmount = potionAmount;
            KeysAmount = keysAmount;
        }

        public void Apply(GameState state)
        {
            throw new System.NotImplementedException();
        }
    }
}
