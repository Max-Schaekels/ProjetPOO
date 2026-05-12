using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class EnemyRacesCollection : ObservableCollection<EnemyRace>
    {
        private int _ownerScenarioId;

        public int OwnerScenarioId
        {
            get => _ownerScenarioId;
            private set => _ownerScenarioId = value;
        }

        public EnemyRacesCollection()
        {
            OwnerScenarioId = 0;
        }

        public EnemyRacesCollection(int ownerScenarioId)
        {
            OwnerScenarioId = ownerScenarioId;
        }

        public void AddEnemyRace(EnemyRace enemyRace)
        {
            if (enemyRace == null)
            {
                throw new ArgumentNullException(nameof(enemyRace));
            }

            if (OwnerScenarioId > 0)
            {
                enemyRace.AssignToScenario(OwnerScenarioId);
            }

            Add(enemyRace);
        }

        public EnemyRace? GetById(int enemyRaceId)
        {
            for (int i = 0; i < Count; i++)
            {
                EnemyRace enemyRace = this[i];

                if (enemyRace.Id == enemyRaceId)
                {
                    return enemyRace;
                }
            }

            return null;
        }

        public bool ContainsName(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                EnemyRace enemyRace = this[i];

                if (enemyRace.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveEnemyRace(int enemyRaceId)
        {
            EnemyRace? enemyRace = GetById(enemyRaceId);

            if (enemyRace != null)
            {
                enemyRace.ClearScenario();
                Remove(enemyRace);
            }
        }
    }
}
