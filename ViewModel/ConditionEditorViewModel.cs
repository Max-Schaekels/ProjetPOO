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
using Condition = ProjetPOO.Model.Story.Condition;

namespace ProjetPOO.ViewModel
{
    public partial class ConditionEditorViewModel : BaseViewModel
    {
        private Choice? selectedChoice;
        private Condition? selectedCondition;
        public ConditionEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition condition";

            conditionTypes = new List<ConditionType>
            {
                ConditionType.MinGold,
                ConditionType.HasPotion,
                ConditionType.HasKey
            };
            selectedConditionType = ConditionType.MinGold;
            value = 0;
        }

        [ObservableProperty]
        private List<ConditionType> conditionTypes;

        [ObservableProperty]
        private ConditionType selectedConditionType;

        [ObservableProperty]
        private int value;


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

            if (Value <= 0)
            {
                await alertService.ShowAlert("Valeur invalide", "La valeur de la condition doit être supérieure à 0.");
                return;
            }

            try
            {
                if (selectedCondition == null)
                {
                    Condition condition = new Condition(
                        SelectedConditionType,
                        Value
                    );

                    condition.SetChoice(selectedChoice.Id);

                    dataAccess.AddCondition(condition);

                    await alertService.ShowAlert("Condition sauvegardée", "La nouvelle condition a bien été créée.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedCondition.ChangeType(SelectedConditionType);
                selectedCondition.ChangeMinValue(Value);

                dataAccess.UpdateCondition(selectedCondition);

                await alertService.ShowAlert("Condition sauvegardée", "La condition a bien été mise à jour.");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
        }

        public void PrepareNewCondition(Choice choice)
        {
            if (choice == null)
            {
                return;
            }

            selectedChoice = choice;
            selectedCondition = null;

            PageTitle = "Nouvelle condition";

            SelectedConditionType = ConditionType.MinGold;
            Value = 1;
        }

        public void LoadCondition(Choice choice, Condition condition)
        {
            if (choice == null || condition == null)
            {
                return;
            }

            selectedChoice = choice;
            selectedCondition = condition;

            PageTitle = "Édition condition";

            SelectedConditionType = condition.Type;
            Value = condition.MinValue;
        }


    }
}
