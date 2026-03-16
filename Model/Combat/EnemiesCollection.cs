using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Combat
{
    public class EnemiesCollection : ObservableCollection<Enemy>
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

        public EnemiesCollection(int ownerScenarioId)
        {
            if (ownerScenarioId <= 0)
            {
                throw new ArgumentException("ownerScenarioId doit être un nombre positif.", nameof(ownerScenarioId));
            }

            OwnerScenarioId = ownerScenarioId;
        }

        public Enemy? GetById(int id)
        {
            Enemy? enemy = this.FirstOrDefault(e => e != null && e.Id == id);
            return enemy;
        }

        public bool ContainsId(int id)
        {
            bool exists = this.Any(e => e != null && e.Id == id);
            return exists;
        }

        public void AddEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (ContainsId(enemy.Id))
            {
                throw new InvalidOperationException($"Un ennemi avec l'id {enemy.Id} existe déjà dans la collection.");
            }

            if (enemy.ScenarioId == 0)
            {
                enemy.AssignToScenario(OwnerScenarioId);
            }
            else if (enemy.ScenarioId != OwnerScenarioId)
            {
                throw new InvalidOperationException(
                    $"L'ennemi \"{enemy.Name}\" appartient déjà à un autre scénario (ScenarioId={enemy.ScenarioId}, attendu={OwnerScenarioId}).");
            }

            Add(enemy);
        }

        public bool RemoveById(int id)
        {
            Enemy? existingEnemy = GetById(id);

            if (existingEnemy == null)
            {
                return false;
            }

            bool removed = Remove(existingEnemy);

            if (removed)
            {
                existingEnemy.ClearScenario();
            }

            return removed;
        }
    }
}
