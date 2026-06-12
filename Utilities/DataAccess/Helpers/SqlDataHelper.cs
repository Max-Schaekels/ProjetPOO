using Microsoft.Data.SqlClient;
using ProjetPOO.Model.Story.Enums;


namespace ProjetPOO.Utilities.DataAccess.Helpers
{
    /// <summary>
    /// Fournit des méthodes utilitaires pour lire les valeurs provenant d'un SqlDataReader
    /// et convertir certaines valeurs du modèle vers un format compatible avec la base de données.
    /// Cette classe centralise notamment la gestion des valeurs nulles et la conversion des enums métier.
    /// </summary>
    public static class SqlDataHelper
    {
        /// <summary>
        /// Lit une valeur entière obligatoire depuis une colonne du SqlDataReader.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Valeur entière contenue dans la colonne.</returns>
        public static int ReadInt(SqlDataReader reader, string columnName)
        {
            return Convert.ToInt32(reader[columnName]);
        }

        /// <summary>
        /// Lit un identifiant nullable depuis une colonne du SqlDataReader.
        /// Si la colonne contient NULL en base de données, la méthode retourne 0 afin de représenter l'absence d'identifiant côté modèle.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Identifiant lu depuis la base de données, ou 0 si la valeur SQL est NULL.</returns>
        public static int ReadNullableId(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        /// <summary>
        /// Lit une valeur entière nullable depuis une colonne du SqlDataReader.
        /// Si la colonne contient NULL en base de données, la méthode retourne null côté C#.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Valeur entière lue depuis la base de données, ou null si la valeur SQL est NULL.</returns>
        public static int? ReadNullableInt(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        /// <summary>
        /// Lit une chaîne de caractères obligatoire depuis une colonne du SqlDataReader.
        /// Si la conversion retourne null, une chaîne vide est retournée afin d'éviter une valeur nulle côté modèle.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Chaîne de caractères lue depuis la colonne, ou une chaîne vide si aucune valeur exploitable n'est trouvée.</returns>
        public static string ReadString(SqlDataReader reader, string columnName)
        {
            return Convert.ToString(reader[columnName]) ?? string.Empty;
        }

        /// <summary>
        /// Lit une chaîne de caractères nullable depuis une colonne du SqlDataReader.
        /// Si la colonne contient NULL en base de données, la méthode retourne une chaîne vide.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Chaîne de caractères lue depuis la base de données, ou une chaîne vide si la valeur SQL est NULL.</returns>
        public static string ReadNullableString(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(reader[columnName]) ?? string.Empty;
        }

        /// <summary>
        /// Lit une chaîne de caractères optionnelle depuis une colonne du SqlDataReader.
        /// Si la colonne contient NULL en base de données, la méthode retourne null côté C#.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne à lire.</param>
        /// <returns>Chaîne de caractères lue depuis la base de données, ou null si la valeur SQL est NULL.</returns>
        public static string? ReadOptionalString(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(reader[columnName]);
        }

        /// <summary>
        /// Convertit un identifiant du modèle vers une valeur compatible avec la base de données.
        /// La valeur 0 est considérée comme une absence d'identifiant et est convertie en DBNull.Value.
        /// </summary>
        /// <param name="id">Identifiant à convertir.</param>
        /// <returns>Identifiant fourni, ou DBNull.Value si l'identifiant vaut 0.</returns>
        public static object ToDbNullableId(int id)
        {
            if (id == 0)
            {
                return DBNull.Value;
            }

            return id;
        }

        /// <summary>
        /// Convertit un entier nullable C# vers une valeur compatible avec la base de données.
        /// Si la valeur est null, la méthode retourne DBNull.Value.
        /// </summary>
        /// <param name="value">Valeur entière nullable à convertir.</param>
        /// <returns>Valeur entière fournie, ou DBNull.Value si la valeur est null.</returns>
        public static object ToDbNullableInt(int? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value.Value;
        }

        /// <summary>
        /// Convertit une chaîne optionnelle vers une valeur compatible avec la base de données.
        /// Si la chaîne est null, vide ou composée uniquement d'espaces, la méthode retourne DBNull.Value.
        /// </summary>
        /// <param name="value">Chaîne de caractères optionnelle à convertir.</param>
        /// <returns>Chaîne fournie, ou DBNull.Value si aucune valeur exploitable n'est présente.</returns>
        public static object ToDbNullableString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// Lit l'identifiant SQL d'un type de scène et le convertit en enum SceneType.
        /// Cette conversion permet de faire le lien entre les valeurs stockées en base de données et les valeurs utilisées dans le modèle.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne contenant l'identifiant du type de scène.</param>
        /// <returns>Valeur SceneType correspondant à l'identifiant SQL.</returns>
        /// <exception cref="InvalidOperationException">Lancée si l'identifiant SQL ne correspond à aucun SceneType connu.</exception>
        public static SceneType ReadSceneType(SqlDataReader reader, string columnName)
        {
            int sceneTypeId = ReadInt(reader, columnName);

            switch (sceneTypeId)
            {
                case 1:
                    return SceneType.Normal;

                case 2:
                    return SceneType.Combat;

                case 3:
                    return SceneType.Shop;

                case 4:
                    return SceneType.End;

                default:
                    throw new InvalidOperationException("SceneTypeId inconnu : " + sceneTypeId);
            }
        }

        /// <summary>
        /// Convertit une valeur SceneType du modèle en identifiant utilisé par la base de données.
        /// </summary>
        /// <param name="sceneType">Type de scène à convertir.</param>
        /// <returns>Identifiant SQL correspondant au type de scène.</returns>
        /// <exception cref="InvalidOperationException">Lancée si le SceneType ne correspond à aucune valeur connue.</exception>
        public static int ToDbSceneTypeId(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.Normal:
                    return 1;

                case SceneType.Combat:
                    return 2;

                case SceneType.Shop:
                    return 3;

                case SceneType.End:
                    return 4;

                default:
                    throw new InvalidOperationException("SceneType inconnu : " + sceneType);
            }
        }

        /// <summary>
        /// Lit l'identifiant SQL d'un type de condition et le convertit en enum ConditionType.
        /// Cette conversion permet de reconstruire les conditions métier à partir des valeurs stockées en base de données.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne contenant l'identifiant du type de condition.</param>
        /// <returns>Valeur ConditionType correspondant à l'identifiant SQL.</returns>
        /// <exception cref="InvalidOperationException">Lancée si l'identifiant SQL ne correspond à aucun ConditionType connu.</exception>
        public static ConditionType ReadConditionType(SqlDataReader reader, string columnName)
        {
            int conditionTypeId = ReadInt(reader, columnName);

            switch (conditionTypeId)
            {
                case 1:
                    return ConditionType.HasKey;

                case 2:
                    return ConditionType.HasPotion;

                case 3:
                    return ConditionType.MinGold;

                default:
                    throw new InvalidOperationException("ConditionTypeId inconnu : " + conditionTypeId);
            }
        }

        /// <summary>
        /// Convertit une valeur ConditionType du modèle en identifiant utilisé par la base de données.
        /// </summary>
        /// <param name="conditionType">Type de condition à convertir.</param>
        /// <returns>Identifiant SQL correspondant au type de condition.</returns>
        /// <exception cref="InvalidOperationException">Lancée si le ConditionType ne correspond à aucune valeur connue.</exception>
        public static int ToDbConditionTypeId(ConditionType conditionType)
        {
            switch (conditionType)
            {
                case ConditionType.HasKey:
                    return 1;

                case ConditionType.HasPotion:
                    return 2;

                case ConditionType.MinGold:
                    return 3;

                default:
                    throw new InvalidOperationException("ConditionType inconnu : " + conditionType);
            }
        }

        /// <summary>
        /// Lit l'identifiant SQL d'un type d'effet et le convertit en enum EffectType.
        /// Cette conversion permet de reconstruire les effets métier à partir des valeurs stockées en base de données.
        /// </summary>
        /// <param name="reader">SqlDataReader contenant la ligne lue depuis la base de données.</param>
        /// <param name="columnName">Nom de la colonne contenant l'identifiant du type d'effet.</param>
        /// <returns>Valeur EffectType correspondant à l'identifiant SQL.</returns>
        /// <exception cref="InvalidOperationException">Lancée si l'identifiant SQL ne correspond à aucun EffectType connu.</exception>
        public static EffectType ReadEffectType(SqlDataReader reader, string columnName)
        {
            int effectTypeId = ReadInt(reader, columnName);

            switch (effectTypeId)
            {
                case 1:
                    return EffectType.AddGold;

                case 2:
                    return EffectType.RemoveGold;

                case 3:
                    return EffectType.Damage;

                case 4:
                    return EffectType.AddPotion;

                case 5:
                    return EffectType.RemovePotion;

                case 6:
                    return EffectType.AddKey;

                case 7:
                    return EffectType.RemoveKey;

                case 8:
                    return EffectType.SetFlag;

                case 9:
                    return EffectType.RemoveFlag;

                default:
                    throw new InvalidOperationException("EffectTypeId inconnu : " + effectTypeId);
            }
        }

        /// <summary>
        /// Convertit une valeur EffectType du modèle en identifiant utilisé par la base de données.
        /// </summary>
        /// <param name="effectType">Type d'effet à convertir.</param>
        /// <returns>Identifiant SQL correspondant au type d'effet.</returns>
        /// <exception cref="InvalidOperationException">Lancée si le EffectType ne correspond à aucune valeur connue.</exception>
        public static int ToDbEffectTypeId(EffectType effectType)
        {
            switch (effectType)
            {
                case EffectType.AddGold:
                    return 1;

                case EffectType.RemoveGold:
                    return 2;

                case EffectType.Damage:
                    return 3;

                case EffectType.AddPotion:
                    return 4;

                case EffectType.RemovePotion:
                    return 5;

                case EffectType.AddKey:
                    return 6;

                case EffectType.RemoveKey:
                    return 7;

                case EffectType.SetFlag:
                    return 8;

                case EffectType.RemoveFlag:
                    return 9;

                default:
                    throw new InvalidOperationException("EffectType inconnu : " + effectType);
            }
        }
    }
}
