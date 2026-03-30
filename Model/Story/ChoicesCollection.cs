using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class ChoicesCollection : ObservableCollection<Choice>
    {
        private int _ownerSceneId;

        public int OwnerSceneId
        {
            get => _ownerSceneId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _ownerSceneId = value;
                }
            }
        }

        public ChoicesCollection(int ownerSceneId)
        {
            if (ownerSceneId <= 0)
            {
                throw new ArgumentException("ownerSceneId doit être un nombre positif.", nameof(ownerSceneId));
            }

            OwnerSceneId = ownerSceneId;
        }

        public ChoicesCollection()
        {
        }

        public Choice? GetById(int id)
        {
            Choice? choice = this.FirstOrDefault(c => c != null && c.Id == id);
            return choice;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(c => c != null && c.Id == id);
            return exists;
        }

        public void AddChoice(Choice choice)
        {
            if (choice == null)
            {
                throw new ArgumentNullException(nameof(choice));
            }

            if (ContainsId(choice.Id))
            {
                throw new InvalidOperationException($"Un choix avec l'id {choice.Id} existe déjà dans la collection.");
            }

            if (choice.SceneId == 0)
            {
                choice.AssignToScene(OwnerSceneId);
            }
            else if (choice.SceneId != OwnerSceneId)
            {
                throw new InvalidOperationException(
                    $"Le choix \"{choice.Label}\" appartient déjà à une autre scène (SceneId={choice.SceneId}, attendu={OwnerSceneId}).");
            }

            Add(choice);
        }

        public bool RemoveById(int id)
        {
            Choice? existingChoice = GetById(id);

            if (existingChoice == null)
            {
                return false;
            }

            bool removed = Remove(existingChoice);

            if (removed)
            {
                existingChoice.ClearScene();
            }

            return removed;
        }

        public void ClearReferencesToScene(int sceneId)
        {
            if (sceneId <= 0)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                Choice choice = this[i];

                if (choice != null && choice.TargetSceneId == sceneId)
                {
                    choice.ClearTargetScene();
                }
            }
        }

        public List<Choice> GetAvailableChoices(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            List<Choice> availableChoices = new List<Choice>();

            for (int i = 0; i < Count; i++)
            {
                Choice choice = this[i];

                if (choice == null)
                {
                    continue;
                }

                if (choice.IsAvailable(state))
                {
                    availableChoices.Add(choice);
                }
            }

            return availableChoices;
        }
    }
}
