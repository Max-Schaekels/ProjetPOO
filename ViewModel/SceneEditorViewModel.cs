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
                return SelectedSceneType == SceneType.Normal;
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
            await alertService.ShowAlert("Sauvegarder scène", "La sauvegarde de la scène sera ajoutée plus tard.");
        }

        [RelayCommand]
        private async Task NewChoice()
        {
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
            if (choice == null)
            {
                return;
            }
            await alertService.ShowAlert("Supprimer choix", $"La suppression du choix '{choice.Label}' sera ajoutée plus tard.");
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


    }
}
