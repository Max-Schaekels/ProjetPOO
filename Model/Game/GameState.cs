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
            throw new NotImplementedException();
        }

        public void SetFlag(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveFlag(string key)
        {
            throw new NotImplementedException();
        }

        public bool CanAfford(int amount)
        {
            throw new NotImplementedException();
        }

        public void AddGold(int amount)
            {
                throw new NotImplementedException();
        }

        public void SpendGold(int amount)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public bool TryUsePotion()
        {
            throw new NotImplementedException();
        }
         public void GrantLoot(Loot loot)
        {
            throw new NotImplementedException();
        }

        public void MoveToScene(int sceneId)
        {
            throw new NotImplementedException();
        }

        public bool IsInCombat()
        {
            throw new NotImplementedException();
        }
    }
}
