using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    [DesignerCategory("")]
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
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control 
        /// Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to 
        /// take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            // Update the WindowsServiceEntity start and check in times.
            CurrentWindowsServiceEntity.LastStartDateTime = DateTime.UtcNow;
            CurrentWindowsServiceEntity.LastCheckInDateTime = CurrentWindowsServiceEntity.LastStartDateTime;
            CurrentWindowsServiceEntity.ServiceFullName = ServiceName;

            using (ServiceController serviceController = new ServiceController(ServiceName))
            {
                CurrentWindowsServiceEntity.ServiceDisplayName = serviceController.DisplayName;
            }

            WindowsServiceManager.SaveWindowsService(CurrentWindowsServiceEntity);

            // Start a thread to check in every WindowsServiceManager.CheckInTimeSpan
            Task.Factory.StartNew(ServiceCheckInWorker);
        }

        /// <summary>
        /// Worker thread for checking in the service
        /// </summary>
        private void ServiceCheckInWorker()
        {
            if (CurrentWindowsServiceEntity == null)
            {
                throw new ArgumentNullException(string.Format("{0} was unable to find a ShipWorks WindowsServiceEntity.", ServiceName));
            }

            windowsServiceCheckInTimer.Enabled = true;
            windowsServiceCheckInTimer.Interval = WindowsServiceManager.CheckInTimeSpan.TotalMilliseconds;
            windowsServiceCheckInTimer.Elapsed += OnWindowsServiceCheckInTimerElapsed;
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
