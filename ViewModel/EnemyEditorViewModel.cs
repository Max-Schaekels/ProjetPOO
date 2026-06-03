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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (SelectedEnemyRace == null)
            {
                await alertService.ShowAlert("Race manquante", "Veuillez sélectionner ou créer une race d'ennemi.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(EnemyName) && EnemyName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de l'ennemi doit contenir au moins 3 caractères s'il est renseigné.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(EnemyName) && EnemyName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de l'ennemi ne peut pas dépasser 50 caractères.");
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

            if (RewardExperience < 0)
            {
                await alertService.ShowAlert("Expérience invalide", "L'expérience donnée ne peut pas être négative.");
                return;
            }

            if (RewardGoldMin < 0 || RewardGoldMax < 0)
            {
                await alertService.ShowAlert("Or invalide", "Les récompenses en or ne peuvent pas être négatives.");
                return;
            }

            if (RewardGoldMin > RewardGoldMax)
            {
                await alertService.ShowAlert("Or invalide", "L'or minimum ne peut pas être supérieur à l'or maximum.");
                return;
            }

            if (PotionDropChance < 0 || PotionDropChance > 100)
            {
                await alertService.ShowAlert("Chance invalide", "La chance de drop de potion doit être comprise entre 0 et 100.");
                return;
            }

            if (PotionAmountMin < 0 || PotionAmountMax < 0)
            {
                await alertService.ShowAlert("Potion invalide", "Les quantités de potions ne peuvent pas être négatives.");
                return;
            }

            if (PotionAmountMin > PotionAmountMax)
            {
                await alertService.ShowAlert("Potion invalide", "La quantité minimum de potions ne peut pas être supérieure au maximum.");
                return;
            }

            if (PotionDropChance > 0 && PotionAmountMax == 0)
            {
                await alertService.ShowAlert("Potion invalide", "Si une potion peut être obtenue, la quantité maximum doit être supérieure à 0.");
                return;
            }

            if (KeyDropChance < 0 || KeyDropChance > 100)
            {
                await alertService.ShowAlert("Chance invalide", "La chance de drop de clé doit être comprise entre 0 et 100.");
                return;
            }

            if (KeyAmountMin < 0 || KeyAmountMax < 0)
            {
                await alertService.ShowAlert("Clé invalide", "Les quantités de clés ne peuvent pas être négatives.");
                return;
            }

            if (KeyAmountMin > KeyAmountMax)
            {
                await alertService.ShowAlert("Clé invalide", "La quantité minimum de clés ne peut pas être supérieure au maximum.");
                return;
            }

            if (KeyDropChance > 0 && KeyAmountMax == 0)
            {
                await alertService.ShowAlert("Clé invalide", "Si une clé peut être obtenue, la quantité maximum doit être supérieure à 0.");
                return;
            }

            try
            {
                string? normalizedEnemyName = null;

                if (!string.IsNullOrWhiteSpace(EnemyName))
                {
                    normalizedEnemyName = EnemyName.Trim();
                }

                if (selectedEnemy == null)
                {
                    Enemy enemy = new Enemy(normalizedEnemyName, SelectedEnemyRace.Id, MaxHp, Attack, Defense, Agility, RewardExperience, RewardGoldMin, RewardGoldMax, PotionDropChance, PotionAmountMin, PotionAmountMax, KeyDropChance, KeyAmountMin, KeyAmountMax );

                    enemy.AssignToScenario(selectedScenario.Id);

                    dataAccess.AddEnemy(enemy);

                    await alertService.ShowAlert("Ennemi sauvegardé", "Le nouvel ennemi a bien été créé.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedEnemy.RenameEnemy(normalizedEnemyName);
                selectedEnemy.ChangeEnemyRace(SelectedEnemyRace.Id);
                selectedEnemy.UpdateStats(MaxHp, Attack, Defense, Agility);
                selectedEnemy.UpdateRewards(RewardExperience, RewardGoldMin, RewardGoldMax);
                selectedEnemy.UpdatePotionLoot(PotionDropChance, PotionAmountMin, PotionAmountMax);
                selectedEnemy.UpdateKeyLoot(KeyDropChance, KeyAmountMin, KeyAmountMax);

                dataAccess.UpdateEnemy(selectedEnemy);

                await alertService.ShowAlert("Ennemi sauvegardé", "L'ennemi a bien été mis à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
        }

        public async Task<bool> SaveNewEnemyRace()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewEnemyRaceName))
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la race ne peut pas être vide.");
                return false;
            }

            if (EnemyRaces != null && EnemyRaces.ContainsName(NewEnemyRaceName.Trim()))
            {
                await alertService.ShowAlert("Race déjà existante", "Une race avec ce nom existe déjà dans ce scénario.");
                return false;
            }

            try
            {
                EnemyRace enemyRace = new EnemyRace(NewEnemyRaceName.Trim(),NewEnemyRaceDescription.Trim());

                enemyRace.AssignToScenario(selectedScenario.Id);

                dataAccess.AddEnemyRace(enemyRace);

                EnemyRaces = dataAccess.GetEnemyRacesByScenarioId(selectedScenario.Id);

                SelectedEnemyRace = GetEnemyRaceByName(NewEnemyRaceName.Trim());

                NewEnemyRaceName = string.Empty;
                NewEnemyRaceDescription = string.Empty;

                return true;
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur", exception.Message);
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

        private EnemyRace? GetEnemyRaceByName(string enemyRaceName)
        {
            if (EnemyRaces == null)
            {
                return null;
            }

            for (int i = 0; i < EnemyRaces.Count; i++)
            {
                EnemyRace enemyRace = EnemyRaces[i];

                if (enemyRace.Name.Equals(enemyRaceName, StringComparison.OrdinalIgnoreCase))
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
