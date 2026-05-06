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

namespace ProjetPOO.ViewModel
{
    public partial class ScenarioEditorViewModel : BaseViewModel
    {
        private readonly SceneEditorPage sceneEditorPage;
        public ScenarioEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, SceneEditorPage sceneEditorPage) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition scénario";
            this.sceneEditorPage = sceneEditorPage;
            scenarioTitle = string.Empty;
            scenarioDescription = string.Empty;
            scenesCount = 0;
            enemiesCount = 0;
            shopsCount = 0;
            playerCharactersCount = 0;
            isScenesEmpty = true;
        }

        private Scenario? selectedScenario;

        [ObservableProperty]
        private string scenarioTitle;

        [ObservableProperty]
        private string scenarioDescription;

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
            await Shell.Current.Navigation.PushAsync(sceneEditorPage);
        }

        [RelayCommand()]
        private async Task AddEnemy()
        {
            await alertService.ShowAlert("Ajouter ennemi", "L'ajout d'ennemi sera ajouté plus tard.");
        }

        [RelayCommand()]
        private async Task AddShop()
        {
            await alertService.ShowAlert("Ajouter boutique", "L'ajout de boutique sera ajouté plus tard.");
        }

        [RelayCommand()]
        private async Task AddPlayerCharacter()
        {
            await alertService.ShowAlert("Ajouter personnage joueur", "L'ajout de personnage joueur sera ajouté plus tard.");
        }

        public void LoadScenario(Scenario scenario)
        {
            selectedScenario = scenario;

            PageTitle = "Édition scénario";

            ScenarioTitle = scenario.Title;
            ScenarioDescription = scenario.Description;

            ScenesCount = scenario.ScenesCount;
            EnemiesCount = scenario.EnemiesCount;
            ShopsCount = scenario.ShopsCount;
            PlayerCharactersCount = scenario.PlayerCharacters.Count;

            IsScenesEmpty = scenario.ScenesCount == 0;
        }

        public void PrepareNewScenario()
        {
            selectedScenario = null;

            PageTitle = "Nouveau scénario";

            ScenarioTitle = string.Empty;
            ScenarioDescription = string.Empty;

            ScenesCount = 0;
            EnemiesCount = 0;
            ShopsCount = 0;
            PlayerCharactersCount = 0;

            IsScenesEmpty = true;
        }

    }
}
