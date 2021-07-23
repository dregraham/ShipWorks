using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.UI;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
    [Component]
    public class ConfigurationDataWrapper : IInitializeForCurrentDatabase, IConfigurationData
    {
        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ConfigurationData.InitializeForCurrentDatabase();

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public IConfigurationEntity FetchReadOnly() =>
            ConfigurationData.FetchReadOnly();

        /// <summary>
        /// Get the customer key
        /// </summary>
        public string FetchCustomerKey(CustomerLicenseKeyType licenseKeyType) => ConfigurationData.FetchCustomerKey(licenseKeyType).Trim();

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        public void CheckForChangesNeeded() =>
            ConfigurationData.CheckForChangesNeeded();

        /// <summary>
        /// Gets the ActionQueueType for this process based on if it's background or not
        /// </summary>
        public ActionQueueType ExecutionModeActionQueueType
        {
            get
            {
                ActionQueueType actionQueueType = ActionQueueType.Scheduled;

                if (Program.ExecutionMode.IsUIDisplayed)
                {
                    actionQueueType = ActionQueueType.UserInterface;
                }

                return actionQueueType;
            }
        }

        /// <summary>
        /// Should UI actions be included.  If the UI isn't running somewhere,
        /// and we are the background process, go ahead and do UI actions too since it's not open
        /// </summary>
        public bool IncludeUserInterfaceActions => !Program.ExecutionMode.IsUISupported &&
            !(Program.ExecutionMode is UserInterfaceExecutionMode || SingleInstance.IsAlreadyRunning);

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        public bool IsArchive(DbConnection connection) => ConfigurationData.IsArchive(connection);

        /// <summary>
        /// Are we currently in an archive database?
        /// </summary>
        public bool IsArchive()
        {
            var configurationEntity = ConfigurationData.FetchReadOnly();

            try
            {
                return XDocument.Parse(configurationEntity.ArchivalSettingsXml)?.Root?.HasElements ?? false;
            }
            catch (XmlException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        /// <summary>
        /// Update the configuration entity
        /// </summary>
        public void UpdateConfiguration(Action<ConfigurationEntity> setConfiguration)
        {
            var configuration = ConfigurationData.Fetch();

            setConfiguration(configuration);

            ConfigurationData.Save(configuration);
        }

        /// <summary>
        /// Get the next update window after the given date
        /// </summary>
        public DateTime GetNextUpdateWindow(DateTime date)
        {
            var configuration = FetchReadOnly();
            var nextUpdateWindowDay = date.TodayOrNext(configuration.AutoUpdateDayOfWeek);
            var updateWindow = new DateTime(nextUpdateWindowDay.Year, nextUpdateWindowDay.Month, nextUpdateWindowDay.Day, configuration.AutoUpdateHourOfDay, 0, 0);

            return updateWindow > date ? updateWindow : updateWindow.AddDays(7);
        }
    }
}
