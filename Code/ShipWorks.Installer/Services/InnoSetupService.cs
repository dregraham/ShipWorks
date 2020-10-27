using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for downloading and running the INNO Setup Installer
    /// </summary>
    public class InnoSetupService : IInnoSetupService
    {
        private const string InnoSetupInstallerURL = "https://prod-sw-downloads-app.s3.amazonaws.com/ShipWorksInstaller.exe";
        private InstallSettings installSettings;

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
            catch (Exception)
            {
                // TODO: Log exception
                installSettings.Error = Enums.InstallError.ShipWorks;
            }
        }

        /// <summary>
        /// Event handler when the file download is complete
        /// </summary>
        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            installSettings.InnoSetupDownloaded = true;
        }
    }
}
