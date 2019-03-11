using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
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
            if (InterapptiveOnly.DisableAutoUpdate)
            {
                log.Info("Autoupdate disabled in registry. Not sending window");
                return Task.CompletedTask;
            }

            log.Info("Autoupdate enabled.");
            SqlSession.Initialize();
            ConfigurationData.CheckForChangesNeeded();

            log.Info("Fetching config data");
            ConfigurationEntity config = ConfigurationData.Fetch();

            log.Info("Fetching customer id");
            DataProvider.InitializeForApplication();
            StoreManager.InitializeForCurrentSession(SecurityContext.EmptySecurityContext);
            UserSession.InitializeForCurrentDatabase();
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

            log.Info("GetUpdateWindowCommandLineOption Complete.");
            return Task.CompletedTask;
        }
    }
}
