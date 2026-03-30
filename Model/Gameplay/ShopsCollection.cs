using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Gameplay
{
    public class ShopsCollection : ObservableCollection<Shop>
    {
        private int _ownerScenarioId;

        public int OwnerScenarioId
        {
            get => _ownerScenarioId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _ownerScenarioId = value;
                }
            }
        }

        public ShopsCollection(int ownerScenarioId)
        {
            if (ownerScenarioId <= 0)
            {
                throw new ArgumentException("ownerScenarioId doit être un nombre positif.", nameof(ownerScenarioId));
            }

            OwnerScenarioId = ownerScenarioId;
        }

        public ShopsCollection()
        {
        }

        public Shop? GetById(int id)
        {
            Shop? shop = this.FirstOrDefault(s => s != null && s.Id == id);
            return shop;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(s => s != null && s.Id == id);
            return exists;
        }

        public void AddShop(Shop shop)
        {
            if (shop == null)
            {
                throw new ArgumentNullException(nameof(shop));
            }

            if (ContainsId(shop.Id))
            {
                throw new InvalidOperationException($"Une boutique avec l'id {shop.Id} existe déjà dans la collection.");
            }

            if (shop.ScenarioId == 0)
            {
                shop.AssignToScenario(OwnerScenarioId);
            }
            else if (shop.ScenarioId != OwnerScenarioId)
            {
                throw new InvalidOperationException(
                    $"La boutique \"{shop.Name}\" appartient déjà à un autre scénario (ScenarioId={shop.ScenarioId}, attendu={OwnerScenarioId}).");
            }

            Add(shop);
        }

        public bool RemoveById(int id)
        {
            Shop? existingShop = GetById(id);

            if (existingShop == null)
            {
                return false;
            }

            bool removed = Remove(existingShop);

            if (removed)
            {
                existingShop.ClearScenario();
            }

            return removed;
        }
    }
}
