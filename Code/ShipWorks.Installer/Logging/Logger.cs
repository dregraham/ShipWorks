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

        /// <summary>
        /// Setup the logger
        /// </summary>
        public static void Setup()
        {
            string logFolder = Path.Combine(Path.GetTempPath(), "ShipWorks.Installer");
            System.IO.Directory.CreateDirectory(logFolder);

            Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date{HH:mm:ss.fff} %-5level [%logger] [%thread] --> %message%newline";
            patternLayout.ActivateOptions();

            FileAppender fileAppender = new FileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.File = Path.Combine(logFolder, $"ShipWorksInstaller_{DateTime.Now:yyyyMMddHHmmss}.log");
            fileAppender.Layout = patternLayout;
            fileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(fileAppender);

            hierarchy.Root.Level = Level.All;
            BasicConfigurator.Configure(hierarchy);
        }
    }
}