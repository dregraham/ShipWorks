using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("")]
    public class ShipWorksServiceBase : ServiceBase
    {
        private Timer windowsServiceCheckInTimer;
        private WindowsServiceEntity windowsServiceEntity;

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
        /// Set properties for this service.
        /// </summary>
        protected void InitializeInstance()
        {
            BaseServiceName = "ShipWorks" + ServiceType;
            ServiceName = BaseServiceName + "$" + ShipWorksSession.InstanceID.ToString("N");
            windowsServiceCheckInTimer = new Timer();
        }

        /// <summary>
        /// Initializes for application.
        /// </summary>
        private static void InitializeForApplication()
        {
            SqlSession.Initialize();
            LogSession.Initialize();

            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            UserSession.InitializeForCurrentDatabase();

            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentUser();

            // Required for printing
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));

            // Initialize any ShipWorks Windows services
            WindowsServiceManager.InitializeForCurrentUser();
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control 
        /// Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to 
        /// take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
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
            StartCheckInTimer();
        }

        /// <summary>
        /// Configures the check-in timer for the service.
        /// </summary>
        private void StartCheckInTimer()
        {
            windowsServiceCheckInTimer.Interval = WindowsServiceManager.CheckInTimeSpan.TotalMilliseconds;
            windowsServiceCheckInTimer.Elapsed += OnWindowsServiceCheckInTimerElapsed;
            windowsServiceCheckInTimer.Enabled = true;
        }

        /// <summary>
        /// Timer elapsted event handler to do the actual work of updating the server check in time.
        /// </summary>
        private void OnWindowsServiceCheckInTimerElapsed(object sender, ElapsedEventArgs e)
        {
            WindowsServiceManager.CheckIn(CurrentWindowsServiceEntity);
        }

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            windowsServiceCheckInTimer.Stop();
            windowsServiceCheckInTimer.Close();
            windowsServiceCheckInTimer.Dispose();

            // Update the WindowsServiceEntity stop and check in times.
            CurrentWindowsServiceEntity.LastStopDateTime = DateTime.UtcNow;
            CurrentWindowsServiceEntity.LastCheckInDateTime = CurrentWindowsServiceEntity.LastStopDateTime;
            WindowsServiceManager.SaveWindowsService(CurrentWindowsServiceEntity);
        }
    }
}
