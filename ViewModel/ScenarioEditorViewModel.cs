using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class ScenarioEditorViewModel : BaseViewModel
    {
        private readonly SceneEditorPage sceneEditorPage;
        private readonly EnemyEditorPage enemyEditorPage;
        private readonly ShopEditorPage shopEditorPage;
        private readonly PlayerCharacterEditorPage playerCharacterEditorPage;

        private Scenario? selectedScenario;

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

            canEditScenarioContent = false;
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

        [ObservableProperty]
        private bool canEditScenarioContent;

        [ObservableProperty]
        private Scene? selectedStartScene;

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(ScenarioTitle))
            {
                await alertService.ShowAlert("Titre invalide", "Le titre du scénario ne peut pas être vide.");
                return;
            }

            if (ScenarioTitle.Trim().Length < 3)
            {
                await alertService.ShowAlert("Titre invalide", "Le titre du scénario doit contenir au moins 3 caractères.");
                return;
            }

            if (ScenarioTitle.Trim().Length > 200)
            {
                await alertService.ShowAlert("Titre invalide", "Le titre du scénario ne peut pas dépasser 200 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ScenarioDescription))
            {
                await alertService.ShowAlert("Description invalide", "La description du scénario ne peut pas être vide.");
                return;
            }

            if (ScenarioDescription.Trim().Length < 30)
            {
                await alertService.ShowAlert("Description invalide", "La description du scénario doit contenir au moins 30 caractères.");
                return;
            }

            try
            {
                if (selectedScenario == null)
                {
                    Scenario scenario = new Scenario(
                        ScenarioTitle.Trim(),
                        ScenarioDescription.Trim()
                    );

                    dataAccess.AddScenario(scenario);

                    await alertService.ShowAlert("Scénario sauvegardé", "Le nouveau scénario a bien été créé.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedScenario.Rename(ScenarioTitle.Trim());
                selectedScenario.ChangeDescription(ScenarioDescription.Trim());

                if (SelectedStartScene != null)
                {
                    selectedScenario.AssignStartScene(SelectedStartScene.Id);
                }
                else
                {
                    selectedScenario.ClearStartScene();
                }

                dataAccess.UpdateScenario(selectedScenario);

                await alertService.ShowAlert("Scénario sauvegardé", "Le scénario a bien été mis à jour.");

            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (playerCharacterEditorPage.BindingContext is PlayerCharacterEditorViewModel playerCharacterEditorViewModel)
            {
                playerCharacterEditorViewModel.PrepareNewPlayerCharacter(selectedScenario);
            }

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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (scene == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer scène",$"Voulez-vous vraiment supprimer la scène \"{scene.Title}\" ?", "Supprimer","Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteScene(scene.Id);

                bool removed = selectedScenario.RemoveSceneById(scene.Id);

                if (!removed)
                {
                    await alertService.ShowAlert("Suppression impossible", "La scène a été supprimée en base, mais pas trouvée dans le scénario chargé.");
                    return;
                }

                RefreshCounts();

                await alertService.ShowAlert("Scène supprimée", "La scène a bien été supprimée.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (enemy == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer ennemi", $"Voulez-vous vraiment supprimer l'ennemi \"{enemy.Name}\" ?","Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteEnemy(enemy.Id);

                selectedScenario.RemoveEnemy(enemy.Id);

                RefreshCounts();

                await alertService.ShowAlert("Ennemi supprimé", "L'ennemi a bien été supprimé.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (shop == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer boutique", $"Voulez-vous vraiment supprimer la boutique \"{shop.Name}\" ?","Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteShop(shop.Id);

                selectedScenario.RemoveShop(shop.Id);

                RefreshCounts();

                await alertService.ShowAlert("Boutique supprimée", "La boutique a bien été supprimée.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

        [RelayCommand()]
        private async Task EditPlayerCharacter(PlayerCharacterTemplate playerCharacter)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (playerCharacter == null)
            {
                return;
            }

            if (playerCharacterEditorPage.BindingContext is PlayerCharacterEditorViewModel playerCharacterEditorViewModel)
            {
                playerCharacterEditorViewModel.LoadPlayerCharacter(selectedScenario, playerCharacter);
            }

            await Shell.Current.Navigation.PushAsync(playerCharacterEditorPage);
        }

        [RelayCommand()]
        private async Task DeletePlayerCharacter(PlayerCharacterTemplate playerCharacter)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (playerCharacter == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer personnage",$"Voulez-vous vraiment supprimer le personnage \"{playerCharacter.Name}\" ?","Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeletePlayerCharacterTemplate(playerCharacter.Id);

                bool removed = selectedScenario.RemovePlayerCharacterById(playerCharacter.Id);

                if (!removed)
                {
                    await alertService.ShowAlert("Suppression impossible", "Le personnage a été supprimé en base, mais pas trouvé dans le scénario chargé.");
                    return;
                }

                RefreshCounts();

                await alertService.ShowAlert("Personnage supprimé", "Le personnage a bien été supprimé.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }

        public void LoadScenario(Scenario scenario)
        {
            selectedScenario = scenario;

            PageTitle = "Édition scénario";

            ScenarioTitle = scenario.Title;
            ScenarioDescription = scenario.Description;

            SetEnemyDisplayRaceNames(scenario.Enemies, scenario.EnemyRaces);

            Scenes = scenario.Scenes;
            Enemies = scenario.Enemies;
            Shops = scenario.Shops;
            PlayerCharacters = scenario.PlayerCharacters;

            CanEditScenarioContent = true;

            SelectedStartScene = GetSceneById(scenario.StartSceneId);

            RefreshCounts();
        }

        public void PrepareNewScenario()
        {
            selectedScenario = null;

            PageTitle = "Nouveau scénario";

            ScenarioTitle = string.Empty;
            ScenarioDescription = string.Empty;

            CanEditScenarioContent = false;

            Scenes = new ScenesCollection();
            Enemies = new EnemiesCollection();
            Shops = new ShopsCollection();
            PlayerCharacters = new PlayerCharactersCollection();

            SelectedStartScene = null;

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

        public void RefreshLoadedScenario()
        {
            if (selectedScenario == null)
            {
                return;
            }

            int scenarioId = selectedScenario.Id;

            string currentTitle = ScenarioTitle;
            string currentDescription = ScenarioDescription;

            Scenario? refreshedScenario = dataAccess.GetScenarioById(scenarioId);

            if (refreshedScenario == null)
            {
                return;
            }

            LoadScenario(refreshedScenario);

            ScenarioTitle = currentTitle;
            ScenarioDescription = currentDescription;
        }

        private void SetEnemyDisplayRaceNames(EnemiesCollection enemies, EnemyRacesCollection enemyRaces)
        {
            if (enemies == null || enemyRaces == null)
            {
                return;
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];

                for (int j = 0; j < enemyRaces.Count; j++)
                {
                    EnemyRace enemyRace = enemyRaces[j];

                    if (enemyRace.Id == enemy.EnemyRaceId)
                    {
                        enemy.SetEnemyRaceName(enemyRace.Name);
                        break;
                    }
                }
            }
        }

        private Scene? GetSceneById(int sceneId)
        {
            if (sceneId <= 0 || Scenes == null)
            {
                return null;
            }

            for (int i = 0; i < Scenes.Count; i++)
            {
                Scene scene = Scenes[i];

                if (scene.Id == sceneId)
                {
                    return scene;
                }
            }

            return null;
        }

        [RelayCommand()]
        private async Task CheckConsistency()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Vérification impossible", "Le scénario doit d'abord être sauvegardé.");
                return;
            }

            try
            {
                int startSceneId = 0;

                if (SelectedStartScene != null)
                {
                    startSceneId = SelectedStartScene.Id;
                }

                Scenario scenarioToValidate = Scenario.Load(
                    selectedScenario.Id,
                    ScenarioTitle.Trim(),
                    ScenarioDescription.Trim(),
                    startSceneId,
                    selectedScenario.Scenes,
                    selectedScenario.Enemies,
                    selectedScenario.EnemyRaces,
                    selectedScenario.Shops,
                    selectedScenario.PlayerCharacters
                );

                List<string> errors;
                bool isPlayable = scenarioToValidate.ValidatePlayable(out errors);

                ValidationResultPopup popup;

                if (isPlayable)
                {
                    popup = new ValidationResultPopup("Vérification terminée", new List<string>() );
                }
                else
                {
                    popup = new ValidationResultPopup( "Problèmes détectés",errors );
                }

                Shell.Current.CurrentPage.ShowPopup(popup);
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur de vérification", exception.Message);
            }
        }

    }
}
