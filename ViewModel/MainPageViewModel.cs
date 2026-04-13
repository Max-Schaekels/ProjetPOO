using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(IAlertService alertService, IDataAccess dataAccess) : base(alertService, dataAccess)
        {
            PageTitle = "Accueil";
        }

        [RelayCommand]
        private async Task Editer()
        {
            await alertService.ShowAlert("Édition", "La partie édition sera ajoutée plus tard.");
        }

        [RelayCommand]
        private async Task Jouer()
        {
            await alertService.ShowAlert("Jeu", "La partie jeu sera ajoutée plus tard.");
        }

        [RelayCommand]
        private void Quitter()
        {
            Application.Current?.Quit();
        }
    }
}
