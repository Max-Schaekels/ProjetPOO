using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay;
using ProjetPOO.Model.Story;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Condition = ProjetPOO.Model.Story.Condition;
using Effect = ProjetPOO.Model.Story.Effect;

namespace ProjetPOO.Utilities.DataAccess
{
    public abstract class DataAccess : IDataAccess
    {
        private string _accessPath;

        /// <summary>
        /// constructor with just the fileName for AccessPath
        /// </summary>
        /// <param name="filePath"></param>
        public DataAccess(string filePath)
        {
            AccessPath = filePath;
        }

        /// <summary>
        /// Constructor with fileName only one and authorized file extensions
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="extensions"> Exemple {".xlxs",".json"}</param>
        public DataAccess(string filePath, string[] extensions)
        {
            Extensions = new List<string>(extensions.ToList());
            AccessPath = filePath;
        }

        /// <summary>
        /// Constructor associated with a DatafileManager object, it will contains all datas files informations (path and subject)
        /// </summary>
        /// <param name="dfm"></param>
        public DataAccess(DataFilesManager dfm)
        {
            DataFilesManager = dfm;
        }

        public DataFilesManager DataFilesManager { get; set; }

        /// <summary>
        /// AccessPath file to the data source
        /// </summary>
        public virtual string AccessPath
        {
            get => _accessPath;
            set
            {
                _accessPath = value;
            }
        }

        /// <summary>
        /// List of authorized extensions .txt, csv, .Json, .xml ...for the AccessPath file
        /// </summary>
        public List<string> Extensions { get; set; }

        /// <summary>
        /// Continue to check AccessPath even after constructor (in the case of the file may be moved, renamed or deleted)
        /// </summary>
        public bool IsValidAccessPath => CheckAccessPath(AccessPath);

        public abstract List<Scenario> GetAllScenarios();
        public abstract Scenario? GetScenarioById(int scenarioId);
        public abstract void AddScenario(Scenario scenario);
        public abstract void UpdateScenario(Scenario scenario);
        public abstract void DeleteScenario(int scenarioId);

        public abstract Scenario? LoadScenario(int scenarioId);
        public abstract void SaveScenario(Scenario scenario);

        public abstract ScenesCollection GetAllScenes();
        public abstract ScenesCollection GetScenesByScenarioId(int scenarioId);
        public abstract Scene? GetSceneById(int sceneId);
        public abstract void AddScene(Scene scene);
        public abstract void UpdateScene(Scene scene);
        public abstract void DeleteScene(int sceneId);

        public abstract ChoicesCollection GetAllChoices();
        public abstract ChoicesCollection GetChoicesBySceneId(int sceneId);
        public abstract Choice? GetChoiceById(int choiceId);
        public abstract void AddChoice(Choice choice);
        public abstract void UpdateChoice(Choice choice);
        public abstract void DeleteChoice(int choiceId);

        public abstract ConditionsCollection GetAllConditions();
        public abstract ConditionsCollection GetConditionsByChoiceId(int choiceId);
        public abstract Condition? GetConditionById(int conditionId);
        public abstract void AddCondition(Condition condition);
        public abstract void UpdateCondition(Condition condition);
        public abstract void DeleteCondition(int conditionId);

        public abstract EffectsCollection GetAllEffects();
        public abstract EffectsCollection GetEffectsByChoiceId(int choiceId);
        public abstract Effect? GetEffectById(int effectId);
        public abstract void AddEffect(Effect effect);
        public abstract void UpdateEffect(Effect effect);
        public abstract void DeleteEffect(int effectId);

        public abstract EnemiesCollection GetAllEnemies();
        public abstract EnemiesCollection GetEnemiesByScenarioId(int scenarioId);
        public abstract Enemy? GetEnemyById(int enemyId);
        public abstract void AddEnemy(Enemy enemy);
        public abstract void UpdateEnemy(Enemy enemy);
        public abstract void DeleteEnemy(int enemyId);

        public abstract ShopsCollection GetAllShops();
        public abstract ShopsCollection GetShopsByScenarioId(int scenarioId);
        public abstract Shop? GetShopById(int shopId);
        public abstract void AddShop(Shop shop);
        public abstract void UpdateShop(Shop shop);
        public abstract void DeleteShop(int shopId);

        public abstract PlayerCharactersCollection GetAllPlayerCharacterTemplates();
        public abstract PlayerCharactersCollection GetPlayerCharacterTemplatesByScenarioId(int scenarioId);
        public abstract PlayerCharacterTemplate? GetPlayerCharacterTemplateById(int playerCharacterTemplateId);
        public abstract void AddPlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate);
        public abstract void UpdatePlayerCharacterTemplate(PlayerCharacterTemplate playerCharacterTemplate);
        public abstract void DeletePlayerCharacterTemplate(int playerCharacterTemplateId);

        /// <summary>
        /// Check AccessPath to the data source file. File path must exist and if
        /// extensions are specified, the extension file must match to one of them
        /// </summary>
        /// <returns>true if valid path and extension file</returns>
        public bool CheckAccessPath(string tryPath)
        {
            if (File.Exists(tryPath))
            {
                if (Extensions?.Any() ?? false)
                {
                    string pattern = "";
                    foreach (string ext in Extensions)
                    {
                        pattern += ext + "|";
                    }
                    pattern = pattern.Substring(0, pattern.Length - 1);

                    if (!Regex.IsMatch(tryPath, pattern + "$"))
                    {
                        Console.WriteLine($"L'extension du fichier {tryPath} n'est pas valide, extensions attendues : {pattern}", "Erreur de fichier");
                        return false;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine($"Le fichier {tryPath} n'existe pas", "Erreur de fichier");
                return false;
            }
        }
    }
}
