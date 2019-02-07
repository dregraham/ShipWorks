using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Logic of the Escalator service
    /// </summary>
    public class Escalator : ServiceBase
    {
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
            ResumeFromDisk();

            // Start a communication bridge to listen for messages from ShipWorks
            ShipWorksCommunicationBridge communicationBridge = new ShipWorksCommunicationBridge(ShipWorks.Escalator.ServiceName.GetInstanceID().ToString());
            communicationBridge.OnMessage += OnShipWorksMessage;
        }

        /// <summary>
        /// Check to see if we need to resume any operations from disk
        /// </summary>
        private void ResumeFromDisk()
        {
            string status = GetResumeStatus();

            if (status == "SUCCESS")
            {
                ShipWorksLauncher.StartShipWorks();
            }
        }

        /// <summary>
        /// Read the resume status from disk
        /// </summary>
        private string GetResumeStatus()
        {
            string pathToStatusFile = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\install.status";

            string status = string.Empty;
            try
            {
                status = File.ReadAllText(pathToStatusFile).Trim();
                File.Delete(pathToStatusFile);
            }
            catch
            {
                // reading the file failed
            }

            return status;
        }

        /// <summary>
        /// React to a message from ShipWorks
        /// </summary>
        private async void OnShipWorksMessage(string message)
        {
            if (Version.TryParse(message, out Version version))
            {
                InstallFile newVersion = await new UpdaterWebClient().Download(new Version(message));

                if (newVersion.IsValid)
                {
                    new ShipWorksInstaller().Install(newVersion);
                }
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
