using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Story.Enums;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class Scenario
    {
        private const int MINIMUM_TITLE_LENGTH = 3;
        private const int MAX_TITLE_LENGTH = 200;
        private const int MINIMUM_DESCRIPTION_LENGTH = 30;

        private static int _nextId = 1;

        private int _id;
        private string _title;
        private string _description;
        private int _startSceneId;
        private List<Scene> _scenes;
        private List<Enemy> _enemies;



        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _id = value;
                }
            }
        }
        public string Title
        {
            get => _title;
            private set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
                {
                    _title = value;
                }
            }
        }

        public string Description
        {
            get => _description;
            private set
            {
                if (ValidUtils.CheckEntryDescription(value, MINIMUM_DESCRIPTION_LENGTH))
                {
                    _description = value;
                }
            }
        }

        public int StartSceneId
        {
            get => _startSceneId;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                {
                    _startSceneId = value;
                }
            }
        }

        public IReadOnlyList<Scene> Scenes => _scenes.AsReadOnly();
        public IReadOnlyList<Enemy> Enemies => _enemies.AsReadOnly();

        public Scenario(string title, string description)
        {
            _scenes = new List<Scene>();
            _enemies = new List<Enemy>();

            Id = GenerateId();
            Title = title;
            Description = description;

            StartSceneId = 0; // draft-friendly
        }

        public Scenario()
        {
            _scenes = new List<Scene>();
            _enemies = new List<Enemy>();

            Title = string.Empty;
            Description = string.Empty;

            Id = GenerateId();
            StartSceneId = 0;
        }

        // Constructeur pour Load (depuis la base de données) avec vérifications de cohérence (ex: les scènes rattachées ont bien le ScenarioId du scénario chargé, pas de scène dupliquée, etc.)
        public static Scenario Load(int id, string title, string description, int startSceneId, List<Scene>? scenes, List<Enemy>? enemies = null)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            Scenario scenario = new Scenario();

            scenario.Id = id;
            EnsureNextIdIsAfterLoadedId(id);

            scenario.Title = title;
            scenario.Description = description;

            scenario._scenes = new List<Scene>();

            if (scenes != null)
            {
                for (int i = 0; i < scenes.Count; i++)
                {
                    Scene scene = scenes[i];
                    if (scene == null)
                    {
                        continue;
                    }

                    bool alreadyExists = scenario._scenes.Any(s => s != null && s.Id == scene.Id);
                    if (alreadyExists)
                    {
                        continue;
                    }

                    // Cohérence ScenarioId
                    if (scene.ScenarioId == 0)
                    {
                        // Draft/partiel : on rattache
                        scene.AssignToScenario(scenario.Id);
                    }
                    else if (scene.ScenarioId != scenario.Id)
                    {
                        throw new InvalidOperationException(
                            $"Load Scenario incohérent : la scène \"{scene.Title}\" a ScenarioId={scene.ScenarioId} mais le scénario chargé a Id={scenario.Id}.");
                    }

                    scenario._scenes.Add(scene);
                }
            }

            scenario._enemies = new List<Enemy>();

            if (enemies != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    Enemy enemy = enemies[i];
                    if (enemy == null)
                    {
                        continue;
                    }

                    bool alreadyExists = scenario._enemies.Any(e => e != null && e.Id == enemy.Id);
                    if (alreadyExists)
                    {
                        continue;
                    }

                    // Cohérence ScenarioId 
                    if (enemy.ScenarioId == 0)
                    {
                        enemy.AssignToScenario(scenario.Id);
                    }
                    else if (enemy.ScenarioId != scenario.Id)
                    {
                        throw new InvalidOperationException(
                            $"Load Scenario incohérent : l'ennemi \"{enemy.Name}\" a ScenarioId={enemy.ScenarioId} mais le scénario chargé a Id={scenario.Id}.");
                    }

                    scenario._enemies.Add(enemy);
                }
            }

            scenario.AssignStartScene(startSceneId);

            return scenario;
        }

        public void Rename(string title)
        {
            if (!ValidUtils.CheckEntryName(title, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
            {
                throw new ArgumentException($"Title doit être compris entre {MINIMUM_TITLE_LENGTH} et {MAX_TITLE_LENGTH} caractères.", nameof(title));
            }

            Title = title;
        }

        public void ChangeDescription(string description)
        {
            if (!ValidUtils.CheckEntryDescription(description, MINIMUM_DESCRIPTION_LENGTH))
            {
                throw new ArgumentException($"Description doit contenir au moins {MINIMUM_DESCRIPTION_LENGTH} caractères.", nameof(description));
            }

            Description = description;
        }

        public void AddEnemy(Enemy enemy)
        {
            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            bool alreadyExists = _enemies.Any(e => e != null && e.Id == enemy.Id);
            if (alreadyExists)
            {
                return;
            }

            if (enemy.ScenarioId == 0)
            {
                enemy.AssignToScenario(Id);
            }
            else if (enemy.ScenarioId != Id)
            {
                throw new InvalidOperationException(
                    $"AddEnemy incohérent : l'ennemi \"{enemy.Name}\" a ScenarioId={enemy.ScenarioId} mais le scénario courant a Id={Id}.");
            }

            _enemies.Add(enemy);
        }

        public void AssignStartScene(int startSceneId)
        {
            if (!ValidUtils.CheckIfNonNegativeNumber(startSceneId))
            {
                throw new ArgumentException("startSceneId doit être >= 0.", nameof(startSceneId));
            }

            StartSceneId = startSceneId;
        }

        public void ClearStartScene()
        {
            StartSceneId = 0;
        }

        public Scene GetStartScene()
        {
            if (StartSceneId == 0)
            {
                throw new InvalidOperationException("StartSceneId n'est pas défini (0). Le scénario est probablement en draft.");
            }
            Scene? start = GetSceneById(StartSceneId);
            if (start == null)
            {
                throw new InvalidOperationException($"La scène de départ avec l'Id {StartSceneId} n'existe pas dans le scénario.");
            }

            return start;
        }

        public Scene? GetSceneById(int sceneId)
        {
            Scene? scene = _scenes.FirstOrDefault(s => s != null && s.Id == sceneId);
            return scene;
        }

        public void AddScene(Scene scene)
        {
            if (scene == null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            bool alreadyExists = _scenes.Any(s => s != null && s.Id == scene.Id);
            if (alreadyExists)
            {
                return; // La scène est déjà dans le scénario, on ne l'ajoute pas une seconde fois
            }

            _scenes.Add(scene);

            scene.AssignToScenario(Id);

        }

        public void RemoveScene(int sceneId)
        {
            Scene? toRemove = _scenes.FirstOrDefault(s => s != null && s.Id == sceneId);
            if (toRemove == null)
            {
                return; // La scène n'existe pas dans le scénario, rien à faire
            }

            _scenes.Remove(toRemove);

            toRemove.ClearScenario();

            if (StartSceneId == sceneId)
            {
                StartSceneId = 0;
            }
        }


        public Enemy? GetEnemyById(int enemyId)
        {
            Enemy? enemy = _enemies.FirstOrDefault(e => e != null && e.Id == enemyId);
            return enemy;
        }


        public bool ValidateSafe(out List<string> errors)
        {
            errors = new List<string>();

            if (!ValidUtils.CheckEntryName(Title, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
            {
                errors.Add($"Le titre doit comporter entre {MINIMUM_TITLE_LENGTH} et {MAX_TITLE_LENGTH} caractères.");
            }
            if (!ValidUtils.CheckEntryDescription(Description, MINIMUM_DESCRIPTION_LENGTH))
            {
                errors.Add($"La description doit comporter au moins {MINIMUM_DESCRIPTION_LENGTH} caractères.");
            }

            if (!ValidUtils.CheckIfNonNegativeNumber(StartSceneId))
            {
                errors.Add("StartSceneId doit être un nombre >= 0.");
            }

            if (_scenes == null)
            {
                errors.Add("Scenario : la liste des scènes est null (liste corrompue).");
                return false;
            }

            if (_enemies == null)
            {
                errors.Add("Scenario : la liste des ennemis est null (liste corromue).");
                return false;
            }

            // vérifier null + ids dupliqués + valider les scènes
            for (int i = 0; i < _scenes.Count; i++)
            {
                Scene scene = _scenes[i];

                if (scene == null)
                {
                    errors.Add("Scenario : une scène est null (liste corrompue).");
                    continue;
                }

                bool duplicate = _scenes.Any(s => s != null && s.Id == scene.Id && !ReferenceEquals(s, scene));
                if (duplicate)
                {
                    errors.Add($"Scenario : Id de scène dupliqué ({scene.Id}).");
                }

                List<string> sceneErrors;
                bool ok = scene.ValidateSafe(out sceneErrors);

                if (!ok)
                {
                    foreach (string sceneError in sceneErrors)
                    {
                        errors.Add($"Scene \"{scene.Title}\" - {sceneError}");
                    }
                }
            }

            // vérifier null + ids dupliqués pour les ennemis (validation légère)
            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy enemy = _enemies[i];

                if (enemy == null)
                {
                    errors.Add("Scenario : un ennemi est null (liste corrompue).");
                    continue;
                }

                bool duplicate = _enemies.Any(e => e != null && e.Id == enemy.Id && !ReferenceEquals(e, enemy));
                if (duplicate)
                {
                    errors.Add($"Scenario : Id d'ennemi dupliqué ({enemy.Id}).");
                }

                if (!ValidUtils.CheckIfNonNegativeNumber(enemy.ScenarioId))
                {
                    errors.Add($"Scenario : EnemyId={enemy.Id} a un ScenarioId invalide (<0).");
                }
            }

            return errors.Count == 0;
        }

        public bool ValidatePlayable(out List<string> errors)
        {
            bool baseOk = ValidateSafe(out errors);

            if (_scenes.Count == 0)
            {
                errors.Add("Pour jouer, le scénario doit contenir au moins une scène.");
            }

            if (!ValidUtils.CheckIfPositiveNumber(StartSceneId))
            {
                errors.Add("Pour jouer, StartSceneId doit être > 0.");
            }
            else
            {
                if (!_scenes.Any(s => s != null && s.Id == StartSceneId))
                {
                    errors.Add($"StartSceneId={StartSceneId} n'existe pas dans le scénario.");
                }
            }

            // En jouable : tous les ennemis doivent être rattachés au scénario (ScenarioId == Id et > 0)
            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy enemy = _enemies[i];
                if (enemy == null)
                {
                    continue;
                }

                if (!ValidUtils.CheckIfPositiveNumber(enemy.ScenarioId))
                {
                    errors.Add($"Enemy \"{enemy.Name}\" : en jouable, ScenarioId doit être > 0.");
                }
                else if (enemy.ScenarioId != Id)
                {
                    errors.Add($"Enemy \"{enemy.Name}\" : ScenarioId doit valoir {Id} (actuel={enemy.ScenarioId}).");
                }
            }

            // Vérifier ScenarioId des scènes + ValidatePlayable des scènes
            for (int i = 0; i < _scenes.Count; i++)
            {
                Scene scene = _scenes[i];
                if (scene == null)
                {
                    continue;
                }

                if (!ValidUtils.CheckIfPositiveNumber(scene.ScenarioId) || scene.ScenarioId != Id)
                {
                    errors.Add($"Scene \"{scene.Title}\" : ScenarioId doit valoir {Id} (actuel={scene.ScenarioId}).");
                }

                List<string> sceneErrors;
                bool ok = scene.ValidatePlayable(out sceneErrors);
                if (!ok)
                {
                    foreach (string sceneError in sceneErrors)
                    {
                        errors.Add($"Scene \"{scene.Title}\" - {sceneError}");
                    }
                }
            }

            // Vérifier que tous les TargetSceneId existent (Choices + Combat targets)
            List<int> existingSceneIds = _scenes.Where(s => s != null).Select(s => s.Id).ToList();

            for (int i = 0; i < _scenes.Count; i++)
            {
                Scene scene = _scenes[i];
                if (scene == null)
                {
                    continue;
                }

                // Choices
                for (int j = 0; j < scene.Choices.Count; j++)
                {
                    Choice choice = scene.Choices[j];
                    if (choice == null)
                    {
                        continue;
                    }

                    int targetId = choice.TargetSceneId;

                    if (!ValidUtils.CheckIfPositiveNumber(targetId))
                    {
                        errors.Add($"Scene \"{scene.Title}\" : le choix \"{choice.Label}\" doit avoir un TargetSceneId > 0.");
                        continue;
                    }

                    if (!existingSceneIds.Contains(targetId))
                    {
                        errors.Add($"Scene \"{scene.Title}\" : le choix \"{choice.Label}\" pointe vers TargetSceneId={targetId} inexistant.");
                    }
                }

                // Combat targets + existence EnemyId dans le scénario
                if (scene.Type == SceneType.Combat)
                {
                    if (scene.EnemyId == null)
                    {
                        errors.Add($"Scene \"{scene.Title}\" : EnemyId est requis pour une scène Combat.");
                    }
                    else
                    {
                        Enemy? enemy = GetEnemyById(scene.EnemyId.Value);
                        if (enemy == null)
                        {
                            errors.Add($"Scene \"{scene.Title}\" : EnemyId={scene.EnemyId.Value} n'existe pas dans la liste d'ennemis du scénario.");
                        }
                        else if (enemy.ScenarioId != Id)
                        {
                            errors.Add($"Scene \"{scene.Title}\" : EnemyId={enemy.Id} a ScenarioId={enemy.ScenarioId} (attendu={Id}).");
                        }
                    }

                    if (scene.VictoryTargetSceneId == null || !existingSceneIds.Contains(scene.VictoryTargetSceneId.Value))
                    {
                        errors.Add($"Scene \"{scene.Title}\" : VictoryTargetSceneId invalide ou inexistant.");
                    }

                    if (scene.DefeatTargetSceneId == null || !existingSceneIds.Contains(scene.DefeatTargetSceneId.Value))
                    {
                        errors.Add($"Scene \"{scene.Title}\" : DefeatTargetSceneId invalide ou inexistant.");
                    }

                    if (scene.FleeTargetSceneId == null || !existingSceneIds.Contains(scene.FleeTargetSceneId.Value))
                    {
                        errors.Add($"Scene \"{scene.Title}\" : FleeTargetSceneId invalide ou inexistant.");
                    }
                }
            }

            return baseOk && errors.Count == 0;
        }

        private static int GenerateId()
        {
            int newId = _nextId;
            _nextId = _nextId + 1;
            return newId;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (_nextId <= loadedId)
            {
                _nextId = loadedId + 1;
            }
        }

    }
}
