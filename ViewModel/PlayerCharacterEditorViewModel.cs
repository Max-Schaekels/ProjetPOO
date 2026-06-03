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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                await alertService.ShowAlert("Nom invalide", "Le nom du personnage ne peut pas être vide.");
                return;
            }

            if (PlayerName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom du personnage doit contenir au moins 3 caractères.");
                return;
            }

            if (PlayerName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom du personnage ne peut pas dépasser 50 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ClassName))
            {
                await alertService.ShowAlert("Classe invalide", "La classe du personnage ne peut pas être vide.");
                return;
            }

            if (ClassName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Classe invalide", "La classe du personnage doit contenir au moins 3 caractères.");
                return;
            }

            if (ClassName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Classe invalide", "La classe du personnage ne peut pas dépasser 50 caractères.");
                return;
            }

            if (string.IsNullOrWhiteSpace(RaceName))
            {
                await alertService.ShowAlert("Race invalide", "La race du personnage ne peut pas être vide.");
                return;
            }

            if (RaceName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Race invalide", "La race du personnage doit contenir au moins 3 caractères.");
                return;
            }

            if (RaceName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Race invalide", "La race du personnage ne peut pas dépasser 50 caractères.");
                return;
            }

            if (MaxHp <= 0)
            {
                await alertService.ShowAlert("PV invalides", "Les PV maximum doivent être supérieurs à 0.");
                return;
            }

            if (Attack < 0)
            {
                await alertService.ShowAlert("Attaque invalide", "L'attaque ne peut pas être négative.");
                return;
            }

            if (Defense < 0)
            {
                await alertService.ShowAlert("Défense invalide", "La défense ne peut pas être négative.");
                return;
            }

            if (Agility < 0)
            {
                await alertService.ShowAlert("Agilité invalide", "L'agilité ne peut pas être négative.");
                return;
            }

            if (StartingExperience < 0)
            {
                await alertService.ShowAlert("Expérience invalide", "L'expérience de départ ne peut pas être négative.");
                return;
            }

            if (StartingLevel <= 0)
            {
                await alertService.ShowAlert("Niveau invalide", "Le niveau de départ doit être supérieur à 0.");
                return;
            }

            try
            {
                if (selectedPlayerCharacter == null)
                {
                    PlayerCharacterTemplate playerCharacter = new PlayerCharacterTemplate(
                        PlayerName.Trim(),
                        ClassName.Trim(),
                        RaceName.Trim(),
                        MaxHp,
                        Attack,
                        Defense,
                        Agility,
                        StartingExperience,
                        StartingLevel
                    );

                    playerCharacter.SetScenario(selectedScenario.Id);

                    dataAccess.AddPlayerCharacterTemplate(playerCharacter);

                    await alertService.ShowAlert("Personnage sauvegardé", "Le nouveau personnage a bien été créé.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedPlayerCharacter.Rename(PlayerName.Trim());
                selectedPlayerCharacter.ChangeClassName(ClassName.Trim());
                selectedPlayerCharacter.ChangeRaceName(RaceName.Trim());
                selectedPlayerCharacter.UpdateStats(MaxHp, Attack, Defense, Agility);
                selectedPlayerCharacter.UpdateStartingProgression(StartingExperience, StartingLevel);

                dataAccess.UpdatePlayerCharacterTemplate(selectedPlayerCharacter);

                await alertService.ShowAlert("Personnage sauvegardé", "Le personnage a bien été mis à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
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
