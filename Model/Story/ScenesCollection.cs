using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class ScenesCollection : ObservableCollection<Scene>
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

        public ScenesCollection(int ownerScenarioId)
        {
            if (ownerScenarioId <= 0)
            {
                throw new ArgumentException("ownerScenarioId doit être un nombre positif.", nameof(ownerScenarioId));
            }

            OwnerScenarioId = ownerScenarioId;
        }

        public Scene? GetById(int id)
        {
            Scene? scene = this.FirstOrDefault(s => s != null && s.Id == id);
            return scene;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(s => s != null && s.Id == id);
            return exists;
        }

        public void AddScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            if (ContainsId(scene.Id))
            {
                throw new InvalidOperationException($"Une scène avec l'id {scene.Id} existe déjà dans la collection.");
            }

            if (scene.ScenarioId == 0)
            {
                scene.AssignToScenario(OwnerScenarioId);
            }
            else if (scene.ScenarioId != OwnerScenarioId)
            {
                throw new InvalidOperationException(
                    $"La scène \"{scene.Title}\" appartient déjà à un autre scénario (ScenarioId={scene.ScenarioId}, attendu={OwnerScenarioId}).");
            }

            Add(scene);
        }

        public bool RemoveById(int id)
        {
            Scene? existingScene = GetById(id);

            if (existingScene == null)
            {
                return false;
            }

            bool removed = Remove(existingScene);

            if (removed)
            {
                existingScene.ClearScenario();
                ClearReferencesToScene(id);
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
                Scene scene = this[i];

                if (scene != null)
                {
                    scene.ClearReferencesToScene(sceneId);
                }
            }
        }
    }
}
