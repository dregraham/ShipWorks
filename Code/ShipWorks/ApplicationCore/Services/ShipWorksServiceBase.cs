using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Stores;
using ShipWorks.ApplicationCore.Interaction;
using ThreadTimer = System.Threading.Timer;
using Interapptive.Shared.UI;
using System.IO;
using Interapptive.Shared;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Base class for all ShipWorks services.  Right now there is only one, so really we could just have ShipWorskSchedulerService.
    /// </summary>
    public partial class ShipWorksServiceBase : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceBase));

        // The entity we use to ping the database with checkins
        ServiceStatusEntity serviceStatusEntity;

        // The last known SQL Session configuration
        bool lastConfigurationSuccess = false;

        // Lock to make sure we aren't updating at the same time
        object timerLock = new object();

        // The timers to keep us checking for sql changes
        ThreadTimer sqlSessionMonitorTimer;
        ThreadTimer checkInTimer;

        // The timespan for checking our connection
        static readonly TimeSpan sqlMonitorTimespan = TimeSpan.FromSeconds(5);

        // For log throttling so we don't fill up the background log so fast
        string lastWarnMessage = null;
        Stopwatch lastWarnMessageTime = Stopwatch.StartNew();

        /// <summary>
        /// Consteructor
        /// </summary>
        public ShipWorksServiceBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets instance-specific properties for this service; must be called after InitializeComponent in a derived service class.
        /// </summary>
        protected void InitializeInstance()
        {
            ServiceName = ShipWorksServiceManager.GetServiceName(ServiceType);
        }

        /// <summary>
        /// The ShipWorks Service type
        /// </summary>
        [Description("The ShipWorks service type that this service implements.")]
        public ShipWorksServiceType ServiceType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The name of the service, as displayed as the name in the SCM
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public string ServiceName
        {
            get { return base.ServiceName; }
            private set { base.ServiceName = value; }
        }

        /// <summary>
        /// Attempts to get the ServiceStatusEntity for this Computer and ServiceType
        /// </summary>
        private ServiceStatusEntity CurrentServiceStatusEntity
        {
            get
            {
                if (serviceStatusEntity == null)
                {
                    serviceStatusEntity = ServiceStatusManager.GetServiceStatus(UserSession.Computer.ComputerID, ServiceType);
                }

                return serviceStatusEntity;
            }
        }

        /// <summary>
        /// Provides access to initiating the "OnStart" method from a custom host
        /// </summary>
        public void InternalHostStart()
        {
            OnStart(null);
        }

        /// <summary>
        /// Provides access to initiating the "OnStop" method from a custom host
        /// </summary>
        public void InternalHostStop()
        {
            OnStop();
        }

        /// <summary>
        /// Begins trying to connect to the database.
        /// </summary>
        protected sealed override void OnStart(string[] args)
        {
            // One time initialization ever - regardless of database or user changes
            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            // Required for printing
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));

            // Start the primary timer
            sqlSessionMonitorTimer = new ThreadTimer(OnSqlSessionMonitorTimerElapsed, null, TimeSpan.Zero, sqlMonitorTimespan);
        }

        /// <summary>
        /// The timer for retrying the connection elapsed
        /// </summary>
        private void OnSqlSessionMonitorTimerElapsed(object state)
        {
            if (!Monitor.TryEnter(timerLock))
            {
                return;
            }

            try
            {
                // Are we running on entering this method
                bool isRunning = checkInTimer != null;

                // Check the SQL Session, and determine if it changed
                bool hasChanged = CheckSqlSession();

                // If the SQL Session is good, and we weren't already running, start it up
                if (lastConfigurationSuccess)
                {
                    if (!isRunning)
                    {
                        // Do our core starting initialization
                        OnStartCore();

                        // Start the timer to periodically checkin
                        checkInTimer = new ThreadTimer(OnCheckInTimerElapsed, null, ServiceStatusManager.CheckInTimeSpan, ServiceStatusManager.CheckInTimeSpan);
                    }
                        // It's already running, but the SQL config changed - we need to make sure the scheduler is now pointing to the correct database
                    else if (hasChanged)
                    {
                        OnSqlConfigurationChanged();
                    }
                }
                else
                {
                    if (isRunning)
                    {
                        // Not connected - stop
                        OnStop();

                        // Stop also stops the sql monitor timer - we need that to keep going so we can detect when it gets fixed
                        sqlSessionMonitorTimer = new ThreadTimer(OnSqlSessionMonitorTimerElapsed, null, sqlMonitorTimespan, sqlMonitorTimespan);
                    }
                }
            }
            finally
            {
                Monitor.Exit(timerLock);
            }
        }

        /// <summary>
        /// Check SQL Session for changes.  Updates 'lastConfiguration' and 'lastConfigurationSuccess', and returns true if the session has changed.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool CheckSqlSession()
        {
            // Reloading the SQL Session and any other changes that may cause have to be within a connection scope, so we don't try to do it while other things are updating,
            // and other things don't try to update while we are doing it.
            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("checking connection", null))
            {
                if (!scope.Acquired)
                {
                    LogThrottledWarn("Could not acquire the connection scope to check for SQL Session updates: " + string.Join(", ", ApplicationBusyManager.GetActiveOperations()));
                    return false;
                }

                // Load a SQL Session from disk
                SqlSessionConfiguration diskConfiguration = new SqlSessionConfiguration();
                diskConfiguration.Load();

                // See if the SQL Session has changed
                bool hasChanged = HasSqlSessionChanged(diskConfiguration);

                // If it's changed, reload it
                if (hasChanged)
                {
                    SqlSession.Initialize();

                    // Make sure that change tracking is enabled for the database and all applicable tables.
                    SqlChangeTracking sqlChangeTracking = new SqlChangeTracking();
                    sqlChangeTracking.Enable();
                }

                try
                {
                    // If the session hasn't changed, and it was succesful last time, no need to go through all of this again
                    if (!hasChanged && lastConfigurationSuccess)
                    {
                        log.DebugFormat("SQL Session has not changed since last check");

                        return false;
                    }

                    lastConfigurationSuccess = false;

                    // The session has changed.  So first we need to clear the previous session info
                    ClearUserSession();

                    // SQL Sesion isn't configured
                    if (!SqlSession.IsConfigured)
                    {
                        LogThrottledWarn("SqlSession is not configured.");
                        return hasChanged;
                    }

                    // If the database is in SINGLE_USER, don't even try to connect
                    SqlSession master = new SqlSession(SqlSession.Current);
                    master.Configuration.DatabaseName = "master";
                    using (SqlConnection testConnection = new SqlConnection(master.Configuration.GetConnectionString()))
                    {
                        testConnection.Open();

                        if (SqlUtility.IsSingleUser(testConnection, SqlSession.Current.Configuration.DatabaseName))
                        {
                            LogThrottledWarn(string.Format("Database {0} is in SINGLE_USER... leaving it alone.", SqlSession.Current.Configuration.DatabaseName));

                            return hasChanged;
                        }
                    }

                    if (!SqlSchemaUpdater.IsCorrectSchemaVersion())
                    {
                        LogThrottledWarn("Schema is not the correct version.");
                        return hasChanged;
                    }

                    if (MigrationController.IsMigrationInProgress())
                    {
                        LogThrottledWarn("A ShipWorks 2 upgrade is in progress.");
                        return hasChanged;
                    }

                    if (StoreManager.GetDatabaseStoreCount() == 0)
                    {
                        LogThrottledWarn("There are no stores, nothing to do");
                        return hasChanged;
                    }

                    UserSession.InitializeForCurrentDatabase();

                    UserManager.InitializeForCurrentUser();
                    UserSession.InitializeForCurrentSession();

                    log.InfoFormat("The user session has been successfully loaded and initialized.");

                    // This is the only spot we know it was a success
                    lastConfigurationSuccess = true;
                    return hasChanged;
                }
                catch (InvalidShipWorksDatabaseException ex)
                {
                    log.Error("Invalid ShipWorks Database", ex);

                    return hasChanged;
                }
                catch (SqlException ex)
                {
                    log.Error("An error occurred while connecting to SQL Server.", ex);

                    return hasChanged;
                }
                catch (ConnectionLostException ex)
                {
                    log.Error("Connection to ShipWorks Database was lost.", ex);

                    return hasChanged;
                }
            }
        }

        /// <summary>
        /// Indicates if the SQL Session has changed since the last time this function was called
        /// </summary>
        private bool HasSqlSessionChanged(SqlSessionConfiguration diskConfiguration)
        {
            SqlSessionConfiguration currentConfiguration = SqlSession.IsConfigured ? SqlSession.Current.Configuration : new SqlSessionConfiguration();

            // They are both null, haven't changed
            if (diskConfiguration == null && currentConfiguration == null)
            {
                return false;
            }

            // They are both not null, check the properties
            if (diskConfiguration != null && currentConfiguration != null)
            {
                // Same server and database
                if (diskConfiguration.ServerInstance == currentConfiguration.ServerInstance && diskConfiguration.DatabaseName == currentConfiguration.DatabaseName)
                {
                    // Same auth (windows) so we can get out
                    if (diskConfiguration.WindowsAuth && currentConfiguration.WindowsAuth)
                    {
                        return false;
                    }

                    // Same auth (password) so we can get out
                    if (!diskConfiguration.WindowsAuth && !currentConfiguration.WindowsAuth &&
                        diskConfiguration.Username == currentConfiguration.Username &&
                        diskConfiguration.Password == currentConfiguration.Password)
                    {
                        return false;
                    }
                }

                // Something changed
                return true;
            }

            // One is null and one is not null, something changed
            return true;
        }

        /// <summary>
        /// Called when the service is in the middle of running, and the SQL Session configuration changes mid-stream
        /// </summary>
        protected virtual void OnSqlConfigurationChanged()
        {

        }

        /// <summary>
        /// Performs core startup once a SqlSession has been established.
        /// </summary>
        protected virtual void OnStartCore()
        {
            // Update the ServiceStatusEntity start and check in times.
            CurrentServiceStatusEntity.LastStartDateTime = DateTime.UtcNow;
            CurrentServiceStatusEntity.ServiceFullName = ServiceName;
            CurrentServiceStatusEntity.ServiceDisplayName = ShipWorksServiceManager.GetDisplayName(ServiceType);

            // Check in right away
            ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
        }

        /// <summary>
        /// The periodic check-in timer has elapsed
        /// </summary>
        void OnCheckInTimerElapsed(object state)
        {
            if (!UserSession.IsLoggedOn)
            {
                return;
            }

            ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
        }

        /// <summary>
        /// Stops the service core, if it was started.
        /// </summary>
        protected sealed override void OnStop()
        {
            if (sqlSessionMonitorTimer != null)
            {
                sqlSessionMonitorTimer.Dispose();
                sqlSessionMonitorTimer = null;
            }

            // If the checkin timer is on, that means we were running.
            if (checkInTimer != null)
            {
                // Stop the timer
                checkInTimer.Dispose();
                checkInTimer = null;

                // And shutdown
                OnStopCore();

                ClearUserSession();
            }
        }

        /// <summary>
        /// Clear the current user session
        /// </summary>
        private void ClearUserSession()
        {
            UserSession.Reset();
        }

        /// <summary>
        /// Performs core shutdown.  Only called if OnStartCore was called.
        /// </summary>
        protected virtual void OnStopCore()
        {
            if (Program.IsCrashing)
            {
                return;
            }

            // Could be stopping due to losing the connection
            if (lastConfigurationSuccess)
            {
                // Update the ServiceStatusEntity stop and check in times.
                CurrentServiceStatusEntity.LastStopDateTime = DateTime.UtcNow;

                // Check in with the stop time
                ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
            }
        }

        /// <summary>
        /// Log a warning, but in a throttled way that doesn't fill up our background log so fast
        /// </summary>
        private void LogThrottledWarn(string message)
        {
            lock (log)
            {
                // Log anytime the message changes, or the same message if a minute has passed since the last time
                if (message != lastWarnMessage || lastWarnMessageTime.Elapsed > TimeSpan.FromMinutes(1))
                {
                    log.Warn(message);

                    lastWarnMessage = message;
                    lastWarnMessageTime = Stopwatch.StartNew();
                }
            }
        }
    }
}
