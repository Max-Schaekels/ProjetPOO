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
        public static void ExecuteNonQuery(SqlConnection sqlConnection, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        public static void ExecuteNonQuery(SqlConnection sqlConnection, SqlTransaction transaction, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection, transaction))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

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
