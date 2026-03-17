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
        private PlayerCharacterInstance _player;
        private EnemyInstance _enemy;
        private bool _isPlayerTurn;
        private int _turnCount;
        private bool _isDefendingThisTurn;
        private CombatResult _result;

        public PlayerCharacterInstance Player
        {
            get => _player;
            private set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _player = value;
            }
        }

        public EnemyInstance Enemy
        {
            get => _enemy;
            private set
            {
                if (ValidUtils.CheckIfNotNull(value))
                    _enemy = value;
            }
        }

        public bool IsPlayerTurn
        {
            get => _isPlayerTurn;
            private set => _isPlayerTurn = value;
        }

        public int TurnCount
        {
            get => _turnCount;
            private set
            {
                if (ValidUtils.CheckIfNonNegativeNumber(value))
                    _turnCount = value;
            }
        }

        public bool IsDefendingThisTurn
        {
            get => _isDefendingThisTurn;
            private set => _isDefendingThisTurn = value;
        }

        public CombatResult Result
        {
            get => _result;
            private set => _result = value;
        }

        public CombatState(PlayerCharacterInstance player, EnemyInstance enemy)
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
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (Result != CombatResult.InProgress)
            {
                return;
            }

            if (!IsPlayerTurn)
            {
                return;
            }

            if (action == CombatActionType.Attack)
            {
                int damage = Combat.CalculateDamage(Player, Enemy, false);
                Enemy.ReceiveDamage(damage);
            }
            else if (action == CombatActionType.Defend)
            {
                IsDefendingThisTurn = true;
            }
            else if (action == CombatActionType.UseItem)
            {
                bool used = state.TryUsePotion();

                if (!used)
                {
                    throw new InvalidOperationException("Le joueur ne possède aucune potion.");
                }
            }
            else if (action == CombatActionType.Flee)
            {
                bool fled = Combat.TryFlee(Player, Enemy);
                if (fled)
                {
                    Result = CombatResult.Fled;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(action), "Action de combat inconnue.");
            }

            CheckEnd();

            if (Result == CombatResult.InProgress)
            {
                EndTurn();
            }
        }

        public void ResolveEnemyAction(GameState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (Result != CombatResult.InProgress)
            {
                return;
            }

            if (IsPlayerTurn)
            {
                return;
            }

            int damage = Combat.CalculateDamage(Enemy, Player, IsDefendingThisTurn);
            Player.ReceiveDamage(damage);

            ClearDefense();

            CheckEnd();

            if (Result == CombatResult.InProgress)
            {
                EndTurn();
            }
        }

        public void CheckEnd()
        {
            if (Result != CombatResult.InProgress)
            {
                return;
            }

            if (!Player.IsAlive())
            {
                Result = CombatResult.Defeat;
                return;
            }

            if (!Enemy.IsAlive())
            {
                Result = CombatResult.Victory;
                return;
            }
        }


        public void ClearDefense()
        {
            IsDefendingThisTurn = false;
        }

        public void EndTurn()
        {
            if(IsPlayerTurn)
            {
                TurnCount++;
            } 
            IsPlayerTurn = !IsPlayerTurn;

        }

    }
}
