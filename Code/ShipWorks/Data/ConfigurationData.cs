using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Logon;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
    public static class ConfigurationData
    {
        private static readonly Func<ISqlAdapter> defaultGetSqlAdapter = () => SqlAdapter.Default;
        private static readonly object lockObj = new object();
        private static ConfigurationEntity config;
        private static IConfigurationEntity configReadOnly;
        private static bool needCheckForChanges;
        private static string customerKey;
        private static string legacyCustomerKey;
        private static readonly Version archiveVersion = new Version(5, 23, 1, 6);

        /// <summary>
        /// Which version was archive functionality introduced
        /// </summary>
        public static Version ArchiveVersion => archiveVersion;

        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            customerKey = string.Empty;
            UpdateConfiguration(defaultGetSqlAdapter);
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
        public static ConfigurationEntity Fetch() =>
            Fetch(defaultGetSqlAdapter);

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public static ConfigurationEntity Fetch(Func<ISqlAdapter> getSqlAdapter)
        {
            lock (lockObj)
            {
                if (needCheckForChanges)
                {
                    UpdateConfiguration(getSqlAdapter);
                }

                return EntityUtility.CloneEntity(config);
            }
        }

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public static IConfigurationEntity FetchReadOnly() =>
            FetchReadOnly(defaultGetSqlAdapter);

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public static IConfigurationEntity FetchReadOnly(Func<ISqlAdapter> getSqlAdapter)
        {
            lock (lockObj)
            {
                if (needCheckForChanges)
                {
                    UpdateConfiguration(getSqlAdapter);
                }

                return configReadOnly;
            }
        }

        /// <summary>
        /// Gets the CustomerKey
        /// </summary>
        public static string FetchCustomerKey(CustomerLicenseKeyType licenseKeyType)
        {
            lock (lockObj)
            {
                if (licenseKeyType == CustomerLicenseKeyType.Legacy)
                {
                    if (string.IsNullOrEmpty(legacyCustomerKey))
                    {
                        legacyCustomerKey = GetConfigurationField(ConfigurationFields.LegacyCustomerKey, "LegacyCustomerKey");
                    }

                    return legacyCustomerKey;
                }

                if (string.IsNullOrEmpty(customerKey))
                {
                    customerKey = GetConfigurationField(ConfigurationFields.CustomerKey, "CustomerKey");
                }

                return customerKey;
            }
        }

        /// <summary>
        /// Get a field value from the configuration table
        /// </summary>
        private static string GetConfigurationField(IEntityFieldCore entityField, string fieldName)
        {
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(entityField, 0, fieldName, "");
            
            using (var sqlAdapter = defaultGetSqlAdapter())
            {
                string fieldValue = string.Empty;
                IDataReader reader = sqlAdapter.FetchDataReader(resultFields, null, CommandBehavior.CloseConnection, 0,
                    null, true);
                
                if (reader.Read())
                {
                    fieldValue = reader.GetString(0).Trim();
                }

                return fieldValue;
            }
        }

        /// <summary>
        /// Update the configuration stored in memory
        /// </summary>
        private static void UpdateConfiguration(Func<ISqlAdapter> getSqlAdapter)
        {
            lock (lockObj)
            {
                ConfigurationEntity newConfig = new ConfigurationEntity(true);
                getSqlAdapter().FetchEntity(newConfig);
                config = newConfig;
                configReadOnly = newConfig.AsReadOnly();
                customerKey = newConfig.CustomerKey;
                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Save the given entity as the current configuration
        /// </summary>
        public static void Save(ConfigurationEntity configuration)
        {
            lock (lockObj)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(configuration);

                    config = EntityUtility.CloneEntity(configuration);
                    configReadOnly = config.AsReadOnly();
                }

                customerKey = configuration.CustomerKey;
                needCheckForChanges = false;
            }
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
                AllowEbayCombineLocally = false,

                AutoUpdateDayOfWeek = DayOfWeek.Thursday,
                AutoUpdateHourOfDay = 23,
                AutoUpdateStartDate = DateTime.UtcNow.AddDays(-1)
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
                    ExcludeIncludeFieldsList includeFieldsList = new ExcludeIncludeFieldsList(false, new[] { ConfigurationFields.ArchivalSettingsXml });
                    sqlAdapter.FetchEntity(configurationEntity, null, null, includeFieldsList);
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
