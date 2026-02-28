using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.Randomization
{
    public static class RandomUtils
    {
        private static readonly Random _random = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("min doit être inférieur ou égal à max.");
            }

            return _random.Next(min, max + 1);
        }

        public static int GetPercentage()
        {
            return _random.Next(0, 101); // 0 à 100 inclus
        }

        public static bool CheckChance(int percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Le pourcentage doit être compris entre 0 et 100.");
            }

            return GetPercentage() <= percentage;
        }
    }
}
