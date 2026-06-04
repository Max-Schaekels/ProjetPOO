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
        private EnemyRace? selectedEnemyRaceForEdit;

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

            enemyRacePopupTitle = "Nouvelle race ennemie";
            enemyRacePopupSaveButtonText = "Créer";
            canManageSelectedEnemyRace = selectedEnemyRace != null;
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

        [ObservableProperty]
        private string enemyRacePopupTitle;

        [ObservableProperty]
        private string enemyRacePopupSaveButtonText;

        [ObservableProperty]
        private bool canManageSelectedEnemyRace;

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();   
        }

        [RelayCommand]
        private void NewEnemyRace()
        {
            selectedEnemyRaceForEdit = null;

            EnemyRacePopupTitle = "Nouvelle race ennemie";
            EnemyRacePopupSaveButtonText = "Créer";

            NewEnemyRaceName = string.Empty;
            NewEnemyRaceDescription = string.Empty;

            EnemyRacePopup popup = new EnemyRacePopup(this);
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        [RelayCommand]
        private async Task EditEnemyRace()
        {
            if (SelectedEnemyRace == null)
            {
                await alertService.ShowAlert("Race manquante", "Veuillez sélectionner une race à modifier.");
                return;
            }

            selectedEnemyRaceForEdit = SelectedEnemyRace;

            EnemyRacePopupTitle = "Modifier race ennemie";
            EnemyRacePopupSaveButtonText = "Modifier";

            NewEnemyRaceName = SelectedEnemyRace.Name;
            NewEnemyRaceDescription = SelectedEnemyRace.Description;

            EnemyRacePopup popup = new EnemyRacePopup(this);
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        [RelayCommand]
        private async Task DeleteEnemyRace()
        {
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (SelectedEnemyRace == null)
            {
                await alertService.ShowAlert("Race manquante", "Veuillez sélectionner une race à supprimer.");
                return;
            }

            if (IsEnemyRaceUsed(SelectedEnemyRace.Id))
            {
                await alertService.ShowAlert("Suppression impossible", "Cette race est utilisée par au moins un ennemi. Supprimez ou modifiez d'abord les ennemis concernés.");
                return;
            }

            bool confirm = await alertService.ShowConfirmation( "Supprimer race",$"Voulez-vous vraiment supprimer la race \"{SelectedEnemyRace.Name}\" ?", "Supprimer", "Annuler");

            if (!confirm)
            {
                return;
            }

            try
            {
                dataAccess.DeleteEnemyRace(SelectedEnemyRace.Id);

                EnemyRaces = dataAccess.GetEnemyRacesByScenarioId(selectedScenario.Id);
                SelectedEnemyRace = GetFirstEnemyRace();

                await alertService.ShowAlert("Race supprimée", "La race a bien été supprimée.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur suppression", exception.Message);
            }
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

        public async Task<bool> SaveEnemyRace()
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

            if (NewEnemyRaceName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la race doit contenir au moins 3 caractères.");
                return false;
            }

            if (NewEnemyRaceName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la race ne peut pas dépasser 50 caractères.");
                return false;
            }

            if (EnemyRaceNameExistsForOtherRace(NewEnemyRaceName.Trim()))
            {
                await alertService.ShowAlert("Race déjà existante", "Une autre race avec ce nom existe déjà dans ce scénario.");
                return false;
            }

            try
            {
                string normalizedName = NewEnemyRaceName.Trim();
                string normalizedDescription = NewEnemyRaceDescription.Trim();

                if (selectedEnemyRaceForEdit == null)
                {
                    EnemyRace enemyRace = new EnemyRace(normalizedName, normalizedDescription);

                    enemyRace.AssignToScenario(selectedScenario.Id);

                    dataAccess.AddEnemyRace(enemyRace);

                    EnemyRaces = dataAccess.GetEnemyRacesByScenarioId(selectedScenario.Id);
                    SelectedEnemyRace = GetEnemyRaceByName(normalizedName);
                }
                else
                {
                    int editedEnemyRaceId = selectedEnemyRaceForEdit.Id;

                    selectedEnemyRaceForEdit.Rename(normalizedName);
                    selectedEnemyRaceForEdit.ChangeDescription(normalizedDescription);

                    dataAccess.UpdateEnemyRace(selectedEnemyRaceForEdit);

                    EnemyRaces = dataAccess.GetEnemyRacesByScenarioId(selectedScenario.Id);
                    SelectedEnemyRace = GetEnemyRaceById(editedEnemyRaceId);
                }

                selectedEnemyRaceForEdit = null;

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

        partial void OnSelectedEnemyRaceChanged(EnemyRace? value)
        {
            CanManageSelectedEnemyRace = value != null;
        }

        private bool EnemyRaceNameExistsForOtherRace(string enemyRaceName)
        {
            if (EnemyRaces == null)
            {
                return false;
            }

            for (int i = 0; i < EnemyRaces.Count; i++)
            {
                EnemyRace enemyRace = EnemyRaces[i];

                if (!enemyRace.Name.Equals(enemyRaceName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (selectedEnemyRaceForEdit == null)
                {
                    return true;
                }

                if (enemyRace.Id != selectedEnemyRaceForEdit.Id)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsEnemyRaceUsed(int enemyRaceId)
        {
            if (selectedScenario == null)
            {
                return false;
            }

            for (int i = 0; i < selectedScenario.Enemies.Count; i++)
            {
                Enemy enemy = selectedScenario.Enemies[i];

                if (enemy.EnemyRaceId == enemyRaceId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
