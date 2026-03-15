using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.Randomization;

namespace ProjetPOO.Model.Combat
{
    public class Combat
    {
        private const int MAX_RANDOM_BONUS = 5;
        private const int MIN_RANDOM_BONUS = 0;
        private const int BLOCKING_DAMAGE_REDUCTION = 2;
        private const int NO_DAMAGE = 0;
        public static int CalculateDamage(Character attacker, Character defender, bool isDefenderBlocking)
        {
            bool success;
            int totalDefense;
            int totalAttack;
            int randomAtkBonus = RandomUtils.GetRandomNumber(MIN_RANDOM_BONUS, MAX_RANDOM_BONUS);
            int randomDefBonus = RandomUtils.GetRandomNumber(MIN_RANDOM_BONUS, MAX_RANDOM_BONUS);
            int damage;

            if(attacker == null )
            {
                throw new ArgumentNullException(nameof(attacker));
            }

            if(defender == null)
            {
                throw new ArgumentNullException(nameof(defender));
            }

            totalAttack = attacker.Attack + randomAtkBonus;
            totalDefense = defender.Defense + randomDefBonus;

            success = totalAttack > totalDefense;

            if (success)
            {
                damage = (totalAttack - totalDefense) - (isDefenderBlocking ? BLOCKING_DAMAGE_REDUCTION : NO_DAMAGE);

                return (damage > NO_DAMAGE) ? damage : NO_DAMAGE;
            }
            else
            {
                return NO_DAMAGE;
            }


        }

        public static bool TryFlee(PlayerCharacterInstance player, Enemy enemy)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            int difference = player.Agility - enemy.Agility;

            if(difference <= -1)
            {
                return RandomUtils.CheckChance(10);
            }
            else if(difference == 0)
            {
                return RandomUtils.CheckChance(25);
            }
            else if(difference == 1)
            {
                return RandomUtils.CheckChance(33);
            }
            else if (difference == 2)
            {
                return RandomUtils.CheckChance(50);
            }
            else
            {
                return RandomUtils.CheckChance(75);
            }

        }
    }
}

