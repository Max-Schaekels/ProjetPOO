using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Gameplay
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

        public Loot(int goldAmount, int potionAmount, int keysAmount)
        {
            GoldAmount = goldAmount;
            PotionAmount = potionAmount;
            KeysAmount = keysAmount;
        }

        public void Apply(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (state.PlayerInventory == null)
            {
                throw new InvalidOperationException("L'inventaire du joueur n'est pas initialisé.");
            }

            state.AddGold(GoldAmount);    

            if (ValidUtils.CheckIfPositiveNumber(PotionAmount))
            {
                state.PlayerInventory.AddPotion(PotionAmount);
            }
            if (ValidUtils.CheckIfPositiveNumber(KeysAmount))
            {
                state.PlayerInventory.AddKey(KeysAmount);
            }

        }

        public string GetDescription()
        {
            List<string> parts = new List<string>();
            if (ValidUtils.CheckIfPositiveNumber(GoldAmount))
                parts.Add($"{GoldAmount} or");
            if (ValidUtils.CheckIfPositiveNumber(PotionAmount))
                parts.Add($"{PotionAmount} potion{(PotionAmount > 1 ? "s" : "")}");
            if (ValidUtils.CheckIfPositiveNumber(KeysAmount))
                parts.Add($"{KeysAmount} clé{(KeysAmount > 1 ? "s" : "")}");
            if (parts.Count == 0)
                return "Aucune récompense";
            return string.Join(", ", parts);
        }

        public void Clear()
        {
            GoldAmount = 0;
            PotionAmount = 0;
            KeysAmount = 0;
        }

    }
}
