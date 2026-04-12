using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Model.Story.Enums;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;
using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.DataAccess;
using ProjetPOO.ViewModel;


namespace ProjetPOO.View

{
    public partial class MainPage : ContentPage
    {
        private const string CONFIG_HOME_CSV = @"C:\ProjetPOO\Max-Schaekels\ProjetPOO\Configuration\Datas\Config.local.txt";
        private const string CONFIG_PORT_CSV = @"C:\POO\Brasserie\Configuration\Datas\Config.local.txt";

        private const string CONFIG_HOME_JSON = @"C:\ProjetPOO\Max-Schaekels\ProjetPOO\Configuration\Datas\ConfigJson.local.txt";
        private const string CONFIG_PORT_JSON = @"C:\POO\Brasserie\Configuration\Datas\ConfigJson.local.txt";

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
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


        }

        private void buttonTestDataAccess_Clicked(object sender, EventArgs e)
        {
            string CONFIG_FILE = CONFIG_HOME_CSV; 
            DataFilesManager dataFilesManager = new DataFilesManager(CONFIG_FILE);
            DataAccessCsvFile da = new DataAccessCsvFile(dataFilesManager);
            lblDebug.Text = "===== TEST DATA ACCESS =====";

            // SCENARIOS
            List<Scenario> scenarios = da.GetAllScenarios();
            foreach (Scenario s in scenarios)
            {
                lblDebug.Text += $"\n[SCENARIO] Id: {s.Id} - Title: {s.Title} - StartSceneId: {s.StartSceneId}";
            }

            // SCENES
            ScenesCollection scenes = da.GetAllScenes();
            foreach (Scene s in scenes)
            {
                lblDebug.Text += $"\n[SCENE] Id: {s.Id} - Title: {s.Title} - Type: {s.Type} - ScenarioId: {s.ScenarioId}";
            }

            // CHOICES
            ChoicesCollection choices = da.GetAllChoices();
            foreach (Choice c in choices)
            {
                lblDebug.Text += $"\n[CHOICE] Id: {c.Id} - Label: {c.Label} - SceneId: {c.SceneId} - TargetSceneId: {c.TargetSceneId}";
            }

            // CONDITIONS
            ConditionsCollection conditions = da.GetAllConditions();
            foreach (Condition c in conditions)
            {
                lblDebug.Text += $"\n[CONDITION] Id: {c.Id} - ChoiceId: {c.ChoiceId} - Type: {c.Type} - MinValue: {c.MinValue}";
            }

            // EFFECTS
            EffectsCollection effects = da.GetAllEffects();
            foreach (Effect efx in effects)
            {
                lblDebug.Text += $"\n[EFFECT] Id: {efx.Id} - ChoiceId: {efx.ChoiceId} - Type: {efx.Type} - Amount: {efx.Amount} - Flag: {efx.FlagKey}";
            }

            // ENEMIES
            EnemiesCollection enemies = da.GetAllEnemies();
            foreach (Enemy en in enemies)
            {
                lblDebug.Text += $"\n[ENEMY] Id: {en.Id} - Name: {en.Name} - ScenarioId: {en.ScenarioId} - HP: {en.MaxHp}";
            }

            // SHOPS
            ShopsCollection shops = da.GetAllShops();
            foreach (Shop s in shops)
            {
                lblDebug.Text += $"\n[SHOP] Id: {s.Id} - Name: {s.Name} - ScenarioId: {s.ScenarioId}";
            }

            // PLAYER TEMPLATES
            PlayerCharactersCollection players = da.GetAllPlayerCharacterTemplates();
            foreach (PlayerCharacterTemplate p in players)
            {
                lblDebug.Text += $"\n[PLAYER] Id: {p.Id} - Name: {p.Name} - ScenarioId: {p.ScenarioId} - HP: {p.MaxHp}";
            }
        }

        private void buttonTestConvertCsvToJson_Clicked(object sender, EventArgs e)
        {
            DataFilesManager dfmCsv = new DataFilesManager(CONFIG_HOME_CSV);
            DataAccessCsvFile daCsv = new DataAccessCsvFile(dfmCsv);

            List<Scenario> scenarios = daCsv.GetAllScenarios();
            ScenesCollection scenes = daCsv.GetAllScenes();
            ChoicesCollection choices = daCsv.GetAllChoices();
            ConditionsCollection conditions = daCsv.GetAllConditions();
            EffectsCollection effects = daCsv.GetAllEffects();
            EnemiesCollection enemies = daCsv.GetAllEnemies();
            ShopsCollection shops = daCsv.GetAllShops();
            PlayerCharactersCollection playerCharacterTemplates = daCsv.GetAllPlayerCharacterTemplates();

            DataFilesManager dfmJson = new DataFilesManager(CONFIG_HOME_JSON);
            DataAccessJsonFile daJson = new DataAccessJsonFile(dfmJson);

            daJson.UpdateAllScenarios(scenarios);
            daJson.UpdateAllScenes(scenes);
            daJson.UpdateAllChoices(choices);
            daJson.UpdateAllConditions(conditions);
            daJson.UpdateAllEffects(effects);
            daJson.UpdateAllEnemies(enemies);
            daJson.UpdateAllShops(shops);
            daJson.UpdateAllPlayerCharacterTemplates(playerCharacterTemplates);

            lblDebug.Text = "Conversion CSV vers JSON terminée.";
        }

        private void buttonTestJson_Clicked(object sender, EventArgs e)
        {
            DataFilesManager dataFilesManager = new DataFilesManager(CONFIG_HOME_JSON);
            DataAccessJsonFile da = new DataAccessJsonFile(dataFilesManager);
            lblDebug.Text = "===== TEST DATA ACCESS =====";

            // SCENARIOS
            List<Scenario> scenarios = da.GetAllScenarios();
            foreach (Scenario s in scenarios)
            {
                lblDebug.Text += $"\n[SCENARIO] Id: {s.Id} - Title: {s.Title} - StartSceneId: {s.StartSceneId}";
            }

            // SCENES
            ScenesCollection scenes = da.GetAllScenes();
            foreach (Scene s in scenes)
            {
                lblDebug.Text += $"\n[SCENE] Id: {s.Id} - Title: {s.Title} - Type: {s.Type} - ScenarioId: {s.ScenarioId}";
            }

            // CHOICES
            ChoicesCollection choices = da.GetAllChoices();
            foreach (Choice c in choices)
            {
                lblDebug.Text += $"\n[CHOICE] Id: {c.Id} - Label: {c.Label} - SceneId: {c.SceneId} - TargetSceneId: {c.TargetSceneId}";
            }

            // CONDITIONS
            ConditionsCollection conditions = da.GetAllConditions();
            foreach (Condition c in conditions)
            {
                lblDebug.Text += $"\n[CONDITION] Id: {c.Id} - ChoiceId: {c.ChoiceId} - Type: {c.Type} - MinValue: {c.MinValue}";
            }

            // EFFECTS
            EffectsCollection effects = da.GetAllEffects();
            foreach (Effect efx in effects)
            {
                lblDebug.Text += $"\n[EFFECT] Id: {efx.Id} - ChoiceId: {efx.ChoiceId} - Type: {efx.Type} - Amount: {efx.Amount} - Flag: {efx.FlagKey}";
            }

            // ENEMIES
            EnemiesCollection enemies = da.GetAllEnemies();
            foreach (Enemy en in enemies)
            {
                lblDebug.Text += $"\n[ENEMY] Id: {en.Id} - Name: {en.Name} - ScenarioId: {en.ScenarioId} - HP: {en.MaxHp}";
            }

            // SHOPS
            ShopsCollection shops = da.GetAllShops();
            foreach (Shop s in shops)
            {
                lblDebug.Text += $"\n[SHOP] Id: {s.Id} - Name: {s.Name} - ScenarioId: {s.ScenarioId}";
            }

            // PLAYER TEMPLATES
            PlayerCharactersCollection players = da.GetAllPlayerCharacterTemplates();
            foreach (PlayerCharacterTemplate p in players)
            {
                lblDebug.Text += $"\n[PLAYER] Id: {p.Id} - Name: {p.Name} - ScenarioId: {p.ScenarioId} - HP: {p.MaxHp}";
            }

        }

    }

}
