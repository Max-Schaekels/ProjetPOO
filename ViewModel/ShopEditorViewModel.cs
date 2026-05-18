using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
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
        private Scenario? selectedScenario;
        private Shop? selectedShop;
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

        public void PrepareNewShop(Scenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedShop = null;

            PageTitle = "Nouvelle boutique";

            ShopName = string.Empty;

            PotionPrice = 10;
            KeyPrice = 10;
        }

        public void LoadShop(Scenario scenario, Shop shop)
        {
            if (scenario == null || shop == null)
            {
                return;
            }

            selectedScenario = scenario;
            selectedShop = shop;

            PageTitle = "Édition boutique";

            ShopName = shop.Name;

            PotionPrice = shop.PotionPrice;
            KeyPrice = shop.KeyPrice;
        }
    }
}
