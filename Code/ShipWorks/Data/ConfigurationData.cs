using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Logon;
using System.Threading;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provices access to the global configuration object
    /// </summary>
    public static class ConfigurationData
    {
        static ConfigurationEntity config;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            config = new ConfigurationEntity(true);
            SqlAdapter.Default.FetchEntity(config);

            needCheckForChanges = false;
        }

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            needCheckForChanges = true;
        }

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public static ConfigurationEntity Fetch()
        {
            if (needCheckForChanges)
            {
                SqlAdapter.Default.FetchEntity(config);
                needCheckForChanges = false;
            }

            return EntityUtility.CloneEntity(config);
        }

        /// <summary>
        /// Save the given entity as the current configuration
        /// </summary>
        public static void Save(ConfigurationEntity configuration)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(configuration);

                Interlocked.Exchange(ref config, EntityUtility.CloneEntity(configuration));
            }

            needCheckForChanges = false;
        }

        /// <summary>
        /// Create the single instance of this row as apart of creating a new shipworks database
        /// </summary>
        public static void CreateInstance(SqlAdapter adapter)
        {
            ConfigurationEntity config = new ConfigurationEntity();
            config.ConfigurationID = true;

            config.LogOnMethod = (int) LogonMethod.SelectUsername;
            config.AddressCasing = true;

            config.CustomerCompareEmail = true;
            config.CustomerCompareAddress = false;

            config.CustomerUpdateBilling = true;
            config.CustomerUpdateShipping = true;

            config.CustomerUpdateModifiedBilling = (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy;
            config.CustomerUpdateModifiedShipping = (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy;

            config.AuditNewOrders = false;
            config.AuditDeletedOrders = false;

            config.CustomerKey = string.Empty;

            adapter.SaveEntity(config);
        }
    }
}
