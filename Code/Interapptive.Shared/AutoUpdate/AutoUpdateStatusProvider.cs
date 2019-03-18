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
                catch (Exception)
                {
                    // if it fails ignore it
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

            string sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), existingFile);

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string destinationPath = Path.Combine(appData, "Interapptive\\ShipWorks\\Instances", instanceId, existingFile);

            File.Copy(sourcePath, destinationPath, true);

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
    }
}
