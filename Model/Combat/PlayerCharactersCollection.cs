using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class PlayerCharactersCollection : ObservableCollection<PlayerCharacterTemplate>
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

        public PlayerCharactersCollection(int ownerScenarioId)
        {
            if (ownerScenarioId <= 0)
            {
                throw new ArgumentException("ownerScenarioId doit être un nombre positif.", nameof(ownerScenarioId));
            }

            OwnerScenarioId = ownerScenarioId;
        }

        public PlayerCharactersCollection()
        {
        }

        public PlayerCharacterTemplate? GetById(int id)
        {
            return this.FirstOrDefault(player => player != null && player.Id == id);
        }

        public bool ContainsId(int id)
        {
            return this.Any(player => player != null && player.Id == id);
        }

        public void AddPlayer(PlayerCharacterTemplate playerTemplate)
        {
            if (playerTemplate == null)
            {
                throw new ArgumentNullException(nameof(playerTemplate));
            }

            if (ContainsId(playerTemplate.Id))
            {
                throw new InvalidOperationException($"Un personnage template avec l'id {playerTemplate.Id} existe déjà.");
            }

            if (playerTemplate.ScenarioId == 0)
            {
                playerTemplate.SetScenario(OwnerScenarioId);
            }
            else if (playerTemplate.ScenarioId != OwnerScenarioId)
            {
                throw new InvalidOperationException(
                    $"Le template de personnage \"{playerTemplate.Name}\" appartient déjà à un autre scénario (ScenarioId={playerTemplate.ScenarioId}, attendu={OwnerScenarioId}).");
            }


            Add(playerTemplate);
        }

        public bool RemoveById(int id)
        {
            PlayerCharacterTemplate? template = GetById(id);

            if (template == null)
            {
                return false;
            }

            bool removed = Remove(template);

            if (removed)
            {
                template.ClearScenario();
            }

            return removed;
        }

        public PlayerCharacterTemplate GetDefault()
        {
            PlayerCharacterTemplate? template = this.FirstOrDefault();

            if (template == null)
            {
                throw new InvalidOperationException("Aucun personnage jouable n'est défini dans la collection.");
            }

            return template;
        }
    }
}
