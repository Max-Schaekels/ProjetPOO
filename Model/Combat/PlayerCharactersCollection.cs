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

            Add(playerTemplate);
        }

        public bool RemoveById(int id)
        {
            PlayerCharacterTemplate? template = GetById(id);

            if (template == null)
            {
                return false;
            }

            return Remove(template);
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
