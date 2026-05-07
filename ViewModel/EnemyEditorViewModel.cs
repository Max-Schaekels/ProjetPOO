using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Utilities.Interfaces;
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

            enemyTypes = new List<EnemyType>
            {
                EnemyType.Slime,
                EnemyType.Goblin,
                EnemyType.Orc,
                EnemyType.Dragon
            };

            enemyName = string.Empty;
            selectedEnemyType = EnemyType.Slime;

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
        }

        [ObservableProperty]
        private string enemyName;

        [ObservableProperty]
        private List<EnemyType> enemyTypes;

        [ObservableProperty]
        private EnemyType selectedEnemyType;

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

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();   
        }

        [RelayCommand]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder ennem", "La sauvegarde de l'ennemi sera ajoutée plus tard.");
        }
    }
}
