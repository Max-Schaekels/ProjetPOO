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
            if (selectedChoice == null)
            {
                await alertService.ShowAlert("Choix manquant", "Aucun choix n'est sélectionné.");
                return;
            }

            if (RequiresAmount(SelectedEffectType) && Amount == null)
            {
                await alertService.ShowAlert("Valeur manquante", "Cet effet nécessite une quantité.");
                return;
            }

            if (RequiresAmount(SelectedEffectType) && Amount <= 0)
            {
                await alertService.ShowAlert("Valeur invalide", "La quantité doit être supérieure à 0.");
                return;
            }

            if (RequiresFlagKey(SelectedEffectType) && string.IsNullOrWhiteSpace(FlagKey))
            {
                await alertService.ShowAlert("Flag manquant", "Cet effet nécessite une clé de flag.");
                return;
            }

            try
            {
                int? normalizedAmount = null;

                if (RequiresAmount(SelectedEffectType))
                {
                    normalizedAmount = Amount;
                }

                string? normalizedFlagKey = null;

                if (RequiresFlagKey(SelectedEffectType))
                {
                    normalizedFlagKey = FlagKey.Trim();
                }

                if (selectedEffect == null)
                {
                    Effect effect = new Effect( SelectedEffectType,normalizedAmount,normalizedFlagKey );

                    effect.SetChoice(selectedChoice.Id);

                    dataAccess.AddEffect(effect);

                    await alertService.ShowAlert("Effet sauvegardé", "Le nouvel effet a bien été créé.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedEffect.ChangeType(SelectedEffectType);
                selectedEffect.ChangeAmount(normalizedAmount);
                selectedEffect.ChangeFlagKey(normalizedFlagKey);

                dataAccess.UpdateEffect(selectedEffect);

                await alertService.ShowAlert("Effet sauvegardé", "L'effet a bien été mis à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
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

        /// <summary>
        /// Indique si le type d'effet sélectionné nécessite une valeur numérique.
        /// Par exemple : ajouter de l'or, retirer de l'or, infliger des dégâts,
        /// ajouter ou retirer des potions, ajouter ou retirer des clés.
        /// </summary>
        /// <param name="effectType">Type d'effet à vérifier.</param>
        /// <returns>True si l'effet nécessite une quantité, sinon false.</returns>
        private bool RequiresAmount(EffectType effectType)
        {
            return effectType == EffectType.AddGold
                || effectType == EffectType.RemoveGold
                || effectType == EffectType.Damage
                || effectType == EffectType.AddPotion
                || effectType == EffectType.RemovePotion
                || effectType == EffectType.AddKey
                || effectType == EffectType.RemoveKey;
        }

        /// <summary>
        /// Indique si le type d'effet sélectionné nécessite une clé de flag.
        /// Par exemple : définir un flag ou retirer un flag.
        /// </summary>
        /// <param name="effectType">Type d'effet à vérifier.</param>
        /// <returns>True si l'effet nécessite une clé de flag, sinon false.</returns>
        private bool RequiresFlagKey(EffectType effectType)
        {
            return effectType == EffectType.SetFlag
                || effectType == EffectType.RemoveFlag;
        }
    }
}
