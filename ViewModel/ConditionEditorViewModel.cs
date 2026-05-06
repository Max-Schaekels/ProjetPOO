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
    public partial class ConditionEditorViewModel : BaseViewModel
    {
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

        
    }
}
