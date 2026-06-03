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
            if (selectedScenario == null)
            {
                await alertService.ShowAlert("Scénario manquant", "Aucun scénario n'est sélectionné.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ShopName))
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la boutique ne peut pas être vide.");
                return;
            }

            if (ShopName.Trim().Length < 3)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la boutique doit contenir au moins 3 caractères.");
                return;
            }

            if (ShopName.Trim().Length > 50)
            {
                await alertService.ShowAlert("Nom invalide", "Le nom de la boutique ne peut pas dépasser 50 caractères.");
                return;
            }

            if (PotionPrice <= 0)
            {
                await alertService.ShowAlert("Prix invalide", "Le prix d'une potion doit être supérieur à 0.");
                return;
            }

            if (KeyPrice <= 0)
            {
                await alertService.ShowAlert("Prix invalide", "Le prix d'une clé doit être supérieur à 0.");
                return;
            }

            try
            {
                if (selectedShop == null)
                {
                    Shop shop = new Shop(ShopName.Trim(),PotionPrice, KeyPrice );

                    shop.AssignToScenario(selectedScenario.Id);

                    dataAccess.AddShop(shop);

                    await alertService.ShowAlert("Boutique sauvegardée", "La nouvelle boutique a bien été créée.");

                    await Shell.Current.Navigation.PopAsync();
                    return;
                }

                selectedShop.Rename(ShopName.Trim());
                selectedShop.UpdatePotionPrice(PotionPrice);
                selectedShop.UpdateKeyPrice(KeyPrice);

                dataAccess.UpdateShop(selectedShop);

                await alertService.ShowAlert("Boutique sauvegardée", "La boutique a bien été mise à jour.");
            }
            catch (Exception exception)
            {
                await alertService.ShowAlert("Erreur sauvegarde", exception.Message);
            }
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
