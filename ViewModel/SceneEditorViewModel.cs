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
            OnPropertyChanged(nameof(IsNormalScene));
            OnPropertyChanged(nameof(IsCombatScene));
            OnPropertyChanged(nameof(IsShopScene));
            OnPropertyChanged(nameof(IsEndScene));
            OnPropertyChanged(nameof(AreChoicesVisible));
            OnPropertyChanged(nameof(IsEnemySelectionVisible));
            OnPropertyChanged(nameof(IsShopSelectionVisible));
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
        private async Task DeleteChoice(Choice choice)
        {
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


    }
}
