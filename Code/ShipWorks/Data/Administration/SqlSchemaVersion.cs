using System;
using System.Data;
using System.Data.Common;
using System.Transactions;
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
        /// Get the schema version of the ShipWorks database using the current open connection
        /// </summary>
        public Version GetInstalledSchemaVersion()
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    using (DbCommand cmd = DbCommandProvider.Create(con))
                    {
                        cmd.CommandText = "GetSchemaVersion";
                        cmd.CommandType = CommandType.StoredProcedure;

                        return new Version((string) DbCommandProvider.ExecuteScalar(cmd));
                    }
                }
            }
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