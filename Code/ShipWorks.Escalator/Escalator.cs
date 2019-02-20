﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Logic of the Escalator service
    /// </summary>
    public class Escalator : ServiceBase
    {
        private static ILog log = LogManager.GetLogger(typeof(ServiceBase));
        ShipWorksUpgrade shipWorksUpgrade;
        UpgradeTimeWindow upgradeTimeWindow;

        /// <summary>
        /// Constructor
        /// </summary>
        public Escalator()
        {
            this.ServiceName = ShipWorks.Escalator.ServiceName.Resolve();
            shipWorksUpgrade = new ShipWorksUpgrade();
            upgradeTimeWindow = new UpgradeTimeWindow(shipWorksUpgrade);
        }

        /// <summary>
        /// Code that runs when the service starts
        /// </summary>
        protected override void OnStart(string[] args)
        {
            try
            {
                System.Diagnostics.Debugger.Launch();

                log.Info("OnStart");
                // Start a communication bridge to listen for messages from ShipWorks
                ShipWorksCommunicationBridge communicationBridge =
                    new ShipWorksCommunicationBridge(ShipWorks.Escalator.ServiceName.GetInstanceID().ToString(),
                    LogManager.GetLogger(typeof(ShipWorksCommunicationBridge)));

                communicationBridge.OnMessage += OnShipWorksMessage;

                UpgradeTimeWindow.CallGetUpdateWindow();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// React to a message from ShipWorks
        /// </summary>
        private async void OnShipWorksMessage(string message)
        {
            log.InfoFormat("Message \"{0}\" received from ShipWorksCommunicationBridge.", message);
            await ProcessMessage(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Processes message - internal so it can be tested outside the service via Program.cs
        /// </summary>
        internal async Task ProcessMessage(string message)
        {
            try
            {
                if (Version.TryParse(message, out Version version))
                {
                    await shipWorksUpgrade.Upgrade(version).ConfigureAwait(false);
                }
                else if (message.TryParseJson(out UpdateWindowData updateWindowData))
                {
                    upgradeTimeWindow.UpdateWindow(updateWindowData);
                }
                else
                {
                    log.InfoFormat("\"{0}\" could not be parsed as version.", message);
                }
            }
            catch (Exception ex)
            {
                log.Error("An exception was thrown when attempting to download and install a new version of SW", ex);
            }
        }

        /// <summary>
        /// Code that runs when the service stops
        /// </summary>
        protected override void OnStop()
        {
            // Do nothing
        }
    }
}
