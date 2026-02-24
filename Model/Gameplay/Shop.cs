using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Gameplay
{
    public class Shop
    {
        private const int MINIMUM_NAME_LENGTH = 3;
        private const int MAXIMUM_NAME_LENGTH = 50;
        private int _id;
        private string _name;
        private int _potionPrice;
        private int _keyPrice;

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

        public int PotionPrice
        {
            get => _potionPrice;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _potionPrice = value;
            }
        }

        public int KeyPrice
        {
            get => _keyPrice;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _keyPrice = value;
            }
        }

        public Shop(int id, string name, int potionPrice, int keyPrice)
        {
            Id = id;
            Name = name;
            PotionPrice = potionPrice;
            KeyPrice = keyPrice;
        }

        public Shop(string name, int potionPrice, int keyPrice)
        {
            Name = name;
            PotionPrice = potionPrice;
            KeyPrice = keyPrice;
        }

        public bool CanBuyPotion(GameState gameState)
        {
            return gameState.CanAfford(PotionPrice);
        }

        public bool CanBuyKey(GameState gameState)
        {
            return gameState.CanAfford(KeyPrice);
        }

        public bool BuyPotion(GameState gameState)
        {
            throw new NotImplementedException();
        }

        public bool BuyKey(GameState gameState)
        {
            throw new NotImplementedException();
        }

    }
}
