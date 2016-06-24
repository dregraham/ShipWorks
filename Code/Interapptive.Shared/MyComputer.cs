using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using log4net;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using Interapptive.Shared.Win32;
using System.Transactions;

namespace Interapptive.Shared
{
    /// <summary>
    /// System and environment properties of the current computer.
    /// </summary>
    public static class MyComputer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MyComputer));

        /// <summary>
        /// True if Windows Firewall is on the current machine.
        /// </summary>
        public static bool HasWindowsFirewall
        {
            get
            {
                // 2003 has it too, but its not on by default, and we don't want to mess with it (for now).
                return IsWindowsXP || IsWindowsVistaOrHigher;
            }
        }

        /// <summary>
        /// True if the current computer is running Windows XP
        /// </summary>
        public static bool IsWindowsXP
        {
            get
            {
                Version osversion = Environment.OSVersion.Version;

                return osversion.Major == 5 && osversion.Minor == 1;
            }
        }

        /// <summary>
        /// True if the current computer is running Windows Vista
        /// </summary>
        public static bool IsWindowsVista
        {
            get
            {
                Version osversion = Environment.OSVersion.Version;

                return osversion.Major == 6 && osversion.Minor == 0;
            }
        }

        /// <summary>
        /// Indicates if its a Vista or more recent operating system
        /// </summary>
        public static bool IsWindowsVistaOrHigher
        {
            get
            {
                Version osversion = Environment.OSVersion.Version;

                return osversion.Major >= 6;
            }
        }

        /// <summary>
        /// True if the current computer is running Windows Server 2003
        /// </summary>
        public static bool IsWindowsServer2003
        {
            get
            {
                Version osversion = Environment.OSVersion.Version;

                return osversion.Major == 5 && osversion.Minor == 2;
            }
        }

        /// <summary>
        /// Gets the service pack level of the operating system.
        /// </summary>
        public static int ServicePack
        {
            get
            {
                Match match = Regex.Match(Environment.OSVersion.ServicePack, "\\d+");

                if (!match.Success)
                {
                    return 0;
                }

                return Convert.ToInt32(match.Value);
            }
        }

        /// <summary>
        /// Indicates if the current process is loaded as a 64bit process.
        /// </summary>
        public static bool Is64BitProcess
        {
            get
            {
                return Environment.Is64BitProcess;
            }
        }

        /// <summary>
        /// Indicates if the running version of windows is 64bit.  This works regardless of if we
        /// are running as 64bit or as 32bit Wow64.
        /// </summary>
        public static bool Is64BitWindows
        {
            get
            {
                return Environment.Is64BitOperatingSystem;
            }
        }

        /// <summary>
        /// Get the installed version of MDAC
        /// </summary>
        public static Version MdacVersion
        {
            get
            {
                Version zero = new Version(0, 0);

                // First try the registry way.  This is how MS says to do it.
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\DataAccess"))
                {
                    if (key != null)
                    {
                        Version version = new Version((string) key.GetValue("FullInstallVer", "0.0.0.0"));
                        
                        // Have seen where the key is wrong and set to zero
                        if (version != zero)
                        {
                            log.InfoFormat("Found MDAC version in registry: {0}", version);
                            return version;
                        }
                    }
                }

                string mdacFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), @"System\msadc\msdfmap.dll");

                // First try the file way
                if (File.Exists(mdacFile))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(mdacFile);
                    Version version = new Version(info.FileVersion);

                    log.InfoFormat("Found MDAC version by file: {0}", version);

                    return version;
                }

                return zero;
            }
        }

        /// <summary>
        /// The installed version of Internet Explorer
        /// </summary>
        public static Version IEVersion
        {
            get
            {
                string iefile = Path.Combine(Environment.SystemDirectory, "wininet.dll");

                if (File.Exists(iefile))
                {
                    FileVersionInfo ieVersionInfo = FileVersionInfo.GetVersionInfo(iefile);
                    return new Version(ieVersionInfo.ProductVersion);
                }

                return new Version("0.0");
            }
        }

        /// <summary>
        /// The installed version of Windows Installer
        /// </summary>
        public static Version WindowsInstallerVersion
        {
            get
            {
                string msiFile = Path.Combine(Environment.SystemDirectory, "msi.dll");

                if (File.Exists(msiFile))
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(msiFile);

                    // This split was added due to Windows 8.  Windows 8 added information text after the version number porition, causing the Version() constructor
                    // to throw an exception.
                    string version = versionInfo.FileVersion.Split(' ')[0];
                    return new Version(version);
                }
                else
                {
                    throw new InvalidOperationException("Windows Installer must exist - something is wrong.");
                }
            }
        }

        /// <summary>
        /// Log important information about the environment we are executing in.
        /// </summary>
        public static void LogEnvironmentProperties()
        {
            // Application Version
            log.Info(Assembly.GetEntryAssembly().FullName);
            log.InfoFormat("Build date: {0}", AssemblyDateAttribute.Read(Assembly.GetEntryAssembly()));

            log.InfoFormat("Loading from: '{0}'", Application.StartupPath);

            // OS Version
            log.Info(Environment.OSVersion.VersionString);

            // Bitness
            log.InfoFormat("x64 OS: {0}", Is64BitWindows);
            log.InfoFormat("x64 Process: {0}", Is64BitProcess);

            // .NET version
            log.InfoFormat(".NET Version: {0}", Environment.Version);

            // Session
            log.InfoFormat("TerminalServices: {0}", SystemInformation.TerminalServerSession);

            // IE Version
            log.Info("IE Version: " + IEVersion);
            log.Info("Windows Installer: " + WindowsInstallerVersion);

            // Log the current culture settings
            log.Info("CurrentCulture: " + Thread.CurrentThread.CurrentCulture);
            log.Info("CurrentUICulture: " + Thread.CurrentThread.CurrentUICulture);

            // Transactions
            log.Info("TransactionManager.DefaultTimeout: " + TransactionManager.DefaultTimeout.TotalSeconds);
            log.Info("TransactionManager.MaximumTimeout: " + TransactionManager.MaximumTimeout.TotalSeconds);
        }

        /// <summary>
        /// Determines the current screen resolution in DPI.
        /// </summary>
        public static string GetSystemDpi()
        {
            float dpiX;
            float dpiY;

            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = graphics.DpiX;
                dpiY = graphics.DpiY;
            }

            return $"{dpiX}x{dpiY}";
        }
    }
}
