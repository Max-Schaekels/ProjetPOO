using CommunityToolkit.Maui.Views;
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
    public partial class GameScenarioListViewModel : BaseViewModel
    {
        public GameScenarioListViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Choisir un scénario";

            scenarios = new ObservableCollection<Scenario>();

            RefreshScenarios();
        }

        [ObservableProperty]
        private ObservableCollection<Scenario> scenarios;

        public void RefreshScenarios()
        {
            Scenarios = new ObservableCollection<Scenario>(dataAccess.GetAllScenarios());
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task PlayScenario(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            Scenario? loadedScenario = dataAccess.GetScenarioById(scenario.Id);

            if (loadedScenario == null)
            {
                await alertService.ShowAlert("Scénario introuvable", "Le scénario sélectionné n'a pas pu être chargé.");
                return;
            }

            List<string> errors;
            bool isPlayable = loadedScenario.ValidatePlayable(out errors);

            if (!isPlayable)
            {
                ValidationResultPopup popup = new ValidationResultPopup( "Scénario non jouable",errors);

                Shell.Current.CurrentPage.ShowPopup(popup);
                return;
            }

            await alertService.ShowAlert("Scénario jouable", "Le scénario peut être lancé. Prochaine étape : choix du personnage.");
        }

    }
}
