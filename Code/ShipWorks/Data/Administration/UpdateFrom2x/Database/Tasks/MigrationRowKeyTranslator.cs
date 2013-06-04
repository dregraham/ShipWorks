using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// Translates a V2 PK to the new V3 PK
    /// </summary>
    public static class MigrationRowKeyTranslator
    {
        /// <summary>
        /// Translate the given v2 key to the new v3 key
        /// </summary>
        public static long TranslateKeyToV3(int originalKey, MigrationRowKeyType keyType, SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "SELECT NewKey FROM v2m_MigrationKeys WHERE OriginalKey = @original AND KeyTypeCode = @type";
            cmd.Parameters.AddWithValue("@original", originalKey);
            cmd.Parameters.AddWithValue("@type", (int) keyType);

            object result = cmd.ExecuteScalar();
            if (result == null || result is DBNull)
            {
                throw new NotFoundException(string.Format("Could not translate v2 key {0}.{1}", originalKey, keyType));
            }

            return (long) result;
        }

        /// <summary>
        /// Translate the given V3 key to what it was in V2
        /// </summary>
        public static int TranslateKeyToV2(long key, SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "SELECT OriginalKey FROM v2m_MigrationKeys WHERE NewKey = @newKey";
            cmd.Parameters.AddWithValue("@NewKey", key);

            object result = cmd.ExecuteScalar();
            if (result == null || result is DBNull)
            {
                throw new NotFoundException(string.Format("Could not translate v3 key {0}", key));
            }

            return (int) result;
        }
    }
}
