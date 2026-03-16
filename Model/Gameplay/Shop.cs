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

        private static int _nextId = 1;

        private int _id;
        private int _scenarioId;
        private string _name;
        private int _potionPrice;
        private int _keyPrice;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _scenarioId = value;
                }
            }
        }

        public string Name
        {
            get => _name;
            private set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
                    _name = value;
            }
        }

        public int PotionPrice
        {
            get => _potionPrice;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _potionPrice = value;
            }
        }

        public int KeyPrice
        {
            get => _keyPrice;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _keyPrice = value;
            }
        }

        // Constructeur "normal" (en mémoire)
        public Shop(string name, int potionPrice, int keyPrice)
        {
            Id = GenerateId();
            ScenarioId = 0;
            Name = name;
            PotionPrice = potionPrice;
            KeyPrice = keyPrice;
            
        }

        // Constructeur privé pour Load
        private Shop(int id, int scenarioId, string name, int potionPrice, int keyPrice)
        {
            Id = id;
            ScenarioId = scenarioId;
            Name = name;
            PotionPrice = potionPrice;
            KeyPrice = keyPrice;
            
        }

        public void AssignToScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être > 0.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
        }

        public void ClearScenario()
        {
            ScenarioId = 0;
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
        public static Shop Load(int id, int scenarioId, string name, int potionPrice, int keyPrice)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être >= 0.", nameof(scenarioId));
            }

            Shop shop = new Shop(id, scenarioId, name, potionPrice, keyPrice);

            EnsureNextIdIsAfterLoadedId(id);

            return shop;
        }


        public void Rename(string name)
        {
            if (!ValidUtils.CheckEntryName(name, MINIMUM_NAME_LENGTH, MAXIMUM_NAME_LENGTH))
            {
                throw new ArgumentException("Nom de shop invalide.", nameof(name));
            }

            Name = name;
        }

        public void UpdatePotionPrice(int potionPrice)
        {
            if (!ValidUtils.CheckIfPositiveNumber(potionPrice))
            {
                throw new ArgumentException("potionPrice doit être un nombre positif.", nameof(potionPrice));
            }

            PotionPrice = potionPrice;
        }

        public void UpdateKeyPrice(int keyPrice)
        {
            if (!ValidUtils.CheckIfPositiveNumber(keyPrice))
            {
                throw new ArgumentException("keyPrice doit être un nombre positif.", nameof(keyPrice));
            }

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
