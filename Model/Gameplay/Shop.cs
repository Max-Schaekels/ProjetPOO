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
            if (gameState == null)
            {
                return false;
            }
            return gameState.CanAfford(PotionPrice);
        }

        public bool CanBuyKey(GameState gameState)
        {
            if (gameState == null)
            {
                return false;
            }
            return gameState.CanAfford(KeyPrice);
        }

        public bool BuyPotion(GameState gameState)
        {
            if (gameState == null)
            {
                return false;
            }

            if (gameState.PlayerInventory == null)
            {
                return false;
            }

            if (!CanBuyPotion(gameState))
            {
                return false;
            }

            gameState.SpendGold(PotionPrice);
            gameState.PlayerInventory.AddPotion(1);
            return true;
        }

        public bool BuyKey(GameState gameState)
        {
            if (gameState == null)
            {
                return false;
            }

            if (gameState.PlayerInventory == null)
            {
                return false;
            }

            if (!CanBuyKey(gameState))
            {
                return false;
            }

            gameState.SpendGold(KeyPrice);
            gameState.PlayerInventory.AddKey(1);
            return true;
        }

    }
}
