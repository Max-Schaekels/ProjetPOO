using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Story;
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
        private Scenario? selectedScenario;
        private PlayerCharacterTemplate? selectedPlayerCharacter;
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

        public void PrepareNewPlayerCharacter(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedPlayerCharacter = null;

            PageTitle = "Nouveau personnage";

            PlayerName = string.Empty;
            ClassName = string.Empty;
            RaceName = string.Empty;

            MaxHp = 10;
            Attack = 5;
            Defense = 2;
            Agility = 2;

            StartingExperience = 0;
            StartingLevel = 1;
        }

        public void LoadPlayerCharacter(Scenario scenario, PlayerCharacterTemplate playerCharacter)
        {
            if (scenario == null || playerCharacter == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedPlayerCharacter = playerCharacter;

            PageTitle = "Édition personnage";

            PlayerName = playerCharacter.Name;
            ClassName = playerCharacter.ClassName;
            RaceName = playerCharacter.RaceName;

            MaxHp = playerCharacter.MaxHp;
            Attack = playerCharacter.Attack;
            Defense = playerCharacter.Defense;
            Agility = playerCharacter.Agility;

            StartingExperience = playerCharacter.StartingExperience;
            StartingLevel = playerCharacter.StartingLevel;
        }
    }
}
