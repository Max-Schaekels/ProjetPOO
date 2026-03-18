using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Story
{
    public class ConditionsCollection : ObservableCollection<Condition>
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

        public ConditionsCollection(int ownerChoiceId)
        {
            if (ownerChoiceId <= 0)
            {
                throw new ArgumentException("ownerChoiceId doit être un nombre positif.", nameof(ownerChoiceId));
            }

            OwnerChoiceId = ownerChoiceId;
        }

        public Condition? GetById(int id)
        {
            Condition? condition = this.FirstOrDefault(c => c != null && c.Id == id);
            return condition;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(c => c != null && c.Id == id);
            return exists;
        }

        public void AddCondition(Condition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (ContainsId(condition.Id))
            {
                throw new InvalidOperationException($"Une condition avec l'id {condition.Id} existe déjà dans la collection.");
            }

            if (condition.ChoiceId == 0)
            {
                condition.SetChoice(OwnerChoiceId);
            }
            else if (condition.ChoiceId != OwnerChoiceId)
            {
                throw new InvalidOperationException(
                    $"La condition appartient déjà à un autre choix (ChoiceId={condition.ChoiceId}, attendu={OwnerChoiceId}).");
            }

            Add(condition);
        }

        public bool RemoveById(int id)
        {
            Condition? existingCondition = GetById(id);

            if (existingCondition == null)
            {
                return false;
            }

            bool removed = Remove(existingCondition);

            if (removed)
            {
                existingCondition.ClearChoice();
            }

            return removed;
        }
    }
}
