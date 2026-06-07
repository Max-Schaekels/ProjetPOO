using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Game;
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
    public partial class SaveGameListViewModel : BaseViewModel
    {
        private readonly GamePage gamePage;
        public SaveGameListViewModel(IAlertService alertService, IDataAccess dataAccessService, GamePage gamePage) : base(alertService, dataAccessService)
        {
            this.gamePage = gamePage;
            PageTitle = "Charger partie";

            saveGames = new ObservableCollection<SaveGame>();

            RefreshSaveGames();
        }

        [ObservableProperty]
        private ObservableCollection<SaveGame> saveGames;

        /// <summary>
        /// Reloads the list of save games from the SQL database.
        /// This method is called when the save game list page appears or after a deletion.
        /// </summary>
        public void RefreshSaveGames()
        {
            SaveGames = new ObservableCollection<SaveGame>(dataAccess.GetAllSaveGames());
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        /// <summary>
        /// Loads the selected save game from the SQL database and opens the game page.
        /// The game engine is recreated from the saved game state.
        /// </summary>
        /// <param name="saveGame">Save game selected by the user.</param>
        /// <returns>Asynchronous task.</returns>
        [RelayCommand()]
        private async Task LoadSaveGame(SaveGame saveGame)
        {
            if (saveGame == null)
            {
                return;
            }

            try
            {
                SaveGame? loadedSaveGame = dataAccess.GetSaveGameById(saveGame.Id);

                if (loadedSaveGame == null)
                {
                    await alertService.ShowAlert("Sauvegarde introuvable", "La sauvegarde sélectionnée n'a pas pu être chargée.");
                    return;
                }

                GameState loadedState = loadedSaveGame.State;

                Scenario? loadedScenario = dataAccess.GetScenarioById(loadedState.ScenarioId);

                if (loadedScenario == null)
                {
                    await alertService.ShowAlert("Scénario introuvable", "Le scénario lié à cette sauvegarde n'existe plus.");
                    return;
                }

                SetEnemyDisplayRaceNames(loadedScenario);

                GameEngine gameEngine = new GameEngine(loadedState);

                if (gamePage.BindingContext is GameViewModel gameViewModel)
                {
                    gameViewModel.LoadGame(loadedScenario, gameEngine);
                }

                await Shell.Current.Navigation.PushAsync(gamePage);
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur chargement", exception.Message);
            }
        }

        /// <summary>
        /// Deletes the selected save game and its related runtime data from the SQL database.
        /// A confirmation message is displayed before deleting the save.
        /// </summary>
        /// <param name="saveGame">Save game selected by the user.</param>
        /// <returns>Asynchronous task.</returns>
        [RelayCommand()]
        private async Task DeleteSaveGame(SaveGame saveGame)
        {
            if (saveGame == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer la sauvegarde", $"Voulez-vous vraiment supprimer la sauvegarde \"{saveGame.Name}\" ?","Supprimer", "Annuler" );

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteSaveGame(saveGame.Id);

                RefreshSaveGames();

                await alertService.ShowAlert("Suppression", "La sauvegarde a été supprimée.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

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
