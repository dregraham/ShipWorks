using System;
using System.Data;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Gives us the installed schema version
    /// </summary>
    public class SqlSchemaVersion : ISqlSchemaVersion
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database on the given connection
        /// </summary>
        public Version GetInstalledSchemaVersion(SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            return new Version((string)SqlCommandProvider.ExecuteScalar(cmd));
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database using the current open connection
        /// </summary>
        public Version GetInstalledSchemaVersion()
        {
            return GetInstalledSchemaVersion(SqlSession.Current.OpenConnection());
        }
    }
}