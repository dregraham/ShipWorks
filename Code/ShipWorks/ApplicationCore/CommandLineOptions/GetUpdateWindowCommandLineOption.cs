using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Tell the update service what the update window is
    /// </summary>
    public class GetUpdateWindowCommandLineOption : ICommandLineCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GetUpdateWindowCommandLineOption));

        /// <summary>
        /// getupdatewindow
        /// </summary>
        public string CommandName => "getupdatewindow";

        /// <summary>
        /// Tell the update service what the update window is
        /// </summary>
        public Task Execute(List<string> args)
        {
            log.Info("Executing getupdatewindow commandline");
            if (!ShouldUpgrade())
            {
                return Task.CompletedTask;
            }

            try
            {
                log.Info("Autoupdate enabled.");
                SqlSession.Initialize();

                if (!ShouldUpgradeSql())
                {
                    return Task.CompletedTask;
                }

                ConfigurationData.CheckForChangesNeeded();

                log.Info("Fetching config data");
                ConfigurationEntity config = ConfigurationData.Fetch();
                Initialize();

                log.Info("Fetching customer id");
                string tangoCustomerId = TangoWebClient.GetTangoCustomerId();

                UpdateWindowData updateData = new UpdateWindowData()
                {
                    AutoUpdateDayOfWeek = config.AutoUpdateDayOfWeek,
                    AutoUpdateHourOfDay = config.AutoUpdateHourOfDay,
                    TangoCustomerId = tangoCustomerId
                };

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    string message = JsonConvert.SerializeObject(updateData);
                    log.InfoFormat("Sending message {0}", message);
                    lifetimeScope.Resolve<IShipWorksCommunicationBridge>(new TypedParameter(typeof(string), ShipWorksSession.InstanceID.ToString())).SendMessage(message);
                    log.Info("Message sent");
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occurred getting update window.", ex);
            }

            log.Info("GetUpdateWindowCommandLineOption Complete.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Initializes SW
        /// </summary>
        private static void Initialize()
        {
            DataProvider.InitializeForApplication();
            StoreManager.InitializeForCurrentSession(SecurityContext.EmptySecurityContext);
            UserSession.InitializeForCurrentDatabase();
        }

        /// <summary>
        /// Returns true if can connect to SQL and SQL is up to date
        /// </summary>
        private bool ShouldUpgradeSql()
        {
            if (!SqlSession.Current.CanConnect())
            {
                log.Info("Cannot connect to SQL Server. Not sending window");
                return false;
            }

            if (SqlSchemaUpdater.IsUpgradeRequired())
            {
                log.Info("Update required. Not sending window");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Return true if last update succeeded and AutoUpdate enabled
        /// </summary>
        public bool ShouldUpgrade()
        {
            if (AutoUpdateSettings.IsAutoUpdateDisabled)
            {
                log.Info("Autoupdate disabled. Not sending window");
                return false;
            }

            if (!AutoUpdateSettings.LastAutoUpdateSucceeded)
            {
                log.Info("Autoupdate recently failed. Not sending window");
                return false;
            }

            return true;
        }
    }
}
