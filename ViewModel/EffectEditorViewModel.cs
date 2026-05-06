using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class EffectEditorViewModel : BaseViewModel
    {
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
    }
}
