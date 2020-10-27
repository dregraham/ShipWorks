using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using log4net;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for downloading and running the INNO Setup Installer
    /// </summary>
    public class InnoSetupService : IInnoSetupService
    {
        private const string InnoSetupInstallerURL = "https://prod-sw-downloads-app.s3.amazonaws.com/ShipWorksInstaller.exe";
        private readonly ILog log;
        private InstallSettings installSettings;

        public InnoSetupService(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(InnoSetupService));
        }

        /// <summary>
        /// Download the INNO Setup installer
        /// </summary>
        public void DownloadInstaller(InstallSettings installSettings)
        {
            this.installSettings = installSettings;
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadComplete);

            try
            {
                client.DownloadFileAsync(new Uri(InnoSetupInstallerURL), $"{Path.GetTempPath()}\\ShipWorksInstaller.exe");
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }
        }

        /// <summary>
        /// Event handler when the file download is complete
        /// </summary>
        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                HandleErrors(e.Error);
                return;
            }

            installSettings.InnoSetupDownloaded = true;
        }

        /// <summary>
        /// Handle errors from downloading the installer
        /// </summary>
        private void HandleErrors(Exception ex)
        {
            log.Error("An error occurred downloading the background installer.", ex);
            installSettings.Error = Enums.InstallError.ShipWorks;
        }
    }
}
