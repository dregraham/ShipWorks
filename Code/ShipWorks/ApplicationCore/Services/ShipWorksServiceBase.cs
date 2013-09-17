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
        SqlSessionConfiguration lastConfiguration = null;
        bool lastConfigurationSuccess = false;

        // Lock to make sure we aren't updating at the same time
        object timerLock = new object();

        /// <summary>
        /// Consteructor
        /// </summary>
        public ShipWorksServiceBase()
        {
            InitializeComponent();

            // Set the checkin timer interval
            checkInTimer.Interval = ServiceStatusManager.CheckInTimeSpan.TotalMilliseconds;

            // See if SQL Session has changed every 5 seconds
            sqlSessionMonitorTimer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
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
            // Start the SQL Session monitor timer for monitoring for SQL Session changes
            sqlSessionMonitorTimer.Start();

            // Kick off the first one right away
            OnSqlSessionMonitorTimerElapsed(null, null);
        }

        /// <summary>
        /// The timer for retrying the connection elapsed
        /// </summary>
        private void OnSqlSessionMonitorTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!Monitor.TryEnter(timerLock))
            {
                return;
            }

            try
            {
                // Are we running on entering this method
                bool isRunning = checkInTimer.Enabled;

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
                        checkInTimer.Start();
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
                        sqlSessionMonitorTimer.Start();
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
        private bool CheckSqlSession()
        {
            try
            {
                // Reload the session
                SqlSession.Initialize();

                // If the session hasn't changed, and it was succesful last time, no need to go through all of this again
                if (!HasSqlSessionChanged() && lastConfigurationSuccess)
                {
                    log.DebugFormat("SQL Session has not changed since last check");

                    return true;
                }

                // Update the lastConfig to the current one, and assume it won't be succesful (until it is)
                lastConfiguration = SqlSession.IsConfigured ? SqlSession.Current.Configuration : null;
                lastConfigurationSuccess = false;

                // The session has changed.  So first we need to clear the previous session info
                ClearUserSession();

                // SQL Sesion isn't configured
                if (!SqlSession.IsConfigured)
                {
                    lastConfiguration = null;

                    log.Warn("SqlSession is not configured.");
                    return false;
                }

                // If the database is in SINGLE_USER, don't even try to connect
                SqlSession master = new SqlSession(SqlSession.Current);
                master.Configuration.DatabaseName = "master";
                using (SqlConnection testConnection = new SqlConnection(master.Configuration.GetConnectionString()))
                {
                    testConnection.Open();

                    if (SqlUtility.IsSingleUser(testConnection, SqlSession.Current.Configuration.DatabaseName))
                    {
                        log.WarnFormat("Database {0} is in SINGLE_USER... leaving it alone.", SqlSession.Current.Configuration.DatabaseName);

                        return false;
                    }
                }

                if (!SqlSchemaUpdater.IsCorrectSchemaVersion())
                {
                    log.Warn("Schema is not the correct version.");
                    return false;
                }

                if (StoreManager.GetDatabaseStoreCount() == 0)
                {
                    log.Warn("There are no stores, nothing to do");
                    return false;
                }

                DataProvider.InitializeForApplication();
                AuditProcessor.InitializeForApplication();

                UserSession.InitializeForCurrentDatabase();

                UserManager.InitializeForCurrentUser();
                UserSession.InitializeForCurrentSession();

                // This is the only spot we know it was a success
                lastConfigurationSuccess = true;
                return true;
            }
            catch (SqlException ex)
            {
                log.Error("Error establishing SqlSession.", ex);

                return false;
            }
        }

        /// <summary>
        /// Indicates if the SQL Session has changed since the last time this function was called
        /// </summary>
        private bool HasSqlSessionChanged()
        {
            SqlSessionConfiguration currentConfiguration = SqlSession.IsConfigured ? SqlSession.Current.Configuration : null;

            // They are both null, haven't changed
            if (lastConfiguration == null && currentConfiguration == null)
            {
                return false;
            }

            // They are both not null, check the properties
            if (lastConfiguration != null && currentConfiguration != null)
            {
                // Same server and database
                if (lastConfiguration.ServerInstance == currentConfiguration.ServerInstance && lastConfiguration.DatabaseName == currentConfiguration.DatabaseName)
                {
                    // Same auth (windows) so we can get out
                    if (lastConfiguration.WindowsAuth && currentConfiguration.WindowsAuth)
                    {
                        return false;
                    }

                    // Same auth (password) so we can get out
                    if (!lastConfiguration.WindowsAuth && !currentConfiguration.WindowsAuth &&
                        lastConfiguration.Username == currentConfiguration.Username &&
                        lastConfiguration.Password == currentConfiguration.Password)
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
        void OnCheckInTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
        }

        /// <summary>
        /// Stops the service core, if it was started.
        /// </summary>
        protected sealed override void OnStop()
        {
            sqlSessionMonitorTimer.Stop();

            // If the checkin timer is on, that means we were running.
            if (checkInTimer.Enabled)
            {
                // Stop the timer
                checkInTimer.Stop();

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
            // Could be stopping due to losing the connection
            if (lastConfigurationSuccess)
            {
                // Update the ServiceStatusEntity stop and check in times.
                CurrentServiceStatusEntity.LastStopDateTime = DateTime.UtcNow;

                // Check in with the stop time
                ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
            }
        }
    }
}
