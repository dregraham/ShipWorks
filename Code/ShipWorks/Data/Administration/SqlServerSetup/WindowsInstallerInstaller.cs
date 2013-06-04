using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShipWorks.ApplicationCore;
using System.Security.Cryptography;
using log4net;
using System.Diagnostics;
using Interapptive.Shared;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Utility class for helping with the installation of Windows Installer
    /// </summary>
    public class WindowsInstallerInstaller
    {
         // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsInstallerInstaller));

        /// <summary>
        /// Information about the versions of the Windows Installer package
        /// </summary>
        class InstallerInfo
        {
            public InstallerInfo(string fileName, string checksum)
            {
                FileName = fileName;
                Checksum = checksum;
            }

            public string FileName { get; set; }
            public string Checksum { get; set; }
        }

        /// <summary>
        /// Target platform for the windows installer redistributable
        /// </summary>
        enum InstallerPlatformTarget
        {
            Vista_Server08_x86,
            Vista_Server08_x64,
            WindowsXP_x86,
            Server08_x86,
            WindowxXP_Server08_x64
        }

        // Maps the target the the required filename and checksum
        static Dictionary<InstallerPlatformTarget, InstallerInfo> installerInfoMap = new Dictionary<InstallerPlatformTarget, InstallerInfo>();

        // The platform target of the running system
        static InstallerPlatformTarget platformTarget;

        // Raised when the installation executable has exited
        public event EventHandler Exited;

        // The exit code from the last attempt at installation
        private int lastExitCode = -1;

        /// <summary>
        /// Static constructor
        /// </summary>
        static WindowsInstallerInstaller()
        {
            installerInfoMap[InstallerPlatformTarget.Vista_Server08_x86]     = new InstallerInfo("Windows6.0-KB942288-v2-x86.msu",        "ioEXoroKuSYWdVV9GkwzQGo54tBiC9l15nCbqa6tRpY=");
            installerInfoMap[InstallerPlatformTarget.Vista_Server08_x64]     = new InstallerInfo("Windows6.0-KB942288-v2-x64.msu",        "WWogYg1GI99qgGBIUT22ezfxUeZokAvkBbbS/dGjHSs=");
            installerInfoMap[InstallerPlatformTarget.WindowsXP_x86]          = new InstallerInfo("WindowsXP-KB942288-v3-x86.exe",         "rNWJtX7RBLXgSnzijcFmZUwJBgycMa6OLJMB+gmLv6U=");
            installerInfoMap[InstallerPlatformTarget.Server08_x86]           = new InstallerInfo("WindowsServer2003-KB942288-v4-x86.exe", "dj4P/y8LRBPBpRtwFuobXkrldo0Fk9/t8oXdwmzxgTc=");
            installerInfoMap[InstallerPlatformTarget.WindowxXP_Server08_x64] = new InstallerInfo("WindowsServer2003-KB942288-v4-x64.exe", "DH4s1zlKmcnXzIsbtXX0duxeMPU+WaNAd/hIv5aqaj0=");

            platformTarget = DeterminePlatformTarget();
            log.InfoFormat("Windows Installer redistributable: {0}", installerInfoMap[platformTarget].FileName);
        }

        /// <summary>
        /// Determine the platform target of the running system
        /// </summary>
        private static InstallerPlatformTarget DeterminePlatformTarget()
        {
            if (MyComputer.IsWindowsXP)
            {
                return MyComputer.Is64BitWindows ? InstallerPlatformTarget.WindowxXP_Server08_x64 : InstallerPlatformTarget.WindowsXP_x86;
            }

            else if (MyComputer.IsWindowsServer2003)
            {
                return MyComputer.Is64BitWindows ? InstallerPlatformTarget.WindowxXP_Server08_x64 : InstallerPlatformTarget.Server08_x86;
            }

            else
            {
                return MyComputer.Is64BitWindows ? InstallerPlatformTarget.Vista_Server08_x64 : InstallerPlatformTarget.Vista_Server08_x86;
            }
        }

        /// <summary>
        /// Indicates if the redistributable file for installing Windows Installer is available
        /// </summary>
        public static bool IsRedistributableAvailable
        {
            get
            {
                if (!File.Exists(RedistributableLocalPath))
                {
                    return false;
                }

                SHA256 sha = SHA256.Create();
                byte[] checksum = sha.ComputeHash(File.ReadAllBytes(RedistributableLocalPath));

                return Convert.ToBase64String(checksum) == installerInfoMap[platformTarget].Checksum;
            }
        }

        /// <summary>
        /// The path to the location of windows installer on the local disk
        /// </summary>
        public static string RedistributableLocalPath
        {
            get
            {
                return Path.Combine(DataPath.Components, installerInfoMap[platformTarget].FileName);
            }
        }

        /// <summary>
        /// The full Uri for downloading the Windows Installer installer
        /// </summary>
        public static Uri RedistributableDownloadUri
        {
            get
            {
                return new Uri("http://www.interapptive.com/download/components/wininstaller45/" + installerInfoMap[platformTarget].FileName);
            }
        }

        /// <summary>
        /// Begin the installation of Windows Installer.  The method returns before the installation is complete.
        /// </summary>
        /// <exception cref="Win32Exception" />
        public void InstallWindowsInstaller()
        {
            // Adding quite is the only way to make it honor norestart
            string args = "/quiet /norestart";

            log.InfoFormat("Executing: {0}", args);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(RedistributableLocalPath, args);
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);
            process.Start();
        }

        /// <summary>
        /// The installation process has exited
        /// </summary>
        private void OnInstallerExited(object sender, EventArgs e)
        {
            Process process = (Process)sender;
            lastExitCode = process.ExitCode;

            if (Exited != null)
            {
                Exited(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The exit code from the last attempt at installation.
        /// </summary>
        public int LastExitCode
        {
            get { return lastExitCode; }
        }

        /// <summary>
        /// Formats the return code to be user readable.
        /// </summary>
        public static string FormatReturnCode(int code)
        {
            // 1603 is supposed to mean fatal error - but in my testing its what it keps returning
            // for canceling using the installer UI.
            if (code == 1602 || code == 1603 || code == 1223)
            {
                return "Setup was canceled by the user.";
            }

            return string.Format("Reason code: {0}.", code);
        }
    }
}
