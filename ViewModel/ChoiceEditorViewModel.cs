using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class ChoiceEditorViewModel : BaseViewModel
    {
        private readonly ConditionEditorPage conditionEditorPage;
        private readonly EffectEditorPage effectEditorPage;
        public ChoiceEditorViewModel(IAlertService alertService, IDataAccess dataAccessService, ConditionEditorPage conditionEditorPage, EffectEditorPage effectEditorPage) : base(alertService, dataAccessService)
        {
            this.conditionEditorPage = conditionEditorPage;
            this.effectEditorPage = effectEditorPage;
            PageTitle = "Édition choix";

            choiceLabel = string.Empty;
            availableTargetScenes = new ScenesCollection();
            selectedTargetScene = null;
            conditions = new ConditionsCollection();
            effects = new EffectsCollection();

        }

        [ObservableProperty]
        private string choiceLabel;

        [ObservableProperty]
        private ScenesCollection availableTargetScenes;

        [ObservableProperty]
        private Scene? selectedTargetScene;

        [ObservableProperty]
        private ConditionsCollection conditions;

        [ObservableProperty]
        private EffectsCollection effects;

        [RelayCommand()]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand()]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder choix", $"La sauvegarde du choix sera ajoutée plus tard.");
        }

        [RelayCommand()]
        private async Task NewCondition()
        {
            await Shell.Current.Navigation.PushAsync(conditionEditorPage);
        }

        [RelayCommand()]
        private async Task NewEffect()
        {
            await Shell.Current.Navigation.PushAsync(effectEditorPage);
        }

    }
}
