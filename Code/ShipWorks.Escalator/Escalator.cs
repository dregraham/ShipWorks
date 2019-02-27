using System;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Logic of the Escalator service
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class Escalator
    {
        private static ILog log;
        private IShipWorksUpgrade shipWorksUpgrade;
        private readonly IShipWorksCommunicationBridge communicationBridge;
        private IUpgradeTimeWindow upgradeTimeWindow;
        private readonly IServiceName serviceName;

        /// <summary>
        /// Constructor
        /// </summary>
        public Escalator(IServiceName serviceName, 
            IShipWorksUpgrade shipWorksUpgrade, 
            IShipWorksCommunicationBridge communicationBridge, 
            IUpgradeTimeWindow upgradeTimeWindow,
            Func<Type, ILog> logFactory)
        {
            this.shipWorksUpgrade = shipWorksUpgrade;
            this.communicationBridge = communicationBridge;
            this.upgradeTimeWindow = upgradeTimeWindow;
            this.serviceName = serviceName;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Code that runs when the service starts
        /// </summary>
        public void OnStart()
        {
            try
            {
                log.Info("OnStart");
                // Start a communication bridge to listen for messages from ShipWorks
                communicationBridge.StartPipeServer();
                communicationBridge.OnMessage += OnShipWorksMessage;

                upgradeTimeWindow.CallGetUpdateWindow();
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
                    log.WarnFormat("\"{0}\" could not be parsed as version.", message);
                }
            }
            catch (Exception ex)
            {
                log.Error("An exception was thrown when attempting to download and install a new version of SW", ex);
            }
        }
    }
}
