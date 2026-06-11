using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Game;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
using System.Collections.ObjectModel;
using ProjetPOO.Model.Combat.Enums;

namespace ProjetPOO.ViewModel
{
    public partial class GameViewModel : BaseViewModel
    {
        private Scenario? selectedScenario;
        private GameEngine? gameEngine;



        public GameViewModel(IAlertService alertService, IDataAccess dataAccess) : base(alertService, dataAccess)
        {
            PageTitle = "Jeu";

            sceneTitle = string.Empty;
            sceneText = string.Empty;
            availableChoices = new ObservableCollection<Choice>();

            isShopVisible = false;
            shopSummary = string.Empty;

            isCombatVisible = false;

            areChoicesVisible = false;

            sceneImageSource = string.Empty;
            hasSceneImage = false;
            hasNoSceneImage = true;

            sceneDisplayTitle = string.Empty;

            currentGameState = null;

            roundReportText = string.Empty;
            hasRoundReport = false;
            canUsePotionInCombat = false;
        }

        [ObservableProperty]
        private string sceneTitle;

        [ObservableProperty]
        private string sceneText;

        [ObservableProperty]
        private ObservableCollection<Choice> availableChoices;


        [ObservableProperty]
        private bool isShopVisible;

        [ObservableProperty]
        private string shopSummary;

        [ObservableProperty]
        private bool isCombatVisible;


        [ObservableProperty]
        private bool areChoicesVisible;

        [ObservableProperty]
        private string sceneImageSource;

        [ObservableProperty]
        private bool hasSceneImage;

        [ObservableProperty]
        private bool hasNoSceneImage;


        [ObservableProperty]
        private string sceneDisplayTitle;

        [ObservableProperty]
        private GameState? currentGameState;

        [ObservableProperty]
        private string roundReportText;

        [ObservableProperty]
        private bool hasRoundReport;

        [ObservableProperty]
        private bool canUsePotionInCombat;

        /// <summary>
        /// Loads a scenario and a game engine into the game page.
        /// This method is used both for a new game and for a loaded save game.
        /// </summary>
        /// <param name="scenario">Scenario currently played.</param>
        /// <param name="engine">Game engine containing the current game state.</param>
        public void LoadGame(Scenario scenario, GameEngine engine)
        {
            selectedScenario = scenario;
            gameEngine = engine;
            CurrentGameState = engine.State;

            PageTitle = scenario.Title;

            RoundReportText = string.Empty;
            HasRoundReport = false;

            RefreshCurrentView();
        }

        [RelayCommand()]
        private async Task Back()
        {
            bool confirm = await alertService.ShowConfirmation("Quitter la partie", "Voulez-vous vraiment quitter la partie en cours ? Pensez à sauvegarder avant de quitter.", "Quitter", "Annuler");

            if (!confirm)
            {
                return;
            }

            await Shell.Current.Navigation.PopAsync();
        }

        /// <summary>
        /// Saves the current game state into the SQL database.
        /// Saving during combat is blocked because combat state is not persisted yet.
        /// </summary>
        /// <returns>Asynchronous task.</returns>
        [RelayCommand()]
        private async Task SaveGame()
        {
            if (gameEngine == null)
            {
                await alertService.ShowAlert("Erreur", "Aucune partie n'est chargée.");
                return;
            }

            GameState state = gameEngine.State;

            if (state.IsInCombat())
            {
                await alertService.ShowAlert( "Sauvegarde impossible", "La sauvegarde pendant un combat n'est pas encore prise en charge.");

                return;
            }

            try
            {
                Scene currentScene = gameEngine.GetCurrentScene(selectedScenario);

                string automaticSaveName = $"{selectedScenario.Title} - {currentScene.Title} - {DateTime.Now:dd/MM HH:mm}";

                string customSaveName = await alertService.ShowPrompt(
                    "Nom de la sauvegarde",
                    $"Nom proposé : {automaticSaveName}"
                );

                string saveName = automaticSaveName;

                if (!string.IsNullOrWhiteSpace(customSaveName))
                {
                    saveName = customSaveName.Trim();
                }

                int inventoryId = dataAccess.AddInventory(state.PlayerInventory);
                int playerCharacterInstanceId = dataAccess.AddPlayerCharacterInstance(state.PlayerCharacter);
                int gameStateId = dataAccess.AddGameState(state, inventoryId, playerCharacterInstanceId);

                IReadOnlyList<string> flags = state.Flags;

                for (int i = 0; i < flags.Count; i++)
                {
                    dataAccess.AddGameStateFlag(gameStateId, flags[i]);
                }

                SaveGame saveGame = new SaveGame(saveName, state);

                dataAccess.AddSaveGame(saveGame, gameStateId);

                await alertService.ShowAlert("Sauvegarde", $"La partie a été sauvegardée :\n{saveName}");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
        }


        /// <summary>
        /// Executes the selected narrative choice and refreshes the game page.
        /// The choice can change the current scene, update resources or trigger gameplay effects.
        /// </summary>
        /// <param name="choice">Choice selected by the player.</param>
        /// <returns>Asynchronous task.</returns>
        [RelayCommand()]
        private async Task PlayChoice(Choice choice)
        {
            if (selectedScenario == null || gameEngine == null)
            {
                await alertService.ShowAlert("Erreur", "Aucune partie n'est chargée.");
                return;
            }

            if (choice == null)
            {
                return;
            }

            try
            {
                RoundReportText = string.Empty;
                HasRoundReport = false;
                gameEngine.PlayChoice(selectedScenario, choice.Id);

                RefreshCurrentView();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur choix", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task BuyPotion()
        {
            if (selectedScenario == null || gameEngine == null)
            {
                await alertService.ShowAlert("Erreur", "Aucune partie n'est chargée.");
                return;
            }

            try
            {
                bool bought = gameEngine.BuyPotionInCurrentShop(selectedScenario);

                if (!bought)
                {
                    await alertService.ShowAlert("Achat impossible", "Vous n'avez pas assez d'or pour acheter une potion.");
                    return;
                }

                RefreshCurrentView();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur achat", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task BuyKey()
        {
            if (selectedScenario == null || gameEngine == null)
            {
                await alertService.ShowAlert("Erreur", "Aucune partie n'est chargée.");
                return;
            }

            try
            {
                bool bought = gameEngine.BuyKeyInCurrentShop(selectedScenario);

                if (!bought)
                {
                    await alertService.ShowAlert("Achat impossible", "Vous n'avez pas assez d'or pour acheter une clé.");
                    return;
                }

                RefreshCurrentView();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur achat", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task Attack()
        {
            await PlayCombatAction(CombatActionType.Attack);
        }

        [RelayCommand()]
        private async Task Defend()
        {
            await PlayCombatAction(CombatActionType.Defend);
        }

        [RelayCommand()]
        private async Task UsePotion()
        {
            await PlayCombatAction(CombatActionType.UseItem);
        }

        [RelayCommand()]
        private async Task Flee()
        {
            await PlayCombatAction(CombatActionType.Flee);
        }

        /// <summary>
        /// Refreshes all properties displayed by the game page.
        /// This method is used after scene changes, shop actions and combat actions.
        /// </summary>
        private void RefreshCurrentView()
        {
            if (selectedScenario == null || gameEngine == null)
            {
                return;
            }

            Scene currentScene = gameEngine.GetCurrentScene(selectedScenario);
            GameState state = gameEngine.State;

            SceneTitle = currentScene.Title;
            SceneText = currentScene.Text;
            SceneDisplayTitle = currentScene.Title;

            if (string.IsNullOrWhiteSpace(currentScene.PictureFileName))
            {
                SceneImageSource = string.Empty;
                HasSceneImage = false;
                HasNoSceneImage = true;
            }
            else
            {
                string fullImagePath = GetSceneImageFullPath(currentScene.PictureFileName);

                if (File.Exists(fullImagePath))
                {
                    SceneImageSource = fullImagePath;
                    HasSceneImage = true;
                    HasNoSceneImage = false;
                }
                else
                {
                    SceneImageSource = string.Empty;
                    HasSceneImage = false;
                    HasNoSceneImage = true;
                }
            }

            IsShopVisible = currentScene.Type == SceneType.Shop;
            ShopSummary = string.Empty;

            if (IsShopVisible)
            {
                Shop shop = gameEngine.GetCurrentShop(selectedScenario);

                SceneDisplayTitle = shop.Name;

                ShopSummary = $"Potion : {shop.PotionPrice} or | Clé : {shop.KeyPrice} or";
            }

            IsCombatVisible = state.IsInCombat();


            AvailableChoices = new ObservableCollection<Choice>();

            if (!state.IsInCombat())
            {
                List<Choice> choices = gameEngine.GetAvailableChoices(selectedScenario);

                for (int i = 0; i < choices.Count; i++)
                {
                    AvailableChoices.Add(choices[i]);
                }
            }
            CanUsePotionInCombat = state.IsInCombat() && state.PlayerInventory.PotionsCount > 0;
            AreChoicesVisible = AvailableChoices.Count > 0 && !state.IsInCombat();
        }

        private async Task PlayCombatAction(CombatActionType actionType)
        {
            if (selectedScenario == null || gameEngine == null)
            {
                await alertService.ShowAlert("Erreur", "Aucune partie n'est chargée.");
                return;
            }

            if (!gameEngine.State.IsInCombat())
            {
                await alertService.ShowAlert("Erreur combat", "Aucun combat n'est en cours.");
                return;
            }

            try
            {
                RoundReport report = gameEngine.PlayRound(selectedScenario, actionType);

                RoundReportText = BuildRoundReportText(report);
                HasRoundReport = true;

                RefreshCurrentView();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur combat", exception.Message);
            }
        }

        private string BuildRoundReportText(RoundReport report)
        {
            if (report == null)
            {
                return string.Empty;
            }

            List<string> lines = new List<string>();

            if (report.PlayerDamage > 0)
            {
                lines.Add($"Vous infligez {report.PlayerDamage} dégât(s).");
            }
            else if (!report.PotionAttempted && !report.FleeAttempted)
            {
                lines.Add("Votre action n'inflige aucun dégât.");
            }

            if (report.PotionAttempted)
            {
                if (report.PotionConsumed)
                {
                    lines.Add("Vous utilisez une potion et récupérez des PV.");
                }
                else
                {
                    lines.Add("Vous n'avez aucune potion à utiliser.");
                }
            }

            if (report.FleeAttempted)
            {
                if (report.FledSuccessfully)
                {
                    lines.Add("Vous parvenez à fuir le combat.");
                }
                else
                {
                    lines.Add("Vous tentez de fuir, mais échouez.");
                }
            }

            if (report.EnemyDamage > 0)
            {
                lines.Add($"L'ennemi vous inflige {report.EnemyDamage} dégât(s).");
            }
            else if (report.Result == CombatResult.InProgress)
            {
                lines.Add("L'ennemi n'inflige aucun dégât.");
            }

            if (report.Result == CombatResult.Victory)
            {
                lines.Add("Victoire !");
                lines.Add($"Expérience gagnée : {report.ExperienceGained}");

                if (!string.IsNullOrWhiteSpace(report.LootDescription))
                {
                    lines.Add($"Butin : {report.LootDescription}");
                }
            }
            else if (report.Result == CombatResult.Defeat)
            {
                lines.Add("Défaite...");
            }
            else if (report.Result == CombatResult.Fled)
            {
                lines.Add("Vous quittez le combat.");
            }

            return string.Join(Environment.NewLine, lines);
        }

        private string GetScenesImagesDirectoryPath()
        {
            string applicationDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            DirectoryInfo? currentDirectory = new DirectoryInfo(applicationDirectoryPath);

            while (currentDirectory != null && !Directory.Exists(Path.Combine(currentDirectory.FullName, "Configuration", "Datas")))
            {
                currentDirectory = currentDirectory.Parent;
            }

            if (currentDirectory == null)
            {
                throw new InvalidOperationException("Impossible de retrouver le dossier Configuration/Datas.");
            }

            string imagesDirectoryPath = Path.Combine(currentDirectory.FullName, "Configuration", "Datas", "Images", "Scenes");

            return imagesDirectoryPath;
        }

        private string GetSceneImageFullPath(string pictureFileName)
        {
            string imagesDirectoryPath = GetScenesImagesDirectoryPath();
            string fullImagePath = Path.Combine(imagesDirectoryPath, pictureFileName);

            return fullImagePath;
        }
    }
}
