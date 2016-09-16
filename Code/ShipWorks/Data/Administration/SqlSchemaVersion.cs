using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Gives us the installed schema version
    /// </summary>
    public class SqlSchemaVersion : ISqlSchemaVersion, IInitializeForCurrentDatabase
    {
        Version minimumVersion = Version.Parse("4.8.0.0");
        Lazy<bool> isCustomerLicenseSupported;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlSchemaVersion()
        {
            isCustomerLicenseSupported = new Lazy<bool>(IsInstalledSchemaVersionGreaterThanMinimum);
        }

        /// <summary>
        /// Initialize for the current database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode)
        {
            isCustomerLicenseSupported = new Lazy<bool>(IsInstalledSchemaVersionGreaterThanMinimum);
        }

        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns>true if version is 4.8.0.0 or better</returns>
        public bool IsCustomerLicenseSupported() => isCustomerLicenseSupported.Value;

        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns>true if version is 4.8.0.0 or better</returns>
        private bool IsInstalledSchemaVersionGreaterThanMinimum()
        {
            return GetInstalledSchemaVersion() >= minimumVersion;
        }

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

                return new Version((string) SqlCommandProvider.ExecuteScalar(cmd));
            }
        }
    }
}