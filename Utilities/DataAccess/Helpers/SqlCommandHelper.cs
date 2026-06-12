using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.Helpers
{
    public static class SqlCommandHelper
    {
        /// <summary>
        /// Exécute une requête SQL qui ne retourne pas de résultat.
        /// Cette surcharge est utilisée pour les commandes exécutées sans transaction, comme certains INSERT, UPDATE ou DELETE simples.
        /// </summary>
        /// <param name="sqlConnection">Connexion SQL utilisée pour exécuter la requête.</param>
        /// <param name="query">Requête SQL à exécuter.</param>
        /// <param name="parameters">Paramètres SQL à ajouter à la commande.</param>
        public static void ExecuteNonQuery(SqlConnection sqlConnection, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Exécute une requête SQL qui ne retourne pas de résultat dans une transaction existante.
        /// Cette surcharge permet d'intégrer la commande dans un ensemble d'opérations devant être validées ou annulées ensemble.
        /// </summary>
        /// <param name="sqlConnection">Connexion SQL utilisée pour exécuter la requête.</param>
        /// <param name="transaction">Transaction SQL dans laquelle la commande doit être exécutée.</param>
        /// <param name="query">Requête SQL à exécuter.</param>
        /// <param name="parameters">Paramètres SQL à ajouter à la commande.</param>
        public static void ExecuteNonQuery(SqlConnection sqlConnection, SqlTransaction transaction, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection, transaction))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Exécute une requête SQL qui retourne une seule valeur et la convertit en entier.
        /// Cette méthode est principalement utilisée après une insertion pour récupérer l'identifiant généré.
        /// </summary>
        /// <param name="sqlConnection">Connexion SQL utilisée pour exécuter la requête.</param>
        /// <param name="query">Requête SQL à exécuter.</param>
        /// <param name="parameters">Paramètres SQL à ajouter à la commande.</param>
        /// <returns>Valeur entière retournée par la requête SQL.</returns>
        /// <exception cref="InvalidOperationException">Lancée si la requête ne retourne aucune valeur.</exception>
        public static int ExecuteScalarInt(SqlConnection sqlConnection, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddRange(parameters);

                object? result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("La requête SQL n'a retourné aucun identifiant.");
                }

                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Exécute une requête SQL qui retourne une seule valeur et la convertit en entier dans une transaction existante.
        /// Cette méthode est principalement utilisée après une insertion transactionnelle pour récupérer l'identifiant généré.
        /// </summary>
        /// <param name="sqlConnection">Connexion SQL utilisée pour exécuter la requête.</param>
        /// <param name="transaction">Transaction SQL dans laquelle la commande doit être exécutée.</param>
        /// <param name="query">Requête SQL à exécuter.</param>
        /// <param name="parameters">Paramètres SQL à ajouter à la commande.</param>
        /// <returns>Valeur entière retournée par la requête SQL.</returns>
        /// <exception cref="InvalidOperationException">Lancée si la requête ne retourne aucune valeur.</exception>
        public static int ExecuteScalarInt(SqlConnection sqlConnection, SqlTransaction transaction, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection, transaction))
            {
                command.Parameters.AddRange(parameters);

                object? result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException("La requête SQL n'a retourné aucun identifiant.");
                }

                return Convert.ToInt32(result);
            }
        }
    }
}
