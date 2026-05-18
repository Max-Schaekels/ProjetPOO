using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Story;
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
        private Scenario? selectedScenario;
        private Enemy? selectedEnemy;
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
                    if (selectedScenario != null)
                    {
                        EnemyRaces = new EnemyRacesCollection(selectedScenario.Id);
                    }
                    else
                    {
                        EnemyRaces = new EnemyRacesCollection();
                    }
                }

                EnemyRaces.AddEnemyRace(enemyRace);
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

        public void PrepareNewEnemy(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedEnemy = null;

            PageTitle = "Nouvel ennemi";

            EnemyName = string.Empty;

            EnemyRaces = scenario.EnemyRaces;
            SelectedEnemyRace = GetFirstEnemyRace();

            MaxHp = 10;
            Attack = 1;
            Defense = 0;
            Agility = 0;

            RewardExperience = 0;
            RewardGoldMin = 0;
            RewardGoldMax = 0;

            PotionDropChance = 0;
            PotionAmountMin = 0;
            PotionAmountMax = 0;

            KeyDropChance = 0;
            KeyAmountMin = 0;
            KeyAmountMax = 0;

            NewEnemyRaceName = string.Empty;
            NewEnemyRaceDescription = string.Empty;
        }

        public void LoadEnemy(Scenario scenario, Enemy enemy)
        {
            if (scenario == null || enemy == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedEnemy = enemy;

            PageTitle = "Édition ennemi";

            EnemyName = enemy.EnemyName ?? string.Empty;

            EnemyRaces = scenario.EnemyRaces;
            SelectedEnemyRace = GetEnemyRaceById(enemy.EnemyRaceId);

            MaxHp = enemy.MaxHp;
            Attack = enemy.Attack;
            Defense = enemy.Defense;
            Agility = enemy.Agility;

            RewardExperience = enemy.RewardExperience;
            RewardGoldMin = enemy.RewardGoldMin;
            RewardGoldMax = enemy.RewardGoldMax;

            PotionDropChance = enemy.PotionDropChance;
            PotionAmountMin = enemy.PotionAmountMin;
            PotionAmountMax = enemy.PotionAmountMax;

            KeyDropChance = enemy.KeyDropChance;
            KeyAmountMin = enemy.KeyAmountMin;
            KeyAmountMax = enemy.KeyAmountMax;

            NewEnemyRaceName = string.Empty;
            NewEnemyRaceDescription = string.Empty;
        }

        private EnemyRace? GetEnemyRaceById(int enemyRaceId)
        {
            if (EnemyRaces == null)
            {
                return null;
            }

            for (int i = 0; i < EnemyRaces.Count; i++)
            {
                EnemyRace enemyRace = EnemyRaces[i];

                if (enemyRace.Id == enemyRaceId)
                {
                    return enemyRace;
                }
            }

            return null;
        }

        private EnemyRace? GetFirstEnemyRace()
        {
            if (EnemyRaces == null || EnemyRaces.Count == 0)
            {
                return null;
            }

            return EnemyRaces[0];
        }
    }
}
