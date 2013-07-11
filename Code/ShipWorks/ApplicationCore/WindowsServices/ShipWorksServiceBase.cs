using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Timers;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    public partial class ShipWorksServiceBase : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceBase));

        private WindowsServiceEntity windowsServiceEntity;

        public ShipWorksServiceBase()
        {
            InitializeComponent();
        }

        [Description("The ShipWorks service type that this service implements.")]
        public ShipWorksServiceType ServiceType { get; set; }

        [Browsable(false)]
        public string BaseServiceName { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public string ServiceName
        {
            get { return base.ServiceName; }
            private set { base.ServiceName = value; }
        }

        /// <summary>
        /// Attempts to get the WindowsServiceEntity for this Computer and ServiceType
        /// </summary>
        private WindowsServiceEntity CurrentWindowsServiceEntity
        {
            get
            {
                if (windowsServiceEntity == null)
                {
                    windowsServiceEntity = WindowsServiceManager.GetWindowsService(UserSession.Computer.ComputerID, ServiceType);
                }

                return windowsServiceEntity;
            }
        }

        /// <summary>
        /// Sets instance-specific properties for this service; must be called after InitializeComponent in a derived service class.
        /// </summary>
        protected void InitializeInstance()
        {
            BaseServiceName = "ShipWorks" + ServiceType;
            ServiceName = BaseServiceName + "$" + ShipWorksSession.InstanceID.ToString("N");
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control 
        /// Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to 
        /// take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected sealed override void OnStart(string[] args)
        {
            TryStart();
        }

        /// <summary>
        /// Tries to connect to the database, if successful, starts up the service core.
        /// </summary>
        void TryStart()
        {
            if (!TryConnect())
            {
                tryStartTimer.Start();
                return;
            }

            OnStartCore();
        }

        /// <summary>
        /// Tries to connect to the database.
        /// </summary>
        bool TryConnect()
        {
            try
            {
                SqlSession.Initialize();

                if (!SqlSession.IsConfigured)
                {
                    log.Warn("SqlSession is not configured.");
                    return false;
                }

                if (!SqlSchemaUpdater.IsCorrectSchemaVersion())
                {
                    log.Warn("Schema is not the correct version.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error establishing SqlSession.", ex);
                return false;
            }
        }

        void OnTryStartTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TryStart();
        }

        /// <summary>
        /// Performs core startup once a SqlSession has been established.
        /// </summary>
        protected virtual void OnStartCore()
        {
            // Setup the service for ShipWorks access.
            InitializeForApplication();

            // Update the WindowsServiceEntity start and check in times.
            CurrentWindowsServiceEntity.LastStartDateTime = DateTime.UtcNow;
            CurrentWindowsServiceEntity.LastCheckInDateTime = CurrentWindowsServiceEntity.LastStartDateTime;
            CurrentWindowsServiceEntity.ServiceFullName = ServiceName;

            using (ServiceController serviceController = new ServiceController(ServiceName))
            {
                CurrentWindowsServiceEntity.ServiceDisplayName = serviceController.DisplayName;
            }

            WindowsServiceManager.SaveWindowsService(CurrentWindowsServiceEntity);

            // Start the timer to check in every WindowsServiceManager.CheckInTimeSpan
            checkInTimer.Interval = WindowsServiceManager.CheckInTimeSpan.TotalMilliseconds;
            checkInTimer.Start();
        }

        static void InitializeForApplication()
        {
            LogSession.Initialize();

            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            UserSession.InitializeForCurrentDatabase();

            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentUser();
        }

        void OnCheckInTimerElapsed(object sender, ElapsedEventArgs e)
        {
            WindowsServiceManager.CheckIn(CurrentWindowsServiceEntity);
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            checkInTimer.Stop();

            // Update the WindowsServiceEntity stop and check in times.
            CurrentWindowsServiceEntity.LastStopDateTime = DateTime.UtcNow;
            CurrentWindowsServiceEntity.LastCheckInDateTime = CurrentWindowsServiceEntity.LastStopDateTime;
            WindowsServiceManager.SaveWindowsService(CurrentWindowsServiceEntity);
        }
    }
}
