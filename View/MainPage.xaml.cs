using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Inventory;

namespace ProjetPOO.View

{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonCreateLoot_Clicked(object sender, EventArgs e)
        {
            
            Loot firstLoot = new Loot(1, 100, 2, 1);
            Loot secondLoot = new Loot(2, 50, 1, 0);

        }

        private void buttonCreateInventory_Clicked(object sender, EventArgs e)
        {
            Inventory firstInventory = new Inventory(1 , 5 , 5);
            Inventory secondInventory = new Inventory(2, 3, 2);
        }
    }

}
