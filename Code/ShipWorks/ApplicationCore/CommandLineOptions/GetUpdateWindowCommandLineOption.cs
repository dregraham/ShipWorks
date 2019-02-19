using System;
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
using ShipWorks.Stores;
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
            SqlSession.Initialize();
            ConfigurationData.CheckForChangesNeeded();

            log.Info("Fetching config data");
            Data.Model.EntityClasses.ConfigurationEntity config = ConfigurationData.Fetch();

            log.Info("Fetching customer id");
            DataProvider.InitializeForApplication();
            StoreManager.InitializeForCurrentSession(SecurityContext.EmptySecurityContext);
            string tangoCustomerId = TangoWebClient.GetTangoCustomerId();

            UpdateWindowData updateData = new UpdateWindowData()
            {
                AutoUpdateDayOfWeek = config.AutoUpdateDayOfWeek,
                AutoUpdateHourOfDay = config.AutoUpdateHourOfDay,
                TangoCustomerId = tangoCustomerId
            };

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                log.Info("Sending message");
                lifetimeScope.Resolve<IUpdateService>().SendMessage(JsonConvert.SerializeObject(updateData));
                log.Info("Message sent");
            }

            log.Info("GetUpdateWindowCommandLineOption Complete.");
            return Task.CompletedTask;
        }
    }
}
