using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace ShipWorks.Installer.Logging
{
    /// <summary>
    /// Class for configuring log4net
    /// </summary>
    public class Logger
    {
        private static readonly string logFolder;
        private const string InnoInstallerLogFileName = "InnoInstaller.log";
        private const string InnoUninstallerLogFileName = "InnoUninstaller.log";

        /// <summary>
        /// Static constructor
        /// </summary>
        static Logger()
        {
            logFolder = Path.Combine(Path.GetTempPath(), "ShipWorks.Installer");
            Directory.CreateDirectory(logFolder);
        }

        /// <summary>
        /// Folder in which to write log files
        /// </summary>
        public static string LogFolder => logFolder;

        /// <summary>
        /// Get a path and filename for logging
        /// </summary>
        private static string GetLoggingPathAndFileName(string fileName)
        {
            return Path.Combine(LogFolder, fileName);
        }

        /// <summary>
        /// Get a path and filename for Inno Installer log file
        /// </summary>
        public static string GetInnoSetupInstallLogName()
        {
            return Path.Combine(LogFolder, InnoInstallerLogFileName);
        }

        /// <summary>
        /// Get a path and filename for Inno Uninstaller log file
        /// </summary>
        public static string GetInnoSetupUninstallLogName()
        {
            return Path.Combine(LogFolder, InnoUninstallerLogFileName);
        }

        /// <summary>
        /// Setup the logger
        /// </summary>
        public static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date{HH:mm:ss.fff} %-5level [%logger] [%thread] --> %message%newline"
            };
            patternLayout.ActivateOptions();

            FileAppender fileAppender = new FileAppender
            {
                AppendToFile = true,
                File = GetLoggingPathAndFileName($"ShipWorksInstaller_{DateTime.Now:yyyyMMddHHmmss}.log"),
                Layout = patternLayout
            };

            fileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(fileAppender);

            hierarchy.Root.Level = Level.All;
            BasicConfigurator.Configure(hierarchy);
        }
    }
}