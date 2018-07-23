using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Logon;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
    public static class ConfigurationData
    {
        static ConfigurationEntity config;
        static IConfigurationEntity configReadOnly;
        static bool needCheckForChanges;

        private static Version archiveVersion = new Version(5, 23, 1, 6);

        /// <summary>
        /// Which version was archive functionality introduced
        /// </summary>
        public static Version ArchiveVersion => archiveVersion;

        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            UpdateConfiguration();
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
                UpdateConfiguration();
            }

            return EntityUtility.CloneEntity(config);
        }

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public static IConfigurationEntity FetchReadOnly()
        {
            if (needCheckForChanges)
            {
                UpdateConfiguration();
            }

            return configReadOnly;
        }

        /// <summary>
        /// Update the configuration stored in memory
        /// </summary>
        private static void UpdateConfiguration()
        {
            ConfigurationEntity newConfig = new ConfigurationEntity(true);
            SqlAdapter.Default.FetchEntity(newConfig);
            config = newConfig;
            configReadOnly = newConfig.AsReadOnly();
            needCheckForChanges = false;
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
            ConfigurationEntity newConfig = new ConfigurationEntity()
            {
                ConfigurationID = true,

                LogOnMethod = (int) LogonMethod.SelectUsername,
                AddressCasing = true,

                CustomerCompareEmail = true,
                CustomerCompareAddress = false,

                CustomerUpdateBilling = true,
                CustomerUpdateShipping = true,

                CustomerUpdateModifiedBilling = (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy,
                CustomerUpdateModifiedShipping = (int) ModifiedOrderCustomerUpdateBehavior.NeverCopy,

                AuditNewOrders = false,
                AuditDeletedOrders = false,

                CustomerKey = string.Empty,

                UseParallelActionQueue = true,
                AllowEbayCombineLocally = false
            };

            adapter.SaveEntity(newConfig);
        }

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        public static bool IsArchive(DbConnection connection)
        {
            string archivalSettingsXml = string.Empty;

            using (ISqlAdapter sqlAdapter = new SqlAdapter(connection))
            {
                try
                {
                    if (SqlSchemaUpdater.GetInstalledSchemaVersion(connection) < ArchiveVersion)
                    {
                        return false;
                    }

                    ConfigurationEntity configurationEntity = new ConfigurationEntity(true);
                    sqlAdapter.FetchEntity(configurationEntity);
                    archivalSettingsXml = configurationEntity.ArchivalSettingsXml;
                }
                catch
                {
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(archivalSettingsXml))
            {
                return false;
            }

            try
            {
                return XDocument.Parse(archivalSettingsXml)?.Root?.HasElements == true;
            }
            catch (XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get telemetry data for configuration
        /// </summary>
        internal static IEnumerable<KeyValuePair<string, string>> GetTelemetryData() =>
            new[]
            {
                Functional.Using(SqlSession.Current.OpenConnection(),
                    connection => new KeyValuePair<string, string>("Database.IsArchive", IsArchive(connection).ToString())),
                new KeyValuePair<string, string>("Auditing.Enabled", FetchReadOnly().AuditEnabled ? "True" : "False")
            };
    }
}
