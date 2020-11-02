using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using log4net;
using ShipWorks.Installer.Environments;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Class for downloading and running the INNO Setup Installer
    /// </summary>
    public class InnoSetupService : IInnoSetupService
    {
        private string innoSetupDownloadPath = $"{Path.GetTempPath()}ShipWorksInstaller.exe";
        private readonly ILog log;
        private readonly WebClientEnvironment webClientEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        public InnoSetupService(Func<Type, ILog> logFactory, IWebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            log = logFactory(typeof(InnoSetupService));
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
        }

        /// <summary>
        /// Install settings
        /// </summary>
        private InstallSettings InstallSettings { get; set; }

        /// <summary>
        /// Download the INNO Setup installer
        /// </summary>
        public void DownloadInstaller(InstallSettings installSettings)
        {
            InstallSettings = installSettings;
            WebClient client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadComplete);

            try
            {
                log.Info("Beginning download of background installer.");
                client.DownloadFileAsync(new Uri(webClientEnvironment.InnoInstallerUrl), innoSetupDownloadPath);
            }
            catch (Exception ex)
            {
                HandleDownloadErrors(ex);
            }
        }

        /// <summary>
        /// Event handler when the file download is complete
        /// </summary>
        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                HandleDownloadErrors(e.Error);
                return;
            }

            log.Info($"Background installer downloaded to {innoSetupDownloadPath}");
            InstallSettings.InnoSetupDownloaded = true;
        }

        /// <summary>
        /// Handle errors from downloading the installer
        /// </summary>
        private void HandleDownloadErrors(Exception ex)
        {
            log.Error("An error occurred downloading the background installer.", ex);
            InstallSettings.Error = Enums.InstallError.ShipWorks;
        }
    }
}
