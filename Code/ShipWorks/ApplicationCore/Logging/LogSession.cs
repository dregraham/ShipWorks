using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Threading;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Log4Net;
using log4net.Appender;
using log4net.Config;
using log4net.Filter;
using log4net.Layout;
using SD.LLBLGen.Pro.DQE.SqlServer;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Class for managing our logging configuration
    /// </summary>
    public static class LogSession
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(LogSession));

        // File from which to save and restore settings
        private static readonly string filename = Path.Combine(DataPath.InstanceSettings, "logging.xml");

        // Base path for log files for this session
        private static string logPath;

        // Current logging options
        private static LogOptions logOptions = new LogOptions();

        // Indicates if private logging should be encrypted

        // Cache of log sources known to be private or not
        private static Dictionary<ApiLogSource, bool> privateLogSources = new Dictionary<ApiLogSource, bool>();

        // Log pattern
        private const string traceLayoutPattern = "%date{HH:mm:ss.fff} %-5level [%logger] [%thread] --> %message%newline";

        /// <summary>
        /// Initialize the configuration of the logger.  If sessionName is specified, it's appeneded to the default log folder name.
        /// </summary>
        public static void Initialize(string sessionName = "")
        {
            logPath = Path.Combine(DataPath.LogRoot, DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + (!string.IsNullOrWhiteSpace(sessionName) ? " - " + sessionName : ""));

            // The thing gets initialized in the static contructor... this ensures it
            DynamicQueryEngine.ArithAbortOn = DynamicQueryEngine.ArithAbortOn ? true : false;

            // Prviate logging is not encrypted for interapptive users
            IsPrivateLoggingEncrypted = !InterapptiveOnly.IsInterapptiveUser;

            logOptions = LoadLogOptions();
            ApplyLogOptions();

            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection());
        }

        /// <summary>
        /// Provide method for background process to register thread to clean up logs
        /// </summary>
        public static void RegisterLogCleanup()
        {
            // Check on idle to see if an logs are old enough to delete.  Run at most once an hour.
            IdleWatcher.RegisterDatabaseIndependentWork("LogCleanup", CleanupThread, TimeSpan.FromHours(1));
        }

        /// <summary>
        /// Configure logging based on the given log options
        /// </summary>
        public static void Configure(LogOptions logOptions)
        {
            // Save the new options
            LogSession.logOptions = logOptions;
            SaveLogOptions(logOptions);

            ApplyLogOptions();
        }

        /// <summary>
        /// Gets a copy of the current effective LogOptions.
        /// </summary>
        public static LogOptions Options => new LogOptions(logOptions);

        /// <summary>
        /// Path to the root of logging for this session.
        /// </summary>
        public static string LogFolder
        {
            get
            {
                Directory.CreateDirectory(logPath);
                return logPath; 
            }
        }

        /// <summary>
        /// Indicates if the given log source should be encrypted when its logged
        /// </summary>
        public static bool IsApiLogSourceEncrypted(ApiLogSource logSource)
        {
            bool isPrivate;
            if (!privateLogSources.TryGetValue(logSource, out isPrivate))
            {
                isPrivate = Attribute.IsDefined(logSource.GetType().GetField(logSource.ToString()), typeof(ApiPrivateLogSourceAttribute));
                privateLogSources[logSource] = isPrivate;
            }

            return isPrivate && IsPrivateLoggingEncrypted;
        }

        /// <summary>
        /// Gets \ sets wether private (Interapptive only) logging is encrypted
        /// </summary>
        public static bool IsPrivateLoggingEncrypted { get; set; } = true;

        /// <summary>
        /// Indicates if the given log source should be logged
        /// </summary>
        public static bool IsApiLogSourceEnabled(ApiLogSource logSource)
        {
            return logOptions.LogServices;
        }

        /// <summary>
        /// Determines if the given Api call should be logged based on the Action Type
        /// </summary>
        public static bool IsApiLogActionTypeEnabled(LogActionType logActionType)
        {
            switch (logActionType)
            {
                case LogActionType.GetRates:
                    return logOptions.LogRateCalls;
                case LogActionType.ExtendedLogging:
                    return !IsPrivateLoggingEncrypted;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Apply the configured log options
        /// </summary>
        private static void ApplyLogOptions()
        {
            log4net.LogManager.ResetConfiguration();

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new DefaultTraceListener());
            
            // Turns on console logging
            BasicConfigurator.Configure(CreateTraceAppender());

            // Add in application tracing
            if (logOptions.LogShipWorks)
            {
                RollingFileAppender appender = CreateFileAppender("shipworks.log", traceLayoutPattern);
                ApplyShipWorksLogging(appender);

                BasicConfigurator.Configure(appender);
            }

            DynamicQueryEngine.Switch.Level = TraceLevel.Off;
        }

        /// <summary>
        /// Aply the logging options for logging sw runtime events
        /// </summary>
        private static void ApplyShipWorksLogging(AppenderSkeleton appender)
        {
            // We only want info and above in the file
            LevelRangeFilter levelFilter = new LevelRangeFilter();

            levelFilter.LevelMin = logOptions.MinLevel;
            levelFilter.LevelMax = logOptions.MaxLevel;

            levelFilter.ActivateOptions();

            appender.AddFilter(levelFilter);
        }

        /// <summary>
        /// Create the main log file
        /// </summary>
        private static RollingFileAppender CreateFileAppender(string logFileName, string layoutPattern)
        {
            RollingFileAppender appender = new RollingFileAppender();

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = layoutPattern;
            layout.ActivateOptions();
            appender.Layout = layout;

            // Appened to existing files, rolling over on date\size boundries
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;

            // Newest rollovers have the highest number, with no rollover limit
            appender.CountDirection = 1;
            appender.MaxSizeRollBackups = -1;

            // Rollover boundries
            appender.MaxFileSize = 10 * 1024 * 1024;
            appender.DatePattern = "MM-dd";
            appender.StaticLogFileName = true;

            // The file to which to log
            appender.File = Path.Combine(LogFolder, logFileName);
            appender.ActivateOptions();

            return appender;
        }

        /// <summary>
        /// Create 
        /// </summary>
        private static IAppender CreateTraceAppender()
        {
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = traceLayoutPattern;
            layout.ActivateOptions();

            CleanTraceAppender appender = new CleanTraceAppender();
            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }

        /// <summary>
        /// Load the persisted configured log options
        /// </summary>
        private static LogOptions LoadLogOptions()
        {
            return !File.Exists(filename) ? new LogOptions() : LogOptions.Load(filename);
        }

        /// <summary>
        /// Save this LogOptions instance to file.
        /// </summary>
        private static void SaveLogOptions(LogOptions options)
        {
            options.Save(filename);

            // In case the cleanup options got tighter, cleanup now
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(delegate { CleanupThread(); }));
        }

        /// <summary>
        /// Runs on a schedule to see if any log files are in need of deleting.
        /// </summary>
        private static void CleanupThread()
        {
            try
            {
                if (Options.MaxLogAgeDays <= 0)
                {
                    log.Info("LogCleanup: Never");
                    return;
                }

                DateTime logExpirationDate = GetLogExpirationDate();

                DirectoryInfo logRoot = new DirectoryInfo(DataPath.LogRoot);

                // Look at each entry in the log directory
                foreach (FileSystemInfo fsi in logRoot.GetFileSystemInfos())
                {
                    // Never delete the current log entry
                    if (fsi.Name == new DirectoryInfo(LogFolder).Name)
                    {
                        continue;
                    }

                    // Deletes the log entry if the entry, and its contents, are out of date.
                    CleanupLogEntry(fsi, logExpirationDate);

                    // Quit if we leave the idle state
                    if (!IdleWatcher.IsIdle)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex) when (ex is SecurityException || ex is DirectoryNotFoundException)
            {
                log.Error("Failed during log cleanup.", ex);
            }
        }

        /// <summary>
        /// Checks to see if the log entry is past the log expiration date and deletes the entry if it is
        /// </summary>
        private static void CleanupLogEntry(FileSystemInfo fsi, DateTime logExpirationDate)
        {
            if (IsLogEntryExpired(fsi, logExpirationDate))
            {
                try
                {
                    // See if its a folder.  Make sure all entries are old enough
                    DirectoryInfo di = fsi as DirectoryInfo;
                    if (di != null)
                    {
                        CleanupLogEntryChildren(di, logExpirationDate, fsi);
                    }
                    else
                    {
                        log.InfoFormat("Deleting file '{0}'", fsi.Name);
                        fsi.Delete();
                    }
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    log.Error("Failed deleting log entry.", ex);
                }

                // If there's a bunch of stuff to delete, we don't want to peg the cpu
                Thread.Sleep(2);
            }
        }

        /// <summary>
        /// Determines if the log entry has expired
        /// </summary>
        private static bool IsLogEntryExpired(FileSystemInfo fsi, DateTime logExpirationDate)
        {
            bool isExpired = true;
            try
            {
                // Created far enough back to delete
                if (fsi.CreationTimeUtc >= logExpirationDate)
                {
                    isExpired = false;
                }
            }
            catch (ArgumentOutOfRangeException ex) when (ex.Message.StartsWith("Not a valid Win32 FileTime", true,
                CultureInfo.InvariantCulture))
            {
                // A crash was occuring when attempting to access invalid timestamps. We are eating the exception
                // and continuing to delete the file, since it is most likely corrupt.
                isExpired = true;
                log.Error("File or directory has an invalid Windows timestamp. ShipWorks will attempt to delete it, " +
                          "as it is likely corrupt.");
            }

            return isExpired;
        }

        /// <summary>
        /// Checks if all of the directories contents are past the log expiration date and deletes the directory if they are
        /// </summary>
        private static void CleanupLogEntryChildren(DirectoryInfo di, DateTime logExpirationDate, FileSystemInfo fsi)
        {
            foreach (FileSystemInfo childFsi in di.GetFileSystemInfos())
            {
                // If any file in the folder has been written to in the proper amount of time, then dont delete the directory
                if (!IsLogEntryChildExpired(childFsi, logExpirationDate))
                {
                    return;
                }
            }
            
            log.InfoFormat("Deleting log '{0}'", fsi.Name);
            Directory.Delete(fsi.FullName, true);
        }

        /// <summary>
        /// Determines whether the given log entry child has expired
        /// </summary>
        private static bool IsLogEntryChildExpired(FileSystemInfo childFsi, DateTime logExpirationDate)
        {
            bool isExpired = true;
            try
            {
                // The file is considered expired if it has not been modified since the expiration date (not created date)
                if (childFsi.LastWriteTimeUtc >= logExpirationDate)
                {
                    isExpired = false;
                }
            }
            catch (ArgumentOutOfRangeException ex) when (ex.Message.StartsWith("Not a valid Win32 FileTime", true,
                CultureInfo.InvariantCulture))
            {
                // A crash was occuring when attempting to access invalid timestamps. We are eating the exception
                // and continuing to delete the file, since it is most likely corrupt.
                log.Error("File found with an invalid Windows timestamp. ShipWorks will continue " +
                          "to check if this directory should be deleted.");
            }

            return isExpired;
        }

        /// <summary>
        /// Gets the maximum date that logs are allowed to live, without ShipWorks trying to delete them
        /// </summary>
        private static DateTime GetLogExpirationDate()
        {
            DateTime expirationDate = DateTime.UtcNow - TimeSpan.FromDays(Options.MaxLogAgeDays);

            // Date a log must be to survive
            log.InfoFormat("Deleting logs older than {0}.", expirationDate.ToLocalTime());
            return expirationDate;
        }
    }
}
