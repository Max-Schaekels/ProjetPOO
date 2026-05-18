using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.ViewModel
{
    public partial class EffectEditorViewModel : BaseViewModel
    {
        private Choice? selectedChoice;
        private Effect? selectedEffect;
        public EffectEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition effet";
            effectTypes = new List<EffectType>
            {
                EffectType.AddGold,
                EffectType.RemoveGold,
                EffectType.Damage,
                EffectType.RemoveFlag,
                EffectType.SetFlag,
                EffectType.AddPotion,
                EffectType.RemovePotion,
                EffectType.AddKey,
                EffectType.RemoveKey
            };
            selectedEffectType = EffectType.AddGold;
            amount = 1;
            flagKey = string.Empty;
            UpdateFieldsVisibility();
        }

        [ObservableProperty]
        private List<EffectType> effectTypes;

        [ObservableProperty]
        private EffectType selectedEffectType;

        [ObservableProperty]
        private int? amount;

        [ObservableProperty]
        private string? flagKey;

        [ObservableProperty]
        private bool isAmountVisible;

        [ObservableProperty]
        private bool isFlagKeyVisible;

        partial void OnSelectedEffectTypeChanged(EffectType value)
        {
            UpdateFieldsVisibility();
        }

        private void UpdateFieldsVisibility()
        {
            IsAmountVisible =
                SelectedEffectType == EffectType.AddGold ||
                SelectedEffectType == EffectType.RemoveGold ||
                SelectedEffectType == EffectType.Damage ||
                SelectedEffectType == EffectType.AddPotion ||
                SelectedEffectType == EffectType.RemovePotion ||
                SelectedEffectType == EffectType.AddKey ||
                SelectedEffectType == EffectType.RemoveKey;

            IsFlagKeyVisible =
                SelectedEffectType == EffectType.SetFlag ||
                SelectedEffectType == EffectType.RemoveFlag;

            if (IsAmountVisible)
            {
                if (Amount == null)
                {
                    Amount = 1;
                }

                FlagKey = string.Empty;
            }
            else if (IsFlagKeyVisible)
            {
                Amount = null;
            }
        }

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder effet", $"La sauvegarde de l'effet sera ajoutée plus tard.");
        }

        public void PrepareNewEffect(Choice choice)
        {
            if (choice == null)
            {
                return;
            }

            selectedChoice = choice;
            selectedEffect = null;

            PageTitle = "Nouvel effet";

            SelectedEffectType = EffectType.AddGold;
            Amount = 1;
            FlagKey = string.Empty;

            UpdateFieldsVisibility();
        }

        public void LoadEffect(Choice choice, Effect effect)
        {
            if (choice == null || effect == null)
            {
                return;
            }

            selectedChoice = choice;
            selectedEffect = effect;

            PageTitle = "Édition effet";

            SelectedEffectType = effect.Type;
            Amount = effect.Amount;
            FlagKey = effect.FlagKey;

            UpdateFieldsVisibility();
        }
    }
}
