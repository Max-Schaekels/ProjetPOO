using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Story
{
    public class EffectsCollection : ObservableCollection<Effect>
    {
        private int _ownerChoiceId;

        public int OwnerChoiceId
        {
            get => _ownerChoiceId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _ownerChoiceId = value;
                }
            }
        }

        public EffectsCollection(int ownerChoiceId)
        {
            if (ownerChoiceId <= 0)
            {
                throw new ArgumentException("ownerChoiceId doit être un nombre positif.", nameof(ownerChoiceId));
            }

            OwnerChoiceId = ownerChoiceId;
        }

        public Effect? GetById(int id)
        {
            Effect? effect = this.FirstOrDefault(e => e != null && e.Id == id);
            return effect;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(e => e != null && e.Id == id);
            return exists;
        }

        public void AddEffect(Effect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException(nameof(effect));
            }

            if (ContainsId(effect.Id))
            {
                throw new InvalidOperationException($"Un effet avec l'id {effect.Id} existe déjà dans la collection.");
            }

            if (effect.ChoiceId == 0)
            {
                effect.SetChoice(OwnerChoiceId);
            }
            else if (effect.ChoiceId != OwnerChoiceId)
            {
                throw new InvalidOperationException(
                    $"L'effet appartient déjà à un autre choix (ChoiceId={effect.ChoiceId}, attendu={OwnerChoiceId}).");
            }

            Add(effect);
        }

        public bool RemoveById(int id)
        {
            Effect? existingEffect = GetById(id);

            if (existingEffect == null)
            {
                return false;
            }

            bool removed = Remove(existingEffect);

            if (removed)
            {
                existingEffect.ClearChoice();
            }

            return removed;
        }
    }
}
