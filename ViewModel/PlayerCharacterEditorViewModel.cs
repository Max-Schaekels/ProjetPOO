using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class PlayerCharacterEditorViewModel : BaseViewModel
    {
        public PlayerCharacterEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition personnage joueur";
            playerName = string.Empty;
            maxHp = 10;
            attack = 5;
            defense = 2;
            agility = 2;
            startingExperience = 0;
            startingLevel = 1;
            className = string.Empty;
            raceName = string.Empty;
        }

        [ObservableProperty]
        private string playerName;

        [ObservableProperty]
        private int maxHp;

        [ObservableProperty]
        private int attack;

        [ObservableProperty]
        private int defense;

        [ObservableProperty]
        private int agility;

        [ObservableProperty]
        private int startingExperience;

        [ObservableProperty]
        private int startingLevel;

        [ObservableProperty]
        private string className;

        [ObservableProperty]
        private string raceName;

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder personnage", "La sauvegarde du personnage sera ajoutée plus tard.");
        }
    }
}
