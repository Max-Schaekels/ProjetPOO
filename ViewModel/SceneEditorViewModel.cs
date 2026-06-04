using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.DataAccess.Files;
using System.IO;
using System.Text;
using ProjetPOO.Utilities.EntriesValidation;
using ProjetPOO.View;


namespace ProjetPOO.ViewModel
{
    public partial class SceneEditorViewModel : BaseViewModel
    {
        private readonly DataFilesManager _dataFilesManager;
        private readonly ChoiceEditorPage choiceEditorPage;
        private Scenario? selectedScenario;
        private Scene? selectedScene;
        public SceneEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, DataFilesManager dataFilesManager, ChoiceEditorPage choiceEditorPage) : base(alertService, dataAccessService)
        {
            _dataFilesManager = dataFilesManager;
            this.choiceEditorPage = choiceEditorPage;
            PageTitle = "Éditeur de scène";
            sceneTitle = "Nouvelle Scène";
            sceneText = string.Empty;
            selectedSceneType = SceneType.Normal;
            pictureFileName = string.Empty;

            enemies = new EnemiesCollection();
            shops = new ShopsCollection();
            choices = new ChoicesCollection();
            sceneTypes = new List<SceneType>
            {
                SceneType.Normal,
                SceneType.Combat,
                SceneType.Shop,
                SceneType.End
            };
            availableScenesForCombatTargets = new ScenesCollection();

            selectedEnemy = null;
            selectedShop = null;
            selectedVictoryTargetScene = null;
            selectedDefeatTargetScene = null;
            selectedFleeTargetScene = null;

            SelectedImageFileName = "Aucune image sélectionnée";

        }

        [ObservableProperty]
        private string sceneTitle;

        [ObservableProperty]
        private string sceneText;

        [ObservableProperty]
        private SceneType selectedSceneType;

        [ObservableProperty]
        private string pictureFileName;

        [ObservableProperty]
        private EnemiesCollection enemies;

        [ObservableProperty]
        private ShopsCollection shops;

        [ObservableProperty]
        private ChoicesCollection choices;

        [ObservableProperty]
        private List<SceneType> sceneTypes;

        [ObservableProperty]
        private Enemy? selectedEnemy;

        [ObservableProperty]
        private Shop? selectedShop;

        [ObservableProperty]
        private ScenesCollection availableScenesForCombatTargets;

        [ObservableProperty]
        private Scene? selectedVictoryTargetScene;

        [ObservableProperty]
        private Scene? selectedDefeatTargetScene;

        [ObservableProperty]
        private Scene? selectedFleeTargetScene;

        [ObservableProperty]
        private bool canEditSceneChoices;

        public bool IsNormalScene
        {
            get
            {
                return SelectedSceneType == SceneType.Normal;
            }
        }

        public bool IsCombatScene
        {
            get
            {
                return SelectedSceneType == SceneType.Combat;
            }
        }

        public bool IsShopScene
        {
            get
            {
                return SelectedSceneType == SceneType.Shop;
            }
        }

        public bool IsEndScene
        {
            get
            {
                return SelectedSceneType == SceneType.End;
            }
        }

        public bool AreChoicesVisible
        {
            get
            {
                return CanEditSceneChoices && (SelectedSceneType == SceneType.Normal || SelectedSceneType == SceneType.Shop);
            }
        }

        public bool IsEnemySelectionVisible
        {
            get
            {
                return SelectedSceneType == SceneType.Combat;
            }
        }

        public bool IsShopSelectionVisible
        {
            get
            {
                return SelectedSceneType == SceneType.Shop;
            }
        }
        private string _selectedImageFileName;
        public string SelectedImageFileName
        {
            get => _selectedImageFileName;
            set
            {
                _selectedImageFileName = value;
                OnPropertyChanged();
            }
        }

        private ImageSource? _sceneImagePreview;
        public ImageSource? SceneImagePreview
        {
            get => _sceneImagePreview;
            set
            {
                _sceneImagePreview = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSceneImagePreview));
                OnPropertyChanged(nameof(HasNoSceneImagePreview));
            }
        }



        public bool HasSceneImagePreview => SceneImagePreview != null;

        public bool HasNoSceneImagePreview => SceneImagePreview == null;
        private string GetScenesImagesDirectoryPath()
        {
            if (string.IsNullOrWhiteSpace(DataFile.FilesPathDir))
            {
                throw new InvalidOperationException("Le dossier de données n'est pas configuré.");
            }

            string? jsonDirectoryPath = DataFile.FilesPathDir;

            DirectoryInfo? jsonDirectory = new DirectoryInfo(jsonDirectoryPath);
            DirectoryInfo? datasDirectory = jsonDirectory.Parent;

            if (datasDirectory == null)
            {
                throw new InvalidOperationException("Impossible de retrouver le dossier Datas.");
            }

            string imagesDirectoryPath = Path.Combine(datasDirectory.FullName, "Images", "Scenes");
            return imagesDirectoryPath;
        }

        private string BuildSafeImageFileName(string originalFileName)
        {
            string extension = Path.GetExtension(originalFileName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            string safeTitle = BuildSafeFileNameBase(SceneTitle);

            if (string.IsNullOrWhiteSpace(safeTitle))
            {
                safeTitle = "scene";
            }

            string finalFileName = safeTitle + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension;
            return finalFileName;
        }

        private string BuildSafeFileNameBase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "scene";
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                char currentChar = value[i];

                if (char.IsLetterOrDigit(currentChar))
                {
                    builder.Append(currentChar);
                }
                else if (currentChar == ' ' || currentChar == '-' || currentChar == '_')
                {
                    builder.Append('_');
                }
            }

            string result = builder.ToString().Trim('_');

            if (string.IsNullOrWhiteSpace(result))
            {
                return "scene";
            }

            return result;
        }

        private void UpdateImagePreview(string fullImagePath)
        {
            if (File.Exists(fullImagePath))
            {
                SceneImagePreview = ImageSource.FromFile(fullImagePath);
            }
            else
            {
                SceneImagePreview = null;
            }
        }
        partial void OnSelectedSceneTypeChanged(SceneType value)
        {
            RefreshSceneTypeVisibility();
        }

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task Save()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SceneTitle))
            {
                await alertService.ShowAlert("Titre invalide", "Le titre de la scène ne peut pas être vide.");
                return;
            }

            if (SceneTitle.Trim().Length < 3)
            {
                await alertService.ShowAlert("Titre invalide", "Le titre de la scène doit contenir au moins 3 caractères.");
                return;
            }

            if (SceneTitle.Trim().Length > 200)
            {
                await alertService.ShowAlert("Titre invalide", "Le titre de la scène ne peut pas dépasser 200 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SceneText))
            {
                await alertService.ShowAlert("Texte invalide", "Le texte de la scène ne peut pas être vide.");
                return;
            }

            if (SceneText.Trim().Length < 10)
            {
                await alertService.ShowAlert("Texte invalide", "Le texte de la scène doit contenir au moins 10 caractères.");
                return;
            }

            if (SelectedSceneType == SceneType.Shop && SelectedShop == null)
            {
                await alertService.ShowAlert("Boutique manquante", "Veuillez sélectionner une boutique pour une scène de type Shop.");
                return;
            }

            if (SelectedSceneType == SceneType.Combat)
            {
                if (SelectedEnemy == null)
                {
                    await alertService.ShowAlert("Ennemi manquant", "Veuillez sélectionner un ennemi pour une scène de combat.");
                    return;
                }

                if (SelectedVictoryTargetScene == null)
                {
                    await alertService.ShowAlert("Scène manquante", "Veuillez sélectionner une scène en cas de victoire.");
                    return;
                }

                if (SelectedDefeatTargetScene == null)
                {
                    await alertService.ShowAlert("Scène manquante", "Veuillez sélectionner une scène en cas de défaite.");
                    return;
                }

                if (SelectedFleeTargetScene == null)
                {
                    await alertService.ShowAlert("Scène manquante", "Veuillez sélectionner une scène en cas de fuite.");
                    return;
                }
            }

            try
            {
                if (selectedScene == null)
                {
                    Scene scene = new Scene(
                        SceneTitle.Trim(),
                        SceneText.Trim(),
                        SelectedSceneType
                    );

                    scene.AssignToScenario(selectedScenario.Id);

                    ApplySceneDetails(scene);

                    dataAccess.AddScene(scene);

                    await alertService.ShowAlert("Scène sauvegardée", "La nouvelle scène a bien été créée.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedScene.Rename(SceneTitle.Trim());
                selectedScene.UpdateText(SceneText.Trim());
                selectedScene.ChangeType(SelectedSceneType);

                ApplySceneDetails(selectedScene);

                dataAccess.UpdateScene(selectedScene);

                await alertService.ShowAlert("Scène sauvegardée", "La scène a bien été mise à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
        }

        [RelayCommand]
        private async Task NewChoice()
        {
            if (selectedScenario == null || selectedScene == null)
            {
                await alertService.ShowAlert("Contexte manquant", "Impossible de créer un choix car la scène courante n'est pas connue.");
                return;
            }

            if (choiceEditorPage.BindingContext is ChoiceEditorViewModel choiceEditorViewModel)
            {
                choiceEditorViewModel.PrepareNewChoice(selectedScenario, selectedScene);
            }

            await Shell.Current.Navigation.PushAsync(choiceEditorPage);
        }

        [RelayCommand]
        private async Task EditChoice(Choice choice)
        {
            if (choice == null)
            {
                return;
            }

            if (selectedScenario == null || selectedScene == null)
            {
                await alertService.ShowAlert("Contexte manquant", "Impossible de modifier ce choix car la scène courante n'est pas connue.");
                return;
            }

            if (choiceEditorPage.BindingContext is ChoiceEditorViewModel choiceEditorViewModel)
            {
                choiceEditorViewModel.LoadChoice(selectedScenario, selectedScene, choice);
            }

            await Shell.Current.Navigation.PushAsync(choiceEditorPage);
        }

        [RelayCommand]
        private async Task DeleteChoice(Choice choice)
        {
            if (selectedScene == null)
            {
                await alertService.ShowAlert("Scène manquante", "Impossible de supprimer ce choix car la scène courante n'est pas connue.");
                return;
            }

            if (choice == null)
            {
                return;
            }

            bool confirm = await alertService.ShowConfirmation("Supprimer choix", $"Voulez-vous vraiment supprimer le choix \"{choice.Label}\" ?", "Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteChoice(choice.Id);

                bool removed = selectedScene.Choices.RemoveById(choice.Id);

                if (!removed)
                {
                    await alertService.ShowAlert("Suppression impossible", "Le choix a été supprimé en base, mais pas trouvé dans la scène chargée.");
                    return;
                }

                

                await alertService.ShowAlert("Choix supprimé", "Le choix a bien été supprimé.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
        }


        [RelayCommand]
        private async Task BrowseImage()
        {
            try
            {
                PickOptions pickOptions = new PickOptions
                {
                    PickerTitle = "Choisir une image",
                    FileTypes = FilePickerFileType.Images
                };

                FileResult? fileResult = await FilePicker.Default.PickAsync(pickOptions);

                if (fileResult == null)
                {
                    return;
                }

                string selectedFileName = fileResult.FileName;

                if (!ValidUtils.CheckFileFormat(selectedFileName, Scene.ALLOWED_PICTURE_FILE_FORMATS) &&
                    !selectedFileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    await alertService.ShowAlert("Image invalide", "Veuillez sélectionner une image au format .jpg, .jpeg ou .png.");
                    return;
                }

                string imagesDirectoryPath = GetScenesImagesDirectoryPath();

                if (!Directory.Exists(imagesDirectoryPath))
                {
                    Directory.CreateDirectory(imagesDirectoryPath);
                }

                string newFileName = BuildSafeImageFileName(selectedFileName);
                string destinationPath = Path.Combine(imagesDirectoryPath, newFileName);

                using Stream sourceStream = await fileResult.OpenReadAsync();
                using FileStream destinationStream = File.Create(destinationPath);

                await sourceStream.CopyToAsync(destinationStream);

                PictureFileName = newFileName;
                SelectedImageFileName = newFileName;

                UpdateImagePreview(destinationPath);
            }
            catch (Exception ex)
            {
                await alertService.ShowAlert("Erreur", "Impossible de sélectionner l'image : " + ex.Message);
            }
        }

        public void PrepareNewScene(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedScene = null;

            PageTitle = "Nouvelle scène";

            SceneTitle = "Nouvelle scène";
            SceneText = string.Empty;
            SelectedSceneType = SceneType.Normal;
            PictureFileName = string.Empty;

            Enemies = scenario.Enemies;
            Shops = scenario.Shops;
            Choices = new ChoicesCollection();

            AvailableScenesForCombatTargets = BuildAvailableTargetScenes(null);

            SelectedEnemy = null;
            SelectedShop = null;
            SelectedVictoryTargetScene = null;
            SelectedDefeatTargetScene = null;
            SelectedFleeTargetScene = null;

            SelectedImageFileName = "Aucune image sélectionnée";
            SceneImagePreview = null;
            CanEditSceneChoices = false;

            RefreshSceneTypeVisibility();
        }

        public void LoadScene(Scenario scenario, Scene scene)
        {
            if (scenario == null || scene == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedScene = scene;

            PageTitle = "Édition scène";

            SceneTitle = scene.Title;
            SceneText = scene.Text;
            SelectedSceneType = scene.Type;

            PictureFileName = scene.PictureFileName ?? string.Empty;
            SelectedImageFileName = string.IsNullOrWhiteSpace(scene.PictureFileName)
                ? "Aucune image sélectionnée"
                : scene.PictureFileName;

            Enemies = scenario.Enemies;
            Shops = scenario.Shops;
            Choices = scene.Choices;

            AvailableScenesForCombatTargets = BuildAvailableTargetScenes(scene);

            SelectedEnemy = GetEnemyById(scene.EnemyId);
            SelectedShop = GetShopById(scene.ShopId);
            SelectedVictoryTargetScene = GetSceneById(scene.VictoryTargetSceneId);
            SelectedDefeatTargetScene = GetSceneById(scene.DefeatTargetSceneId);
            SelectedFleeTargetScene = GetSceneById(scene.FleeTargetSceneId);

            LoadSceneImagePreview(scene.PictureFileName);

            CanEditSceneChoices = true;

            RefreshSceneTypeVisibility();
        }

        private ScenesCollection BuildAvailableTargetScenes(Scene? sceneToExclude)
        {
            ScenesCollection availableScenes = new ScenesCollection();

            if (selectedScenario == null)
            {
                return availableScenes;
            }

            for (int i = 0; i < selectedScenario.Scenes.Count; i++)
            {
                Scene scene = selectedScenario.Scenes[i];

                if (sceneToExclude != null && scene.Id == sceneToExclude.Id)
                {
                    continue;
                }

                availableScenes.Add(scene);
            }

            return availableScenes;
        }

        private Enemy? GetEnemyById(int? enemyId)
        {
            if (enemyId == null)
            {
                return null;
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemy enemy = Enemies[i];

                if (enemy.Id == enemyId.Value)
                {
                    return enemy;
                }
            }

            return null;
        }

        private Shop? GetShopById(int? shopId)
        {
            if (shopId == null)
            {
                return null;
            }

            for (int i = 0; i < Shops.Count; i++)
            {
                Shop shop = Shops[i];

                if (shop.Id == shopId.Value)
                {
                    return shop;
                }
            }

            return null;
        }

        private Scene? GetSceneById(int? sceneId)
        {
            if (sceneId == null || selectedScenario == null)
            {
                return null;
            }

            for (int i = 0; i < selectedScenario.Scenes.Count; i++)
            {
                Scene scene = selectedScenario.Scenes[i];

                if (scene.Id == sceneId.Value)
                {
                    return scene;
                }
            }

            return null;
        }

        private void LoadSceneImagePreview(string? pictureFileName)
        {
            if (string.IsNullOrWhiteSpace(pictureFileName))
            {
                SceneImagePreview = null;
                return;
            }

            try
            {
                string imagesDirectoryPath = GetScenesImagesDirectoryPath();
                string fullImagePath = Path.Combine(imagesDirectoryPath, pictureFileName);

                UpdateImagePreview(fullImagePath);
            }
            catch
            {
                SceneImagePreview = null;
            }
        }

        private void RefreshSceneTypeVisibility()
        {
            OnPropertyChanged(nameof(IsNormalScene));
            OnPropertyChanged(nameof(IsCombatScene));
            OnPropertyChanged(nameof(IsShopScene));
            OnPropertyChanged(nameof(IsEndScene));
            OnPropertyChanged(nameof(AreChoicesVisible));
            OnPropertyChanged(nameof(IsEnemySelectionVisible));
            OnPropertyChanged(nameof(IsShopSelectionVisible));
        }

        private void ApplySceneDetails(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            if (string.IsNullOrWhiteSpace(PictureFileName))
            {
                scene.ClearPicture();
            }
            else
            {
                scene.SetPicture(PictureFileName);
            }

            if (SelectedSceneType == SceneType.Normal)
            {
                scene.ChangeType(SceneType.Normal);
                scene.ClearShop();
                scene.ClearCombat();
                return;
            }

            if (SelectedSceneType == SceneType.Shop)
            {
                if (SelectedShop == null)
                {
                    throw new InvalidOperationException("Une scène de type Shop doit avoir une boutique.");
                }

                scene.ClearCombat();
                scene.ChangeType(SceneType.Shop);
                scene.SetShop(SelectedShop.Id);
                return;
            }

            if (SelectedSceneType == SceneType.Combat)
            {
                if (SelectedEnemy == null)
                {
                    throw new InvalidOperationException("Une scène de combat doit avoir un ennemi.");
                }

                if (SelectedVictoryTargetScene == null)
                {
                    throw new InvalidOperationException("Une scène de combat doit avoir une scène de victoire.");
                }

                if (SelectedDefeatTargetScene == null)
                {
                    throw new InvalidOperationException("Une scène de combat doit avoir une scène de défaite.");
                }

                if (SelectedFleeTargetScene == null)
                {
                    throw new InvalidOperationException("Une scène de combat doit avoir une scène de fuite.");
                }

                scene.ClearShop();
                scene.ChangeType(SceneType.Combat);

                scene.SetCombat( SelectedEnemy.Id, SelectedFleeTargetScene.Id, SelectedDefeatTargetScene.Id,SelectedVictoryTargetScene.Id );

                return;
            }

            if (SelectedSceneType == SceneType.End)
            {
                scene.ClearShop();
                scene.ClearCombat();
                scene.ChangeType(SceneType.End);
                return;
            }
        }

        public void RefreshLoadedScene()
        {
            if (selectedScene == null)
            {
                return;
            }

            int sceneId = selectedScene.Id;

            string currentTitle = SceneTitle;
            string currentText = SceneText;
            string currentPictureFileName = PictureFileName;
            string currentSelectedImageFileName = SelectedImageFileName;

            SceneType currentSceneType = SelectedSceneType;

            int? currentShopId = SelectedShop == null ? null : SelectedShop.Id;
            int? currentEnemyId = SelectedEnemy == null ? null : SelectedEnemy.Id;
            int? currentFleeTargetSceneId = SelectedFleeTargetScene == null ? null : SelectedFleeTargetScene.Id;
            int? currentDefeatTargetSceneId = SelectedDefeatTargetScene == null ? null : SelectedDefeatTargetScene.Id;
            int? currentVictoryTargetSceneId = SelectedVictoryTargetScene == null ? null : SelectedVictoryTargetScene.Id;

            Scene? refreshedScene = dataAccess.GetSceneById(sceneId);

            if (refreshedScene == null)
            {
                return;
            }

            if (selectedScenario == null)
            {
                return;
            }

            Scenario? refreshedScenario = dataAccess.GetScenarioById(selectedScenario.Id);

            if (refreshedScenario == null)
            {
                return;
            }

            LoadScene(refreshedScenario, refreshedScene);

            SceneTitle = currentTitle;
            SceneText = currentText;
            PictureFileName = currentPictureFileName;
            SelectedImageFileName = currentSelectedImageFileName;

            SelectedSceneType = currentSceneType;

            SelectedShop = GetShopById(currentShopId);
            SelectedEnemy = GetEnemyById(currentEnemyId);
            SelectedFleeTargetScene = GetSceneById(currentFleeTargetSceneId);
            SelectedDefeatTargetScene = GetSceneById(currentDefeatTargetSceneId);
            SelectedVictoryTargetScene = GetSceneById(currentVictoryTargetSceneId);

            RefreshSceneTypeVisibility();
        }
    }
}
