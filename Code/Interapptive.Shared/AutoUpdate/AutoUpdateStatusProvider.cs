using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using log4net;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Status provider for the auto update process
    /// </summary>
    public static class AutoUpdateStatusProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoUpdateStatusProvider));
        private const string SplashScreenExe = "ShipWorks.SplashScreen";
        private const string ProcessName = "ShipWorks.SplashScreen.temp";

        /// <summary>
        /// Update the AutoUpdate status
        /// </summary>
        public static void UpdateStatus(string status)
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
        public static void CloseSplashScreen()
        {
            using (Process process = Process.GetProcessesByName(ProcessName).FirstOrDefault())
            {
                process?.Kill();
            }
        }

        /// <summary>
        /// Show the splash screen
        /// </summary>
        /// <remarks>
        /// If the splash isnt shown this will show it
        /// </remarks>
        public static void ShowSplashScreen()
        {
            using (IDisposable process = Process.GetProcessesByName(ProcessName).FirstOrDefault())
            {
                if (process == null)
                {
                    ShowSplashScreen();
                }
            }

            string existingFile = $"{SplashScreenExe}.exe";
            string newFile = $"{SplashScreenExe}.temp.exe";

            string sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), existingFile);
            string destinationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), newFile);
            File.Copy(sourcePath, destinationPath, true);

            // Ensure the process can be accessed by anyone
            FileSecurity fileSecurity = new FileSecurity();
            fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl(destinationPath, fileSecurity);

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                if (identity.IsSystem)
                {
                    ShipWorksLauncher.StartProcessAsCurrentUser(destinationPath);
                }
            }

            Process.Start(destinationPath);
        }
    }
}
