using CommunityToolkit.Mvvm.ComponentModel;
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
            scenarios = new ObservableCollection<Scenario>();
            RefreshScenarios();
            this.scenarioEditorPage = scenarioEditorPage;
        }


        [ObservableProperty]
        private ObservableCollection<Scenario> scenarios;

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

            bool confirm = await alertService.ShowConfirmation( "Supprimer scénario", $"Voulez-vous vraiment supprimer le scénario \"{scenario.Title}\" ?",  "Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteScenario(scenario.Id);

                RefreshScenarios();

                await alertService.ShowAlert("Scénario supprimé", "Le scénario a bien été supprimé.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }


        public void RefreshScenarios()
        {
            Scenarios = new ObservableCollection<Scenario>(dataAccess.GetAllScenarios());
        }
    }
}
