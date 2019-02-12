using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Logic of the Escalator service
    /// </summary>
    public class Escalator : ServiceBase
    {
        private static ILog log = LogManager.GetLogger(typeof(ServiceBase));

        /// <summary>
        /// Constructor
        /// </summary>
        public Escalator()
        {
            this.ServiceName = ShipWorks.Escalator.ServiceName.Resolve();
        }

        /// <summary>
        /// Code that runs when the service starts
        /// </summary>
        protected override void OnStart(string[] args)
        {
            log.Info("OnStart");
            // Start a communication bridge to listen for messages from ShipWorks
            ShipWorksCommunicationBridge communicationBridge = new ShipWorksCommunicationBridge(ShipWorks.Escalator.ServiceName.GetInstanceID().ToString());
            communicationBridge.OnMessage += OnShipWorksMessage;
        }     

        /// <summary>
        /// React to a message from ShipWorks
        /// </summary>
        private async void OnShipWorksMessage(string message)
        {
            log.Info($"Message \"{message}\" received from ShipWorksCommunicationBridge.");
            await ProcessMessage(message);
        }

        /// <summary>
        /// Processes message - internal so it can be tested outside the service via Program.cs
        /// </summary>
        internal static async Task ProcessMessage(string message)
        {
            try
            {
                if (Version.TryParse(message, out Version version))
                {
                    InstallFile newVersion = await new UpdaterWebClient().Download(new Version(message));

                    log.Info("Attempting to install new version");
                    new ShipWorksInstaller().Install(newVersion);
                }
                else
                {
                    log.Info($"\"{message}\" could not be parsed as version.");
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
