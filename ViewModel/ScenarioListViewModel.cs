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
            scenarioEditorPage.PrepareNewScenario();
            await Shell.Current.Navigation.PushAsync(scenarioEditorPage);
        }

        [RelayCommand()]
        private async Task EditScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            scenarioEditorPage.LoadScenario(scenario);
            await Shell.Current.Navigation.PushAsync(scenarioEditorPage);
        }

        [RelayCommand()]
        private async Task DeleteScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation(
                "Supprimer scénario",
                $"Voulez-vous vraiment supprimer le scénario \"{scenario.Title}\" ?",
                "Supprimer",
                "Annuler");

            if (!confirm)
            {
                return;
            }

            bool removed = Scenarios.Remove(scenario);

            if (!removed)
            {
                await alertService.ShowAlert("Suppression impossible", "Le scénario n'a pas pu être supprimé.");
            }
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

    }
}
