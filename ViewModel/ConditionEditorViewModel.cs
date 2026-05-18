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
            await alertService.ShowAlert("Sauvegarder condition", $"La sauvegarde de la condition sera ajoutée plus tard.");
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
