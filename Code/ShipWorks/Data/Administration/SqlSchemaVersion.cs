using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Gives us the installed schema version
    /// </summary>
    public class SqlSchemaVersion : ISqlSchemaVersion
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database using the current open connection
        /// </summary>
        public Version GetInstalledSchemaVersion()
        {
            SqlConnection con = SqlSession.Current.OpenConnection();
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            return new Version((string)SqlCommandProvider.ExecuteScalar(cmd));
        }

        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns>true if version is 4.8.0.0 or better</returns>
        public bool IsCustomerLicenseSupported()
        {
            return (GetInstalledSchemaVersion() >= Version.Parse("4.8.0.0"));
        }
    }
}