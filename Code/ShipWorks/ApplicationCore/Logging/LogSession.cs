using System;
using System.Collections.Generic;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using ShipWorks.Common.Threading;
using System.Diagnostics;
using System.IO;
using SD.LLBLGen.Pro.DQE.SqlServer;
using System.Threading;
using ShipWorks.ApplicationCore.Interaction;
using System.Security;
using Common.Logging.Log4Net;
using Common.Logging;
using Interapptive.Shared;
using NameValueCollection = Common.Logging.Configuration.NameValueCollection;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Class for managing our logging configuration
    /// </summary>
    public static class LogSession
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(LogSession));

        // File from which to save and restore settings
        static readonly string filename = Path.Combine(DataPath.InstanceSettings, "logging.xml");

        // Base path for log files for this session
        static string logPath;

        // Current logging options
        static LogOptions logOptions = new LogOptions();

        // Indicates if private logging should be encrypted
        static bool privateLoggingEncrypted = true;

        // Cache of log sources known to be private or not
        static Dictionary<ApiLogSource, bool> privateLogSources = new Dictionary<ApiLogSource, bool>();

        // Log pattern
        const string traceLayoutPattern = "%date{HH:mm:ss.fff} %-5level [%logger] [%thread] --> %message%newline";
        const string queryLayoutPattern = "%date{HH:mm:ss.fff} [%thread] %message%newline";

        /// <summary>
        /// Initialize the configuration of the logger.  If sessionName is specified, it's appeneded to the default log folder name.
        /// </summary>
        public static void Initialize(string sessionName = "")
        {
            logPath = Path.Combine(DataPath.LogRoot, DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + (!string.IsNullOrWhiteSpace(sessionName) ? " - " + sessionName : ""));

            // The thing gets initialized in the static contructor... this ensures it
            DynamicQueryEngine.ArithAbortOn = DynamicQueryEngine.ArithAbortOn ? true : false;

            // Prviate logging is not encrypted for interapptive users
            privateLoggingEncrypted = !InterapptiveOnly.IsInterapptiveUser;

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
        public static LogOptions Options
        {
            get
            {
                return new LogOptions(logOptions);
            }
        }

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

            return isPrivate && privateLoggingEncrypted;
        }

        /// <summary>
        /// Gets \ sets wether private (Interapptive only) logging is encrypted
        /// </summary>
        public static bool IsPrivateLoggingEncrypted
        {
            get { return privateLoggingEncrypted; }
            set { privateLoggingEncrypted = value; }
        }

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
            // appender.AddFilter(new LoggerMatchFilter { LoggerToMatch = typeof(PagedEntityGrid).FullName, AcceptOnMatch = false });
            // appender.AddFilter(new LoggerMatchFilter { LoggerToMatch = typeof(PagedEntityGateway).FullName, AcceptOnMatch = false });
            appender.ActivateOptions();

            return appender;
        }

        /// <summary>
        /// Load the persisted configured log options
        /// </summary>
        private static LogOptions LoadLogOptions()
        {
            if (!File.Exists(filename))
            {
                return new LogOptions();
            }
            else
            {
                return LogOptions.Load(filename);
            }
        }

        /// <summary>
        /// Save this LogOptions instance to file.
        /// </summary>
        private static void SaveLogOptions(LogOptions options)
        {
            options.Save(filename);

            // In case the cleanup options got tighter, cleanup now
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem((WaitCallback) delegate { CleanupThread(); }));
        }

        /// <summary>
        /// Runs on a schedule to see if any log files are in need of deleting.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void CleanupThread()
        {
            try
            {
                string current = new DirectoryInfo(LogFolder).Name;
                LogOptions options = LogSession.Options;

                if (options.MaxLogAgeDays <= 0)
                {
                    log.Info("LogCleanup: Never");
                    return;
                }

                // Date a log must be to survive
                DateTime mustBeDate = DateTime.UtcNow - TimeSpan.FromDays(options.MaxLogAgeDays);
                log.InfoFormat("Deleting logs older than {0}.", mustBeDate.ToLocalTime());

                DirectoryInfo logRoot = new DirectoryInfo(DataPath.LogRoot);

                // Look at each entry in the log directory
                foreach (FileSystemInfo fsi in logRoot.GetFileSystemInfos())
                {
                    // Never delete the current
                    if (fsi.Name == current)
                    {
                        continue;
                    }

                    // Created far enough back to delete
                    if (fsi.CreationTimeUtc < mustBeDate)
                    {
                        try
                        {
                            // See if its a folder.  Make sure all entries are old enough
                            DirectoryInfo di = fsi as DirectoryInfo;
                            if (di != null)
                            {
                                bool delete = true;

                                // If any file in the folder has been written too in the proper amount of time, then dont delete the directory
                                foreach (FileSystemInfo childFsi in di.GetFileSystemInfos())
                                {
                                    if (childFsi.LastWriteTimeUtc >= mustBeDate)
                                    {
                                        delete = false;
                                        break;
                                    }
                                }

                                if (delete)
                                {
                                    log.InfoFormat("Deleting log '{0}'", fsi.Name);
                                    Directory.Delete(fsi.FullName, true);
                                }
                            }
                            else
                            {
                                log.InfoFormat("Deleting file '{0}'", fsi.Name);
                                fsi.Delete();
                            }
                        }
                        catch (IOException ex)
                        {
                            log.Error("Failed deleting log entry.", ex);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            log.Error("Failed deleting log entry.", ex);
                        }

                        // If there's a bunch of stuff to delete, we don't want to peg the cpu
                        Thread.Sleep(2);
                    }

                    // Quit if we leave the idle state
                    if (!IdleWatcher.IsIdle)
                    {
                        return;
                    }
                }
            }
            catch (SecurityException ex)
            {
                log.Error("Failed during log cleanup.", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                log.Error("Failed during log cleanup.", ex);
            }
        }
    }
}
