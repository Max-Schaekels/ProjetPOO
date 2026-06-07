using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Game;
using ProjetPOO.Model.Gameplay;
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
    public partial class PlayerCharacterSelectionViewModel : BaseViewModel
    {
        private Scenario? selectedScenario;
        private readonly GamePage gamePage;
        public PlayerCharacterSelectionViewModel(IAlertService alertService, IDataAccess dataAccessService, GamePage gamePage) : base(alertService, dataAccessService)
        {
            PageTitle = "Choisir un personnage";
            this.gamePage = gamePage;

            playerCharacters = new ObservableCollection<PlayerCharacterTemplate>();
        }

        [ObservableProperty]
        private ObservableCollection<PlayerCharacterTemplate> playerCharacters;

        /// <summary>
        /// Loads the selected scenario and displays the available player characters.
        /// This method is called before navigating to the character selection page.
        /// </summary>
        /// <param name="scenario">Scenario selected by the user.</param>
        public void LoadScenario(Scenario scenario)
        {
            selectedScenario = scenario;

            PageTitle = $"Personnage - {scenario.Title}";

            PlayerCharacters = new ObservableCollection<PlayerCharacterTemplate>(scenario.PlayerCharacters);
           
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        /// <summary>
        /// Creates the initial game state with the selected player character and opens the game page.
        /// The selected scenario is started from its configured start scene.
        /// </summary>
        /// <param name="playerCharacter">Player character selected by the user.</param>
        /// <returns>Asynchronous task.</returns>
        [RelayCommand()]
        private async Task StartGame(PlayerCharacterTemplate playerCharacter)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (playerCharacter == null)
            {
                await alertService.ShowAlert("Personnage manquant", "Veuillez sélectionner un personnage.");
                return;
            }

            if (selectedScenario.StartSceneId <= 0)
            {
                await alertService.ShowAlert("Scène de départ manquante", "Le scénario ne possède pas de scène de départ.");
                return;
            }

            try
            {
                PlayerCharacterInstance playerCharacterInstance = playerCharacter.CreateInstance();

                Inventory inventory = new Inventory(0, 0);

                // Valeur temporaire pour tester la boutique.
                // À remplacer plus tard par une vraie règle de départ.
                int startingGold = 30;

                GameState gameState = new GameState( selectedScenario.StartSceneId, selectedScenario.Id,startingGold,inventory, playerCharacterInstance);

                GameEngine gameEngine = new GameEngine(gameState);
                gameEngine.StartScenario(selectedScenario);

                if (gamePage.BindingContext is GameViewModel gameViewModel)
                {
                    gameViewModel.LoadGame(selectedScenario, gameEngine);
                }

                await Shell.Current.Navigation.PushAsync(gamePage);
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur lancement", exception.Message);
            }
        }
    }
}
