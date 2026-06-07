using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class PlayerCharacterSelectionViewModel : BaseViewModel
    {
        private Scenario? selectedScenario;
        public PlayerCharacterSelectionViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Choisir un personnage";

            playerCharacters = new ObservableCollection<PlayerCharacterTemplate>();
        }

        [ObservableProperty]
        private ObservableCollection<PlayerCharacterTemplate> playerCharacters;

        [ObservableProperty]
        private PlayerCharacterTemplate? selectedPlayerCharacter;

        public void LoadScenario(Scenario scenario)
        {
            selectedScenario = scenario;

            PageTitle = $"Personnage - {scenario.Title}";

            PlayerCharacters = new ObservableCollection<PlayerCharacterTemplate>(scenario.PlayerCharacters);

            SelectedPlayerCharacter = null;
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task StartGame(PlayerCharacterTemplate playerCharacter)
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (playerCharacter == null)
            {
                await alertService.ShowAlert("Personnage manquant", "Veuillez sélectionner un personnage.");
                return;
            }

            await alertService.ShowAlert("Démarrage", $"Le personnage \"{playerCharacter.Name}\" a été sélectionné. Prochaine étape : création de l'état de partie.");
        }
    }
}
