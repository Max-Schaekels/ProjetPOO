using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Data;
using ProjetPOO.Model.Game;


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
            
            Loot firstLoot = new Loot( 100, 2, 1);
            Loot secondLoot = new Loot( 50, 1, 0);

        }

        private void buttonCreateInventory_Clicked(object sender, EventArgs e)
        {
            Inventory firstInventory = new Inventory( 5 , 5);
            Inventory secondInventory = new Inventory( 3, 2);
        }

        private void buttonInitialiseAllClass_Clicked(object sender, EventArgs e)
        {
            Choice choice = new Choice("Choix numéro 1", 0, 0);
            Condition condition = new Condition(ConditionType.HasPotion, 1);
            Effect effect = new Effect(EffectType.AddGold, 100);

            Scenario scenario = new Scenario("Scénario numéro 1", "Description du scénario numéro 1 assez longue pour être valide.");

            PlayerCharacterTemplate template = new PlayerCharacterTemplate("Héros", 100, 20, 10, 5, 0, 1);
            scenario.PlayerCharacters.AddPlayer(template);

            Scene scene = new Scene("Scène numéro 1", "Description de la scène numéro 1", SceneType.Normal);
            scenario.Scenes.AddScene(scene);
            scenario.AssignStartScene(scene.Id);

            PlayerCharacterTemplate selectedTemplate = scenario.GetDefaultPlayerCharacterTemplate();
            PlayerCharacterInstance player = selectedTemplate.CreateInstance();

            Enemy enemy = new Enemy("Gobelin 1", 50, 15, 5, 3, EnemyType.Goblin, 50, 0, 25, 50, 0, 2, 10, 0, 3);
            scenario.AddEnemy(enemy);

            Inventory inventory = new Inventory(5, 5);
            Shop shop = new Shop("La boutique de Loïc", 10, 5);
            scenario.AddShop(shop);

            Loot loot = new Loot(100, 2, 1);

            GameState gameState = new GameState( scenario.GetStartScene().Id, scenario.Id, 10, inventory, player);

            CombatState combatState = new CombatState(player, enemy);

            DataAccess dataAccess = new DataAccess();
        }
    }

}
