using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Model.Game;
using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class CombatState
    {
        private PlayerCharacter _player;
        private Enemy _enemy;
        private bool _isPlayerTurn;
        private int _turnCount;
        private bool _isDefendingThisTurn;
        private CombatResult _result;

        public PlayerCharacter Player
        {
            get => _player;
            set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _player = value;
            }
        }

        public Enemy Enemy
        {
            get => _enemy;
            set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _enemy = value;
            }
        }

        public bool IsPlayerTurn
        {
            get => _isPlayerTurn;
            set => _isPlayerTurn = value;
        }

        public int TurnCount
        {
            get => _turnCount;
            set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _turnCount = value;
            }
        }

        public bool IsDefendingThisTurn
        {
            get => _isDefendingThisTurn;
            set => _isDefendingThisTurn = value;
        }

        public CombatResult Result
        {
            get => _result;
            private set => _result = value;
        }

        public CombatState(PlayerCharacter player, Enemy enemy)
        {
            Player = player;
            Enemy = enemy;


            IsPlayerTurn = true;
            TurnCount = 0;
            IsDefendingThisTurn = false;
            Result = CombatResult.InProgress;
        }


        public void ResolvePlayerAction(CombatActionType action, GameState state)
        {
            throw new NotImplementedException();
        }

        public void ResolveEnemyAction(GameState state)
        {
            throw new NotImplementedException();
        }

        public void CheckEnd()
        {
            throw new NotImplementedException();
        }


        public void ClearDefense()
        {
            throw new NotImplementedException();
        }

        public void EndTurn()
        {
            throw new NotImplementedException();

        }

    }
}
