using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
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
            ShipWorksCommunicationBridge communicationBridge =
                new ShipWorksCommunicationBridge(ShipWorks.Escalator.ServiceName.GetInstanceID().ToString(),
                LogManager.GetLogger(typeof(ShipWorksCommunicationBridge)));

            communicationBridge.OnMessage += OnShipWorksMessage;
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
        internal static async Task ProcessMessage(string message)
        {
            try
            {
                if (Version.TryParse(message, out Version version))
                {

                    UpdaterWebClient updaterWebClient = new UpdaterWebClient();
                    (Uri url, string sha) newVersionInfo = await updaterWebClient.GetVersionToDownload(new Version(message)).ConfigureAwait(false);

                    if (IsInstallRunning(newVersionInfo.url))
                    {
                        log.ErrorFormat("The installer {0} is already running", newVersionInfo.url);
                    }
                    else
                    {
                        await Install(updaterWebClient, newVersionInfo);
                    }
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
        /// Download and install new version of Shipworks
        /// </summary>
        private static async Task Install(UpdaterWebClient updaterWebClient, (Uri url, string sha) newVersionInfo)
        {
            log.InfoFormat("The installer {0} is not already running. Beginning Download...", newVersionInfo.url);
            InstallFile newVersion = await updaterWebClient.Download(newVersionInfo.url, newVersionInfo.sha).ConfigureAwait(false);

            log.Info("Attempting to install new version");
            Result installationResult = new ShipWorksInstaller().Install(newVersion);
            if (installationResult.Failure)
            {
                log.ErrorFormat("An error occured while installing the new version of ShipWorks: {0}", installationResult.Message);
            }
        }

        /// <summary>
        /// Detects that the installer is already running
        /// </summary>
        private static bool IsInstallRunning(Uri newVersion)
        {
            string fileName = Path.GetFileNameWithoutExtension(newVersion.LocalPath);
            return Process.GetProcessesByName(fileName).Any();
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
