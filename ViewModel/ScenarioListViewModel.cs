using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class ScenarioListViewModel : BaseViewModel
    {
        public ScenarioListViewModel(IAlertService alertService, IDataAccess dataAccess) : base(alertService, dataAccess)
        {
            PageTitle = "Liste des scénarios";
            Scenarios = new ObservableCollection<Scenario>(dataAccess.GetAllScenarios());
        }

        public ObservableCollection<Scenario> Scenarios { get; set; }

        [RelayCommand()]
        private async Task NewScenario()
        {
            await alertService.ShowAlert("Nouveau scénario", "La création de scénario sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task EditScenario(Scenario scenario)
        {
            await alertService.ShowAlert("Éditer scénario", $"L'édition du scénario '{scenario.Title}' sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task DeleteScenario(Scenario scenario)
        {
            await alertService.ShowAlert("Supprimer scénario", $"La suppression du scénario '{scenario.Title}' sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
