using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;

namespace ProjetPOO.ViewModel
{
    public partial class ScenarioEditorViewModel : BaseViewModel
    {
        private readonly SceneEditorPage sceneEditorPage;
        private readonly EnemyEditorPage enemyEditorPage;
        private readonly ShopEditorPage shopEditorPage;
        private readonly PlayerCharacterEditorPage playerCharacterEditorPage;

        private Scenario? selectedScenario;
        private Scene? selectedScene;

        public ScenarioEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, SceneEditorPage sceneEditorPage, EnemyEditorPage enemyEditorPage, ShopEditorPage shopEditorPage, PlayerCharacterEditorPage playerCharacterEditorPage) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition scénario";

            this.sceneEditorPage = sceneEditorPage;
            this.enemyEditorPage = enemyEditorPage;
            this.shopEditorPage = shopEditorPage;
            this.playerCharacterEditorPage = playerCharacterEditorPage;

            scenarioTitle = string.Empty;
            scenarioDescription = string.Empty;

            scenes = new ScenesCollection();
            enemies = new EnemiesCollection();
            shops = new ShopsCollection();
            playerCharacters = new PlayerCharactersCollection();


            scenesCount = 0;
            enemiesCount = 0;
            shopsCount = 0;
            playerCharactersCount = 0;

            isScenesEmpty = true;
            isEnemiesEmpty = true;
            isShopsEmpty = true;
            isPlayerCharactersEmpty = true;
        }


        [ObservableProperty]
        private string scenarioTitle;

        [ObservableProperty]
        private string scenarioDescription;

        [ObservableProperty]
        private ScenesCollection scenes;

        [ObservableProperty]
        private EnemiesCollection enemies;

        [ObservableProperty]
        private ShopsCollection shops;

        [ObservableProperty]
        private PlayerCharactersCollection playerCharacters;

        [ObservableProperty]
        private int scenesCount;

        [ObservableProperty]
        private int enemiesCount;

        [ObservableProperty]
        private int shopsCount;

        [ObservableProperty]
        private int playerCharactersCount;

        [ObservableProperty]
        private bool isScenesEmpty;

        [ObservableProperty]
        private bool isEnemiesEmpty;

        [ObservableProperty]
        private bool isShopsEmpty;

        [ObservableProperty]
        private bool isPlayerCharactersEmpty;

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder scénario", "La sauvegarde du scénario sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task AddScene()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (sceneEditorPage.BindingContext is SceneEditorViewModel sceneEditorViewModel)
            {
                sceneEditorViewModel.PrepareNewScene(selectedScenario);
            }

            await Shell.Current.Navigation.PushAsync(sceneEditorPage);
        }

        [RelayCommand()]
        private async Task AddEnemy()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (enemyEditorPage.BindingContext is EnemyEditorViewModel enemyEditorViewModel)
            {
                enemyEditorViewModel.PrepareNewEnemy(selectedScenario);
            }

            await Shell.Current.Navigation.PushAsync(enemyEditorPage);
        }

        [RelayCommand()]
        private async Task AddShop()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (shopEditorPage.BindingContext is ShopEditorViewModel shopEditorViewModel)
            {
                shopEditorViewModel.PrepareNewShop(selectedScenario);
            }

            await Shell.Current.Navigation.PushAsync(shopEditorPage);
        }

        [RelayCommand()]
        private async Task AddPlayerCharacter()
        {
            await Shell.Current.Navigation.PushAsync(playerCharacterEditorPage);
        }

        [RelayCommand()]
        private async Task EditScene(Scene scene)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (scene == null)
            {
                return;
            }

            if (sceneEditorPage.BindingContext is SceneEditorViewModel sceneEditorViewModel)
            {
                sceneEditorViewModel.LoadScene(selectedScenario, scene);
            }

            await Shell.Current.Navigation.PushAsync(sceneEditorPage);
        }

        [RelayCommand()]
        private async Task DeleteScene(Scene scene)
        {
            if (scene == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer scène", $"La suppression de la scène \"{scene.Title}\" sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task EditEnemy(Enemy enemy)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (enemy == null)
            {
                return;
            }

            if (enemyEditorPage.BindingContext is EnemyEditorViewModel enemyEditorViewModel)
            {
                enemyEditorViewModel.LoadEnemy(selectedScenario, enemy);
            }

            await Shell.Current.Navigation.PushAsync(enemyEditorPage);
        }

        [RelayCommand()]
        private async Task DeleteEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer ennemi", "La suppression de cet ennemi sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task EditShop(Shop shop)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (shop == null)
            {
                return;
            }

            if (shopEditorPage.BindingContext is ShopEditorViewModel shopEditorViewModel)
            {
                shopEditorViewModel.LoadShop(selectedScenario, shop);
            }

            await Shell.Current.Navigation.PushAsync(shopEditorPage);

        }

        [RelayCommand()]
        private async Task DeleteShop(Shop shop)
        {
            if (shop == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer boutique", $"La suppression de la boutique \"{shop.Name}\" sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task EditPlayerCharacter(PlayerCharacterTemplate playerCharacter)
        {
            if (playerCharacter == null)
            {
                return;
            }

            await alertService.ShowAlert("Modifier personnage", $"L'édition du personnage \"{playerCharacter.Name}\" sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task DeletePlayerCharacter(PlayerCharacterTemplate playerCharacter)
        {
            if (playerCharacter == null)
            {
                return;
            }

            await alertService.ShowAlert("Supprimer personnage", $"La suppression du personnage \"{playerCharacter.Name}\" sera ajoutée plus tard.");
        }

        public void LoadScenario(Scenario scenario)
        {
            selectedScenario = scenario;

            PageTitle = "Édition scénario";

            ScenarioTitle = scenario.Title;
            ScenarioDescription = scenario.Description;

            Scenes = scenario.Scenes;
            Enemies = scenario.Enemies;
            Shops = scenario.Shops;
            PlayerCharacters = scenario.PlayerCharacters;

            RefreshCounts();
        }

        public void PrepareNewScenario()
        {
            selectedScenario = null;

            PageTitle = "Nouveau scénario";

            ScenarioTitle = string.Empty;
            ScenarioDescription = string.Empty;

            Scenes = new ScenesCollection();
            Enemies = new EnemiesCollection();
            Shops = new ShopsCollection();
            PlayerCharacters = new PlayerCharactersCollection();

            RefreshCounts();
        }

        private void RefreshCounts()
        {
            ScenesCount = Scenes == null ? 0 : Scenes.Count;
            EnemiesCount = Enemies == null ? 0 : Enemies.Count;
            ShopsCount = Shops == null ? 0 : Shops.Count;
            PlayerCharactersCount = PlayerCharacters == null ? 0 : PlayerCharacters.Count;

            IsScenesEmpty = ScenesCount == 0;
            IsEnemiesEmpty = EnemiesCount == 0;
            IsShopsEmpty = ShopsCount == 0;
            IsPlayerCharactersEmpty = PlayerCharactersCount == 0;
        }



    }
}
