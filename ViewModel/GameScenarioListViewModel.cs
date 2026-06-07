using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
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
        private readonly PlayerCharacterSelectionPage playerCharacterSelectionPage;
        private readonly SaveGameListPage saveGameListPage;
        public GameScenarioListViewModel(IAlertService alertService, IDataAccess dataAccessService, PlayerCharacterSelectionPage playerCharacterSelectionPage, SaveGameListPage saveGameListPage) : base(alertService, dataAccessService)
        {
            this.playerCharacterSelectionPage = playerCharacterSelectionPage;
            this.saveGameListPage = saveGameListPage;

            PageTitle = "Nouvelle partie";

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
        private async Task GoToSaveGameList()
        {
            await Shell.Current.Navigation.PushAsync(saveGameListPage);
        }

        /// <summary>
        /// Loads the selected scenario and checks if it is playable before starting a new game.
        /// If the scenario is valid, the user is redirected to the character selection page.
        /// </summary>
        /// <param name="scenario">Scenario selected by the user.</param>
        /// <returns>Asynchronous task.</returns>
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

            SetEnemyDisplayRaceNames(loadedScenario);

            List<string> errors;
            bool isPlayable = loadedScenario.ValidatePlayable(out errors);

            if (!isPlayable)
            {
                ValidationResultPopup popup = new ValidationResultPopup( "Scénario non jouable",errors);

                Shell.Current.CurrentPage.ShowPopup(popup);
                return;
            }

            if (playerCharacterSelectionPage.BindingContext is PlayerCharacterSelectionViewModel viewModel)
            {
                viewModel.LoadScenario(loadedScenario);
            }

            await Shell.Current.Navigation.PushAsync(playerCharacterSelectionPage);
        }

        /// <summary>
        /// Sets the display race name of each enemy loaded from SQL.
        /// Enemies only store the race identifier, so the race name is restored for display purposes.
        /// </summary>
        /// <param name="scenario">Scenario containing enemies and enemy races.</param>
        private void SetEnemyDisplayRaceNames(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            for (int i = 0; i < scenario.Enemies.Count; i++)
            {
                Enemy enemy = scenario.Enemies[i];

                for (int j = 0; j < scenario.EnemyRaces.Count; j++)
                {
                    EnemyRace enemyRace = scenario.EnemyRaces[j];

                    if (enemyRace.Id == enemy.EnemyRaceId)
                    {
                        enemy.SetEnemyRaceName(enemyRace.Name);
                        break;
                    }
                }
            }
        }

    }
}
