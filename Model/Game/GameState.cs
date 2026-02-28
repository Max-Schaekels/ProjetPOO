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
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }

        public int CurrentSceneId
        {
            get => _currentSceneId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _currentSceneId = value;
            }
        }

        public int ScenarioId
        {
            get => _scenarioId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _scenarioId = value;
            }
        }


        public int Gold
        {
            get => _gold;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _gold = value;
            }
        }

        public Inventory PlayerInventory
        {
            get => _playerInventory;
            set
            {
                if(ValidUtils.CheckIfNotNull(value))
                    _playerInventory = value;
            }
        }

        public PlayerCharacter PlayerCharacter
        {
            get => _playerCharacter;
            set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _playerCharacter = value;
            }
        }  

        public CombatState? CurrentCombat
        {
            get => _currentCombat;
            set => _currentCombat = value;
        }

        public IReadOnlyList<string> Flags => _flags.AsReadOnly();



        public GameState(int id, int currentSceneId, int scenarioId, int gold, Inventory playerInventory, PlayerCharacter playerCharacter)
        {
            Id = id;
            CurrentSceneId = currentSceneId;
            ScenarioId = scenarioId;
            Gold = gold;
            PlayerInventory = playerInventory;
            PlayerCharacter = playerCharacter;
            _flags = new List<string>();
        }

        public GameState( int currentSceneId, int scenarioId, int gold, Inventory playerInventory, PlayerCharacter playerCharacter)
        {
            CurrentSceneId = currentSceneId;
            ScenarioId = scenarioId;
            Gold = gold;
            PlayerInventory = playerInventory;
            PlayerCharacter = playerCharacter;
            _flags = new List<string>();
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

        public bool IsInCombat()
        {
            return CurrentCombat != null;
        }
    }
}
