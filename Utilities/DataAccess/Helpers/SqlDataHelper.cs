using Microsoft.Data.SqlClient;
using ProjetPOO.Model.Story.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.Helpers
{
    public static class SqlDataHelper
    {
        public static int ReadInt(SqlDataReader reader, string columnName)
        {
            return Convert.ToInt32(reader[columnName]);
        }

        public static int ReadNullableId(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        public static int? ReadNullableInt(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        public static string ReadString(SqlDataReader reader, string columnName)
        {
            return Convert.ToString(reader[columnName]) ?? string.Empty;
        }

        public static string ReadNullableString(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return string.Empty;
            }

            return Convert.ToString(reader[columnName]) ?? string.Empty;
        }

        public static string? ReadOptionalString(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == DBNull.Value)
            {
                return null;
            }

            return Convert.ToString(reader[columnName]);
        }

        public static object ToDbNullableId(int id)
        {
            if (id == 0)
            {
                return DBNull.Value;
            }

            return id;
        }

        public static object ToDbNullableInt(int? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value.Value;
        }

        public static object ToDbNullableString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return DBNull.Value;
            }

            return value;
        }

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
