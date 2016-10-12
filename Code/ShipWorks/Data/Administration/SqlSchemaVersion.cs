using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Gives us the installed schema version
    /// </summary>
    [Component]
    public class SqlSchemaVersion : ISqlSchemaVersion
    {
        private Version versionFourEight = Version.Parse("4.8.0.0");
        private ILog log;

        public SqlSchemaVersion(Func<Type, ILog> getLog)
        {
            log = getLog(GetType());
        }

        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns>true if version is 4.8.0.0 or better</returns>
        public bool IsCustomerLicenseSupported() =>
            GetInstalledSchemaVersion() >= versionFourEight;

        /// <summary>
        /// Get the schema version of the ShipWorks database using the current open connection
        /// </summary>
        private Version GetInstalledSchemaVersion()
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                SqlConnection con = SqlSession.Current.OpenConnection();
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = "GetSchemaVersion";
                cmd.CommandType = CommandType.StoredProcedure;

                string version = SqlCommandProvider.ExecuteScalar(cmd) as string;
                log.Info($"Got installed schema version: {version}");

                return new Version(version);
            }
        }
    }
}