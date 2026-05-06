using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class ChoiceEditorViewModel : BaseViewModel
    {
        public ChoiceEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
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
            await alertService.ShowAlert("Ajouter condition", $"L'ajout d'une condition sera ajouté plus tard.");
        }

        [RelayCommand()]
        private async Task NewEffect()
        {
            await alertService.ShowAlert("Ajouter effet", $"L'ajout d'un effet sera ajouté plus tard.");
        }

    }
}
