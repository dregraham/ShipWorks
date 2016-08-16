using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using log4net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Utility class for installing the .NET Framework 3.5 SP1
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5350: Do not use insecure cryptographic algorithm SHA1",
        Justification = "This is what ShipWorks currently uses")]
    public class DotNet35Installer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DotNet35Installer));

        // Redistributable filename
        const string fileName = "dotnetfx35setup.exe";

        // Raised when the installation executable has exited
        public event EventHandler Exited;

        // The exit code from the last attempt at installation
        private int lastExitCode = -1;

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

                SHA1 sha = SHA1.Create();
                byte[] checksum = sha.ComputeHash(File.ReadAllBytes(RedistributableLocalPath));

                return Convert.ToBase64String(checksum) == "7J8MMbmUnKHPFOmkO8oGX6W8DnE=";
            }
        }

        /// <summary>
        /// The path to the location of windows installer on the local disk
        /// </summary>
        public static string RedistributableLocalPath
        {
            get
            {
                return Path.Combine(DataPath.Components, fileName);
            }
        }

        /// <summary>
        /// The full Uri for downloading the Windows Installer installer
        /// </summary>
        public static Uri RedistributableDownloadUri
        {
            get
            {
                return new Uri("http://www.interapptive.com/download/components/dotnet35sp1/" + fileName);
            }
        }

        /// <summary>
        /// Begin the installation of Windows Installer.  The method returns before the installation is complete.
        /// </summary>
        /// <exception cref="Win32Exception" />
        public void InstallDotNet35()
        {
            // Adding quite is the only way to make it honor norestart
            string args = "/q /norestart";

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
            Process process = (Process) sender;
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
            // 1603 is supposed to mean fatal error - but in my testing its what it keeps returning
            // for canceling using the installer UI.
            if (code == 1602)
            {
                return "Setup was canceled by the user.";
            }

            return string.Format("Reason code: {0}.", code);
        }
    }
}
