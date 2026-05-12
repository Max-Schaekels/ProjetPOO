using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class EnemyEditorViewModel : BaseViewModel
    {
        public EnemyEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition ennemi";

            enemyName = string.Empty;

            enemyRaces = dataAccessService.GetAllEnemyRaces();
            selectedEnemyRace = null;

            if (enemyRaces != null && enemyRaces.Count > 0)
            {
                selectedEnemyRace = enemyRaces[0];
            }

            maxHp = 10;
            attack = 1;
            defense = 0;
            agility = 0;

            rewardExperience = 0;
            rewardGoldMin = 0;
            rewardGoldMax = 0;

            potionDropChance = 0;
            potionAmountMin = 0;
            potionAmountMax = 0;

            keyDropChance = 0;
            keyAmountMin = 0;
            keyAmountMax = 0;

            newEnemyRaceName = string.Empty;
            newEnemyRaceDescription = string.Empty;
        }

        [ObservableProperty]
        private string enemyName;

        [ObservableProperty]
        private EnemyRacesCollection enemyRaces;

        [ObservableProperty]
        private EnemyRace? selectedEnemyRace;

        [ObservableProperty]
        private int maxHp;

        [ObservableProperty]
        private int attack;

        [ObservableProperty]
        private int defense;

        [ObservableProperty]
        private int agility;

        [ObservableProperty]
        private int rewardExperience;

        [ObservableProperty]
        private int rewardGoldMin;

        [ObservableProperty]
        private int rewardGoldMax;

        [ObservableProperty]
        private int potionDropChance;

        [ObservableProperty]
        private int potionAmountMin;

        [ObservableProperty]
        private int potionAmountMax;

        [ObservableProperty]
        private int keyDropChance;

        [ObservableProperty]
        private int keyAmountMin;

        [ObservableProperty]
        private int keyAmountMax;

        [ObservableProperty]
        private string newEnemyRaceName;

        [ObservableProperty]
        private string newEnemyRaceDescription;

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();   
        }

        [RelayCommand]
        private void NewEnemyRace()
        {
            NewEnemyRaceName = string.Empty;
            NewEnemyRaceDescription = string.Empty;

            EnemyRacePopup popup = new EnemyRacePopup(this);
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        [RelayCommand]
        private async Task Save()
        {
            if (SelectedEnemyRace == null)
            {
                await alertService.ShowAlert("Race manquante", "Veuillez sélectionner ou créer une race d'ennemi.");
                return;
            }

            await alertService.ShowAlert("Sauvegarder ennemi", "La sauvegarde de l'ennemi sera ajoutée plus tard.");
        }

        public async Task<bool> SaveNewEnemyRace()
        {
            try
            {
                EnemyRace enemyRace = new EnemyRace(NewEnemyRaceName, NewEnemyRaceDescription);

                if (EnemyRaces == null)
                {
                    EnemyRaces = new EnemyRacesCollection();
                }

                EnemyRaces.Add(enemyRace);
                SelectedEnemyRace = enemyRace;

                NewEnemyRaceName = string.Empty;
                NewEnemyRaceDescription = string.Empty;

                return true;
            }
            catch (Exception ex)
            {
                await alertService.ShowAlert("Erreur", ex.Message);
                return false;
            }
        }
    }
}
