using System;
using System.Data.SqlClient;

namespace ShipWorks.Data.Administration
{
    public interface ISqlSchemaVersion
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database using the current connection
        /// </summary>
        Version GetInstalledSchemaVersion();

        /// <summary>
        /// Get the schema version of the ShipWorks database using the given connection
        /// </summary>
        Version GetInstalledSchemaVersion(SqlConnection con);
    }
}