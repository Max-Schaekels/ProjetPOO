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
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly ScenarioListPage scenarioListPage;
        public MainPageViewModel(IAlertService alertService, IDataAccess dataAccess, ScenarioListPage scenarioListPage) : base(alertService, dataAccess)
        {
            this.scenarioListPage = scenarioListPage;
            PageTitle = "Accueil";
        }

        [RelayCommand()]
        private async Task Editer()
        {
            await Shell.Current.Navigation.PushAsync(scenarioListPage);
        }

        [RelayCommand()]
        private async Task Jouer()
        {
            await alertService.ShowAlert("Jeu", "La partie jeu sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private void Quitter()
        {
            Application.Current?.Quit();
        }
    }
}
