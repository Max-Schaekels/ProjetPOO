using ProjetPOO.Model.Game;
using ProjetPOO.Model.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Data
{
    public class DataAccess
    {
        private List<Scenario> _scenarios;
        private List<SaveGame> _saveGames;

        public DataAccess()
        {
            _scenarios = new List<Scenario>();
            _saveGames = new List<SaveGame>();
        }

        //Partie Scénarios

        public void SaveScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            Scenario? existing = _scenarios.FirstOrDefault(s => s != null && s.Id == scenario.Id);

            if (existing == null)
            {
                _scenarios.Add(scenario);
                return;
            }

            int index = _scenarios.IndexOf(existing);
            _scenarios[index] = scenario;
        }

        public Scenario? GetScenario(int id)
        {
            Scenario? scenario = _scenarios.FirstOrDefault(s => s != null && s.Id == id);
            return scenario;
        }

        public List<Scenario> GetAllScenarios()
        {
            List<Scenario> result = _scenarios
                .Where(s => s != null)
                .ToList();

            return result;
        }

        public bool DeleteScenario(int id)
        {
            Scenario? scenario = _scenarios.FirstOrDefault(s => s != null && s.Id == id);

            if (scenario == null)
            {
                return false;
            }

            return _scenarios.Remove(scenario);
        }

        //Partie Sauvegarde

        public void SaveGame(SaveGame save)
        {
            if (save == null)
            {
                throw new ArgumentNullException(nameof(save));
            }

            SaveGame? existing = _saveGames.FirstOrDefault(s => s != null && s.Id == save.Id);

            if (existing == null)
            {
                _saveGames.Add(save);
                return;
            }

            int index = _saveGames.IndexOf(existing);
            _saveGames[index] = save;
        }

        public SaveGame? GetSaveGame(int id)
        {
            SaveGame? save = _saveGames.FirstOrDefault(s => s != null && s.Id == id);
            return save;
        }

        public List<SaveGame> GetAllSaveGames()
        {
            List<SaveGame> result = _saveGames
                .Where(s => s != null)
                .ToList();

            return result;
        }

        public bool DeleteSaveGame(int id)
        {
            SaveGame? save = _saveGames.FirstOrDefault(s => s != null && s.Id == id);

            if (save == null)
            {
                return false;
            }

            return _saveGames.Remove(save);
        }
    }
}
