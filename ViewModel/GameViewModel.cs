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

            playerSummary = string.Empty;
            inventorySummary = string.Empty;
            goldSummary = string.Empty;

            isShopVisible = false;
            shopSummary = string.Empty;

            isCombatVisible = false;
            combatSummary = string.Empty;

            areChoicesVisible = false;

            sceneImageSource = string.Empty;
            hasSceneImage = false;
            hasNoSceneImage = true;

            playerIdentitySummary = string.Empty;
            playerStatsSummary = string.Empty;
            sceneDisplayTitle = string.Empty;

            currentGameState = null;
        }

        [ObservableProperty]
        private string sceneTitle;

        [ObservableProperty]
        private string sceneText;

        [ObservableProperty]
        private ObservableCollection<Choice> availableChoices;

        [ObservableProperty]
        private string playerSummary;

        [ObservableProperty]
        private string inventorySummary;

        [ObservableProperty]
        private string goldSummary;

        [ObservableProperty]
        private bool isShopVisible;

        [ObservableProperty]
        private string shopSummary;

        [ObservableProperty]
        private bool isCombatVisible;

        [ObservableProperty]
        private string combatSummary;

        [ObservableProperty]
        private bool areChoicesVisible;

        [ObservableProperty]
        private string sceneImageSource;

        [ObservableProperty]
        private bool hasSceneImage;

        [ObservableProperty]
        private bool hasNoSceneImage;

        [ObservableProperty]
        private string playerIdentitySummary;

        [ObservableProperty]
        private string playerStatsSummary;

        [ObservableProperty]
        private string sceneDisplayTitle;

        [ObservableProperty]
        private GameState? currentGameState;

        public void LoadGame(Scenario scenario, GameEngine engine)
        {
            selectedScenario = scenario;
            gameEngine = engine;
            CurrentGameState = engine.State;

            PageTitle = scenario.Title;

            RefreshCurrentView();
        }

        [RelayCommand()]
        private async Task Back()
        {
            bool confirm = await alertService.ShowConfirmation("Quitter la partie","Voulez-vous vraiment quitter la partie en cours ? La sauvegarde sera ajoutée plus tard.","Quitter", "Annuler");

            if (!confirm)
            {
                return;
            }

            await Shell.Current.Navigation.PopAsync();
        }

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
                SceneImageSource = currentScene.PictureFileName;
                HasSceneImage = true;
                HasNoSceneImage = false;
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
            CombatSummary = string.Empty;

            if (IsCombatVisible && state.CurrentCombat != null)
            {
                CombatSummary =
                    $"Combat contre {state.CurrentCombat.Enemy.Name}\n" +
                    $"Ennemi PV : {state.CurrentCombat.Enemy.CurrentHp}/{state.CurrentCombat.Enemy.MaxHp}";
            }

            AvailableChoices = new ObservableCollection<Choice>();

            if (!state.IsInCombat())
            {
                List<Choice> choices = gameEngine.GetAvailableChoices(selectedScenario);

                for (int i = 0; i < choices.Count; i++)
                {
                    AvailableChoices.Add(choices[i]);
                }
            }

            AreChoicesVisible = AvailableChoices.Count > 0 && !state.IsInCombat();
        }
    }
}
