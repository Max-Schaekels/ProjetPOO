using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
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
        private readonly ScenarioEditorPage scenarioEditorPage;
        public ScenarioListViewModel(IAlertService alertService, IDataAccess dataAccess, ScenarioEditorPage scenarioEditorPage) : base(alertService, dataAccess)
        {
            PageTitle = "Liste des scénarios";
            Scenarios = new ObservableCollection<Scenario>(dataAccess.GetAllScenarios());
            this.scenarioEditorPage = scenarioEditorPage;
        }

        public ObservableCollection<Scenario> Scenarios { get; set; }

        [RelayCommand()]
        private async Task NewScenario()
        {
            await Shell.Current.Navigation.PushAsync(scenarioEditorPage);
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
