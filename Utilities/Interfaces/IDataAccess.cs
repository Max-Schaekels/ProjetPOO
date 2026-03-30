using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.Utilities.Interfaces
{
    public interface IDataAccess
    {
        string AccessPath { get; set; }

        List<Scenario> GetAllScenarios();
        Scenario? GetScenarioById(int scenarioId);
        void AddScenario(Scenario scenario);
        void UpdateScenario(Scenario scenario);
        void DeleteScenario(int scenarioId);

        Scenario? LoadScenario(int scenarioId);
        void SaveScenario(Scenario scenario);

        ScenesCollection GetAllScenes();
        ScenesCollection GetScenesByScenarioId(int scenarioId);
        Scene? GetSceneById(int sceneId);
        void AddScene(Scene scene);
        void UpdateScene(Scene scene);
        void DeleteScene(int sceneId);

        ChoicesCollection GetAllChoices();
        ChoicesCollection GetChoicesBySceneId(int sceneId);
        Choice? GetChoiceById(int choiceId);
        void AddChoice(Choice choice);
        void UpdateChoice(Choice choice);
        void DeleteChoice(int choiceId);

        ConditionsCollection GetAllConditions();
        ConditionsCollection GetConditionsByChoiceId(int choiceId);
        Condition? GetConditionById(int conditionId);
        void AddCondition(Condition condition);
        void UpdateCondition(Condition condition);
        void DeleteCondition(int conditionId);

        EffectsCollection GetAllEffects();
        EffectsCollection GetEffectsByChoiceId(int choiceId);
        Effect? GetEffectById(int effectId);
        void AddEffect(Effect effect);
        void UpdateEffect(Effect effect);
        void DeleteEffect(int effectId);

        EnemiesCollection GetAllEnemies();
        EnemiesCollection GetEnemiesByScenarioId(int scenarioId);
        Enemy? GetEnemyById(int enemyId);
        void AddEnemy(Enemy enemy);
        void UpdateEnemy(Enemy enemy);
        void DeleteEnemy(int enemyId);

        ShopsCollection GetAllShops();
        ShopsCollection GetShopsByScenarioId(int scenarioId);
        Shop? GetShopById(int shopId);
        void AddShop(Shop shop);
        void UpdateShop(Shop shop);
        void DeleteShop(int shopId);

        PlayerCharactersCollection GetAllPlayerCharacterTemplates();
        PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId);
        PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId);
        void AddPlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate);
        void UpdatePlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate);
        void DeletePlayerCharacterTemplate(int playerCharacterTemplateId);
    }
}
