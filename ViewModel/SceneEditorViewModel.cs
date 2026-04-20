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


namespace ProjetPOO.ViewModel
{
    public partial class SceneEditorViewModel : BaseViewModel
    {
        
        public SceneEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
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
        private async Task AddChoice()
        {
            await alertService.ShowAlert("Ajouter choix", "L'ajout d'un choix sera ajouté plus tard.");
        }

        [RelayCommand]
        private async Task DeleteChoice(Choice choice)
        {
            await alertService.ShowAlert("Supprimer choix", $"La suppression du choix '{choice.Label}' sera ajoutée plus tard.");
        }


        [RelayCommand]
        private async Task BrowseImage()
        {
            await alertService.ShowAlert("Image", "La sélection d'image sera implémentée plus tard.");
        }


    }
}
