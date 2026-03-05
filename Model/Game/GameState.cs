using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Model.Combat;
using ProjetPOO.Model.Gameplay; 
using ProjetPOO.Utilities.EntriesValidation;


namespace ProjetPOO.Model.Game
{
    public class GameState
    {
        private const int POTION_HEAL_AMOUNT = 5;

        private static int _nextId = 1;

        private int _id;
        private int _currentSceneId;
        private int _scenarioId;
        private int _gold;
        private List<string> _flags;
        private Inventory _playerInventory; 
        private PlayerCharacter _playerCharacter;
        private CombatState? _currentCombat;

        public int Id
        {
            get => _id;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int CurrentSceneId
        {
            get => _currentSceneId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _currentSceneId = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _scenarioId = value;
            }
        }


        public int Gold
        {
            get => _gold;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _gold = value;
            }
        }

        public Inventory PlayerInventory
        {
            get => _playerInventory;
            private set
            {
                if(ValidUtils.CheckIfNotNull(value))
                    _playerInventory = value;
            }
        }

        public PlayerCharacter PlayerCharacter
        {
            get => _playerCharacter;
            private set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _playerCharacter = value;
            }
        }  

        public CombatState? CurrentCombat
        {
            get => _currentCombat;
            private set => _currentCombat = value;
        }

        public IReadOnlyList<string> Flags => _flags.AsReadOnly();



        // Constructeur "normal" (en mémoire)
        public GameState(int currentSceneId, int scenarioId, int gold, Inventory playerInventory, PlayerCharacter playerCharacter)
        {
            Id = GenerateId();

            _flags = new List<string>();

            ScenarioId = scenarioId;
            CurrentSceneId = currentSceneId;
            Gold = gold;
            PlayerInventory = playerInventory;
            PlayerCharacter = playerCharacter;

            CurrentCombat = null;
        }

        // Constructeur privé pour Load (évite de dupliquer l'init)
        private GameState()
        {
            _flags = new List<string>();
            _playerInventory = null!;
            _playerCharacter = null!;
            _currentCombat = null;
        }

        private static int GenerateId()
        {
            int id = _nextId;
            _nextId++;
            return id;
        }

        private static void EnsureNextIdIsAfterLoadedId(int loadedId)
        {
            if (loadedId >= _nextId)
            {
                _nextId = loadedId + 1;
            }
        }

        // Constructeur pour Load (depuis la base de données)
        public static GameState Load( int id, int currentSceneId, int scenarioId, int gold,Inventory playerInventory,  PlayerCharacter playerCharacter, List<string>? flags = null, CombatState? currentCombat = null)
        {
            if (!ValidUtils.CheckIfPositiveNumber(id))
            {
                throw new ArgumentException("id doit être un nombre positif.", nameof(id));
            }

            GameState state = new GameState();

            state.Id = id;
            EnsureNextIdIsAfterLoadedId(id);

            state.ScenarioId = scenarioId;
            state.CurrentSceneId = currentSceneId;
            state.Gold = gold;

            state.PlayerInventory = playerInventory;
            state.PlayerCharacter = playerCharacter;

            state.CurrentCombat = currentCombat;

            if (flags != null)
            {
                state._flags = new List<string>(flags);
            }

            return state;
        }




        public bool HasFlag(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return _flags.Contains(key);
        }

        public void SetFlag(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Le flag ne peut pas être vide.", nameof(key));
            }

            if (!_flags.Contains(key))
            {
                _flags.Add(key);
            }
        }

        public void RemoveFlag(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Le flag ne peut pas être vide.", nameof(key));
            }

            if (_flags.Contains(key))
            {
                _flags.Remove(key);
            }
        }

        public bool CanAfford(int amount)
        {
            if (!ValidUtils.CheckIfPositiveNumber(amount))
            {
                return false;
            }

            return Gold >= amount;
        }

        public void AddGold(int amount)
        {
            if (!ValidUtils.CheckIfPositiveNumber(amount))
            {
                throw new ArgumentException("Le montant d'or à ajouter doit être positif.", nameof(amount));
            }

            Gold = Gold + amount;
        }

        public void SpendGold(int amount)
        {
            if (!ValidUtils.CheckIfPositiveNumber(amount))
            {
                throw new ArgumentException("Le montant d'or à dépenser doit être positif.", nameof(amount));
            }

            if (!CanAfford(amount))
            {
                throw new InvalidOperationException("Or insuffisant pour effectuer cette action.");
            }

            Gold = Gold - amount;
        }

        public void SetGold(int gold)
        {
            if (!ValidUtils.CheckIfNonNegativeNumber(gold))
            {
                throw new ArgumentException("gold doit être >= 0.", nameof(gold));
            }

            Gold = gold;
        }

        public void SetPlayerInventory(Inventory playerInventory)
        {
            if (!ValidUtils.CheckIfNotNull(playerInventory))
            {
                throw new ArgumentNullException(nameof(playerInventory));
            }

            PlayerInventory = playerInventory;
        }

        public void SetPlayerCharacter(PlayerCharacter playerCharacter)
        {
            if (!ValidUtils.CheckIfNotNull(playerCharacter))
            {
                throw new ArgumentNullException(nameof(playerCharacter));
            }

            PlayerCharacter = playerCharacter;
        }


        public void TakeDamage(int damage)
        {
            if (!ValidUtils.CheckIfPositiveNumber(damage))
            {
                return;
            }

            PlayerCharacter.ReceiveDamage(damage);
        }

        public bool TryUsePotion()
        {
            if (PlayerInventory == null)
            {
                throw new InvalidOperationException("L'inventaire du joueur n'est pas initialisé.");
            }

            if (PlayerCharacter == null)
            {
                throw new InvalidOperationException("Le personnage joueur n'est pas initialisé.");
            }

            bool consumed = PlayerInventory.ConsumePotion();
            if (!consumed)
            {
                return false;
            }

            PlayerCharacter.Heal(POTION_HEAL_AMOUNT);
            return true;
        }

        public void GrantLoot(Loot loot)
        {
            if(loot == null)
            {
                throw new ArgumentNullException(nameof(loot));
            }

            loot.Apply(this);
        }

        public void MoveToScene(int sceneId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(sceneId))
            {
                throw new ArgumentException("sceneId doit être un nombre positif.", nameof(sceneId));
            }

            CurrentSceneId = sceneId;
        }

        public void SetScenario(int scenarioId)
        {
            if (!ValidUtils.CheckIfPositiveNumber(scenarioId))
            {
                throw new ArgumentException("scenarioId doit être un nombre positif.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
        }

        public bool IsInCombat()
        {
            return CurrentCombat != null;
        }

        public void StartCombat(CombatState combatState)
        {
            if (combatState == null)
            {
                throw new ArgumentNullException(nameof(combatState));
            }

            CurrentCombat = combatState;
        }

        public void EndCombat()
        {
            CurrentCombat = null;
        }
    }
}
