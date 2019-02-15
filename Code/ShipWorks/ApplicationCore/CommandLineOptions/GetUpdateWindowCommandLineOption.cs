using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Tell the update service what the update window is
    /// </summary>
    public class GetUpdateWindowCommandLineOption : ICommandLineCommandHandler
    {
        /// <summary>
        /// getupdatewindow
        /// </summary>
        public string CommandName => "getupdatewindow";

        /// <summary>
        /// Tell the update service what the update window is
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task Execute(List<string> args)
        {
            SqlSession.Initialize();
            ConfigurationData.CheckForChangesNeeded();
            Data.Model.EntityClasses.ConfigurationEntity config = ConfigurationData.Fetch();

            UpdateWindowData updateData = new UpdateWindowData()
            {
                AutoUpdateDayOfWeek = config.AutoUpdateDayOfWeek,
                AutoUpdateHourOfDay = config.AutoUpdateHourOfDay
            };

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IUpdateService>().SendMessage(JsonConvert.SerializeObject(updateData));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// struct to hold the data we send
        /// </summary>
        private struct UpdateWindowData
        {
            public DayOfWeek AutoUpdateDayOfWeek { get; set; }
            public int AutoUpdateHourOfDay { get; set; }
        }
    }
}
