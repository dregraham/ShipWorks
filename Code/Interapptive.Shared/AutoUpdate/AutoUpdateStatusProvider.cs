using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Status provider for the auto update process
    /// </summary>
    [Component]
    public class AutoUpdateStatusProvider : IAutoUpdateStatusProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoUpdateStatusProvider));
        private const string SplashScreenProcess = "ShipWorks.SplashScreen";

        /// <summary>
        /// Update the AutoUpdate status
        /// </summary>
        public void UpdateStatus(string status)
        {
            log.Info($"AutoUpdateStatusProvider.UpdateStatus - status: {status}");

            using (NamedPipeClientStream statusPipe = new NamedPipeClientStream(".", "ShipWorksUpgradeStatus", PipeDirection.Out))
            {
                try
                {
                    statusPipe.Connect(100);
                    statusPipe.Write(Encoding.UTF8.GetBytes(status), 0, status.Length);
                }
                catch (Exception ex)
                {
                    log.Error("error updating status", ex);
                }
            }
        }

        /// <summary>
        /// Close the splash screen
        /// </summary>
        public void CloseSplashScreen()
        {
            foreach (Process process in Process.GetProcessesByName(SplashScreenProcess))
            {
                try
                {
                    process?.Kill();
                }
                catch (Exception ex)
                {
                    // if it fails ignore it
                    log.Error("error closing splash screen", ex);
                }
            }
        }

        /// <summary>
        /// Show the splash screen
        /// </summary>
        /// <remarks>
        /// If the splash isnt shown this will show it
        /// Path where to copy the splash screen
        /// </remarks>
        public void ShowSplashScreen(string instanceId)
        {
            using (IDisposable process = Process.GetProcessesByName(SplashScreenProcess).FirstOrDefault())
            {
                if (process != null)
                {
                    return;
                }
            }

            string existingFile = $"{SplashScreenProcess}.exe";

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string destinationPath = Path.Combine(appData, "Interapptive\\ShipWorks\\Instances", instanceId, existingFile);

            CopySplashScreenExe(existingFile, destinationPath);

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                if (identity.IsSystem)
                {
                    var result = ShipWorksLauncher.StartProcessAsCurrentUser(destinationPath);
                }
                else
                {
                    Process.Start(destinationPath);
                }
            }
        }

        /// <summary>
        /// Copies ShipWorks.SplashScreen.exe to appData
        /// </summary>
        private static void CopySplashScreenExe(string existingFile, string destinationPath)
        {
            string sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), existingFile);
            try
            {
                File.Copy(sourcePath, destinationPath, true);
            }
            catch (Exception ex)
            {
                // if it fails ignore it
                log.Error("error deleting shipworks.splashscreen.exe", ex);
            }
        }
    }
}
