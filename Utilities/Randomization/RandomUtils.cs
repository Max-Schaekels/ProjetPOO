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

        /// <summary>
        /// Génère un nombre aléatoire compris entre les limites minimum et maximum fournies.
        /// Les deux limites sont incluses dans les valeurs possibles.
        /// </summary>
        /// <param name="min">Valeur minimale possible.</param>
        /// <param name="max">Valeur maximale possible.</param>
        /// <returns>Nombre aléatoire compris entre min et max inclus.</returns>
        /// <exception cref="ArgumentException">Lancée si min est supérieur à max.</exception>
        public static int GetRandomNumber(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("min doit être inférieur ou égal à max.");
            }

            return _random.Next(min, max + 1);
        }

        /// <summary>
        /// Génère un nombre aléatoire entre 0 et 99.
        /// Cette méthode est utilisée comme base pour les tests de probabilité en pourcentage.
        /// </summary>
        /// <returns>Nombre aléatoire compris entre 0 inclus et 100 exclus.</returns>
        public static int GetPercentage()
        {
            return _random.Next(0, 100); 
        }

        /// <summary>
        /// Vérifie si un événement aléatoire se produit selon un pourcentage de chance donné.
        /// Par exemple, une valeur de 25 représente environ 25% de chances de réussite.
        /// </summary>
        /// <param name="percentage">Pourcentage de chance de réussite, compris entre 0 et 100.</param>
        /// <returns>True si le test de chance réussit, sinon false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lancée si le pourcentage est inférieur à 0 ou supérieur à 100.</exception>
        public static bool CheckChance(int percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Le pourcentage doit être compris entre 0 et 100.");
            }

            return GetPercentage() < percentage;
        }
    }
}
