using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Installer.Environments;
using ShipWorks.Installer.Extensions;
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
        private readonly IRegistryService registryService;

        /// <summary>
        /// Constructor
        /// </summary>
        public InnoSetupService(Func<Type, ILog> logFactory,
            IWebClientEnvironmentFactory webClientEnvironmentFactory,
            IRegistryService registryService)
        {
            log = logFactory(typeof(InnoSetupService));
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
            this.registryService = registryService;
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
            InstallSettings.Error = Enums.InstallError.DownloadingShipWorks;
        }

        /// <summary>
        /// Call Inno installer to install ShipWorks
        /// </summary>
        public async Task InstallShipWorks(InstallSettings installSettings)
        {
            InstallSettings = installSettings;

            if (!File.Exists(innoSetupDownloadPath))
            {
                log.InfoFormat($"Inno Install File {innoSetupDownloadPath} was not found.");

                InstallSettings.Error = Enums.InstallError.DownloadingShipWorks;
                return;
            }

            log.Info("Starting ShipWorks Install");

            try
            {
                await RunSetup(installSettings.InstallPath);
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while running Inno Setup.", ex);
                InstallSettings.Error = Enums.InstallError.InstallShipWorks;
            }
        }

        /// <summary>
        /// Run ShipWorks setup
        /// </summary>
        private async Task RunSetup(string installLocation)
        {
            // Kill any ShipWorks instances that are running
            KillShipWorks();

            string logFolder = Path.Combine(Path.GetTempPath(), "InnoShipWorksInstaller");
            System.IO.Directory.CreateDirectory(logFolder);
            string logFileName = Path.Combine(logFolder, "InnoInstaller.log");

            var createIconCommand = InstallSettings.CreateShortcut ? "/MERGETASKS=\"desktopicon\"" : "/MERGETASKS=\"!desktopicon\"";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = innoSetupDownloadPath,
                Arguments = $"/VERYSILENT /DIR=\"{installLocation}\" /LOG=\"{logFileName}\" {createIconCommand} /FORCECLOSEAPPLICATIONS ",
            };

            log.InfoFormat("Command to run [{0} {1}]", start.FileName, start.Arguments);

            int exitCode;

            log.Info("Starting Inno Install Process");

            using (Process proc = Process.Start(start))
            {
                log.Info("Waiting for Inno Install Process to finish");
                await proc.WaitForExitAsync();

                log.Info("Inno Install Process finished.");
                exitCode = proc.ExitCode;
                log.InfoFormat("Inno Install Process exit code {0}", exitCode);
            }

            if (exitCode != 1)
            {
                InstallSettings.Error = Enums.InstallError.InstallShipWorks;
            }
        }

        /// <summary>
        /// Kill any instance of Shipworks UI running.
        /// </summary>
        private void KillShipWorks()
        {
            foreach (Process process in Process.GetProcessesByName("shipworks"))
            {
                log.Info($"Killing ShipWorks processes.");
                process?.Kill();
            }
        }

        /// <summary>
        /// Silently run the INNO uninstaller
        /// </summary>
        public async Task RunUninstaller()
        {
            // Kill any ShipWorks instances that are running
            KillShipWorks();

            string logFolder = Path.Combine(Path.GetTempPath(), "InnoShipWorksUninstaller");
            Directory.CreateDirectory(logFolder);
            string logFileName = Path.Combine(logFolder, "InnoInstaller.log");

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = registryService.GetUninstallerPath(),
                Arguments = $"/VERYSILENT /LOG=\"{logFileName}\"",
            };

            log.InfoFormat("Command to run [{0} {1}]", start.FileName, start.Arguments);

            int exitCode;

            log.Info("Starting Uninstaller");

            using (Process proc = Process.Start(start))
            {
                log.Info("Waiting for the uninstaller to finish");
                await proc.WaitForExitAsync();

                log.Info("Uninstaller finished.");
                exitCode = proc.ExitCode;
                log.InfoFormat("Uninstaller exited with code {0}", exitCode);
            }
        }
    }
}
