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
            Scenario scenario = new Scenario( "Scénario de test","Description du scénario de test suffisamment longue pour être valide." );

            ScenesCollection scenesCollection = scenario.Scenes;
            EnemiesCollection enemiesCollection = scenario.Enemies;
            ShopsCollection shopsCollection = scenario.Shops;
            PlayerCharactersCollection playerCharactersCollection = scenario.PlayerCharacters;

            Scene introScene = new Scene("Introduction", "Le héros arrive à l'entrée d'un ancien donjon mystérieux.", SceneType.Normal );

            Scene combatScene = new Scene( "Salle du gobelin", "Un gobelin surgit devant le héros et bloque le passage.", SceneType.Combat );

            Scene shopScene = new Scene( "Marchand ambulant", "Un étrange marchand propose potions et clés au voyageur.", SceneType.Shop);

            Scene endScene = new Scene("Fin de test", "Cette scène sert uniquement à terminer le petit test.", SceneType.End);

            scenario.Scenes.AddScene(introScene);
            scenario.Scenes.AddScene(combatScene);
            scenario.Scenes.AddScene(shopScene);
            scenario.Scenes.AddScene(endScene);

            scenario.AssignStartScene(introScene.Id);

            ChoicesCollection introChoicesCollection = introScene.Choices;

            Choice introChoice = new Choice("Entrer dans le donjon", combatScene.Id, introScene.Id);
            introScene.Choices.AddChoice(introChoice);

            ConditionsCollection conditionsCollection = introChoice.Conditions;
            EffectsCollection effectsCollection = introChoice.Effects;

            Condition introCondition = new Condition(ConditionType.MinGold, 5);
            Effect introEffect = new Effect(EffectType.RemoveGold, 5);

            introChoice.Conditions.AddCondition(introCondition);
            introChoice.Effects.AddEffect(introEffect);

            PlayerCharacterTemplate playerTemplate = new PlayerCharacterTemplate( "Héros", 100, 20,10, 5, 0, 1 );
            scenario.PlayerCharacters.AddPlayer(playerTemplate);

            PlayerCharacterInstance playerInstance = playerTemplate.CreateInstance();

            Enemy enemy = new Enemy( "Gobelin test",50, 15,5,3, EnemyType.Goblin,50, 10, 25, 50, 0,2, 10, 0, 1 );
            scenario.Enemies.AddEnemy(enemy);

            EnemyInstance enemyInstance = new EnemyInstance(enemy);

            Shop shop = new Shop("Boutique de test", 10, 5);
            scenario.Shops.AddShop(shop);

            combatScene.SetCombat(enemy.Id, introScene.Id, endScene.Id, shopScene.Id);
            combatScene.ChangeType(SceneType.Combat);

            shopScene.SetShop(shop.Id);
            shopScene.ChangeType(SceneType.Shop);

            Inventory inventory = new Inventory(2, 1);
            Loot loot = new Loot(25, 1, 0);

            GameState gameState = new GameState( scenario.GetStartScene().Id,scenario.Id, 20, inventory, playerInstance);

            CombatState combatState = new CombatState(playerInstance, enemyInstance);
            SaveGame saveGame = new SaveGame("Sauvegarde de test", gameState);

            DataAccess dataAccess = new DataAccess();
            dataAccess.SaveScenario(scenario);
            dataAccess.SaveGame(saveGame);
        }
    }

}
