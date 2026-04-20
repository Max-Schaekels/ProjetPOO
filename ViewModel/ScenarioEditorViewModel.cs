using CommunityToolkit.Mvvm.Input;
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
        public ScenarioEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, SceneEditorPage sceneEditorPage) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition scénario";
            this.sceneEditorPage = sceneEditorPage;
        }

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

    }
}
