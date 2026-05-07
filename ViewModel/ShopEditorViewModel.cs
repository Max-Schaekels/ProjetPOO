using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.ViewModel
{
    public partial class ShopEditorViewModel : BaseViewModel
    {
        public ShopEditorViewModel(IAlertService alertService, IDataAccess dataAccessService) : base(alertService, dataAccessService)
        {
            PageTitle = "Édition boutique";
            shopName = string.Empty;
            potionPrice = 0;
            keyPrice = 0;
        }

        [ObservableProperty]
        private string shopName;

        [ObservableProperty]
        private int potionPrice;

        [ObservableProperty]
        private int keyPrice;

        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task Save()
        {
            await alertService.ShowAlert("Sauvegarder boutique", "La sauvegarde de la boutique sera ajoutée plus tard.");
        }
    }
}
