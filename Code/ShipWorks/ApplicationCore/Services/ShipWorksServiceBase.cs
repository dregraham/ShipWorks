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

namespace ShipWorks.ApplicationCore.Services
{
    public partial class ShipWorksServiceBase : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceBase));
        static Type[] serviceTypesCache;

        /// <summary>
        /// Gets service instances for all ShipWorks service types.
        /// </summary>
        public static ShipWorksServiceBase[] GetAllServices()
        {
            if (serviceTypesCache == null)
            {
                serviceTypesCache = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.BaseType == typeof(ShipWorksServiceBase))
                    .ToArray();
            }

            return serviceTypesCache
                .Select(Activator.CreateInstance)
                .Cast<ShipWorksServiceBase>()
                .ToArray();
        }

        /// <summary>
        /// Gets a service instance for a ShipWorks service type.
        /// </summary>
        public static ShipWorksServiceBase GetService(ShipWorksServiceType serviceType)
        {
            return GetAllServices().Single(s => s.ServiceType == serviceType);
        }

        /// <summary>
        /// Gets a service instance for a ShipWorks service type name.
        /// </summary>
        public static ShipWorksServiceBase GetService(string serviceTypeName)
        {
            var results = EnumHelper.GetEnumList<ShipWorksServiceType>().Where(e => string.Compare(e.ApiValue, serviceTypeName, true) == 0).Select(e => e.Value);
            if (results.Count() != 1)
            {
                throw new NotFoundException("A ShipWorks service of the name '" + serviceTypeName + "' was not found.");
            }

            return GetService(results.First());
        }

        /// <summary>
        /// Gets the instance-specific service name for a ShipWorks service type.
        /// </summary>
        public static string GetServiceName(ShipWorksServiceType serviceType)
        {
            return GetServiceName(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Gets the instance-specific service name for a ShipWorks service type.
        /// </summary>
        public static string GetServiceName(ShipWorksServiceType serviceType, Guid instanceID)
        {
            return "ShipWorks" + serviceType + "$" + instanceID.ToString("N");
        }

        /// <summary>
        /// Gets the instance-specific display name for a ShipWorks service type.
        /// </summary>
        public static string GetDisplayName(ShipWorksServiceType serviceType)
        {
            return GetDisplayName(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Gets the instance-specific display name for a ShipWorks service type.
        /// </summary>
        public static string GetDisplayName(ShipWorksServiceType serviceType, Guid instanceID)
        {
            return EnumHelper.GetDescription(serviceType) + " " + instanceID.ToString("B").ToUpper();
        }

        /// <summary>
        /// Self-hosts a ShipWorks service object as an instance-specific background process instead of via the SCM.
        /// If the background process is already running, it is signaled to stop and will be superseded by this instance.
        /// This is a blocking call for the lifetime of the running service.
        /// </summary>
        /// <returns>true if the service ran; otherwise, false.</returns>
        /// <returns>true if the service ran to completion in the background; false if the service was already running and could not be stopped.</returns>
        public static bool RunInBackground(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");

            // Suppress the nappy-headed Windows error dialogs
            NativeMethods.SetErrorMode(NativeMethods.SEM_FAILCRITICALERRORS | NativeMethods.SEM_NOGPFAULTERRORBOX);

            bool createdNew;
            using (var stopSignal = new EventWaitHandle(false, EventResetMode.ManualReset, service.ServiceName, out createdNew))
            {
                if (!createdNew)
                {
                    log.Warn("Service is already running in the background; sending stop signal.");
                    if (!stopSignal.Set())
                    {
                        // MSDN gives no indication why or when this could happen...
                        log.Error("Stop signal failed!");
                        return false;
                    }
                }

                service.OnStart(null);

                stopSignal.Reset();
                stopSignal.WaitOne();
                log.Info("Stop signal received; shutting down service.");

                service.OnStop();
                return true;
            }
        }

        /// <summary>
        /// Hosts all ShipWorks services as new instance-specific background processes.
        /// If a background process is already running, it is signaled to stop and will be superseded by the new process.
        /// This is a non-blocking call as all running services are started in new processes.
        /// </summary>
        public static void RunAllInBackground()
        {
            foreach (ShipWorksServiceType serviceType in EnumHelper.GetEnumList<ShipWorksServiceType>().Select(e => e.Value))
            {
                Process.Start(Program.AppFileName, "/s=" + serviceType).Dispose();
            }
        }

        /// <summary>
        /// Signals an instance-specific ShipWorks background service process that it should shut down.
        /// </summary>
        /// <returns>true if the service was not running, or was running and signaled; false if the signal fails.</returns>
        public static bool StopInBackground(ShipWorksServiceType serviceType)
        {
            return StopInBackground(serviceType, ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Signals an instance-specific ShipWorks background service process that it should shut down.
        /// </summary>
        /// <returns>true if the service was not running, or was running and signaled; false if the signal fails.</returns>
        public static bool StopInBackground(ShipWorksServiceType serviceType, Guid instanceID)
        {
            var serviceName = GetServiceName(serviceType, instanceID);

            bool createdNew;
            using (var stopSignal = new EventWaitHandle(false, EventResetMode.ManualReset, serviceName, out createdNew))
            {
                if (!createdNew && !stopSignal.Set())
                {
                    // MSDN gives no indication why or when this could happen...
                    log.Error("Stop signal failed!");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Signals all running instance-specific ShipWorks background processes to shut down.
        /// </summary>
        /// <returns>true if the all services were not running, or were running and signaled; false if any signal fails.</returns>
        public static bool StopAllInBackground()
        {
            return StopAllInBackground(ShipWorksSession.InstanceID);
        }

        /// <summary>
        /// Signals all running instance-specific ShipWorks background processes to shut down.
        /// </summary>
        /// <returns>true if the all services were not running, or were running and signaled; false if any signal fails.</returns>
        public static bool StopAllInBackground(Guid instanceID)
        {
            return EnumHelper.GetEnumList<ShipWorksServiceType>().Select(e => e.Value)
                .Select(serviceType => StopInBackground(serviceType, instanceID))
                .Aggregate((all, stopped) => all && stopped);
        }


        private ServiceStatusEntity serviceStatusEntity;

        public ShipWorksServiceBase()
        {
            InitializeComponent();
            checkInTimer.Interval = ServiceStatusManager.CheckInTimeSpan.TotalMilliseconds;
        }

        [Description("The ShipWorks service type that this service implements.")]
        public ShipWorksServiceType ServiceType { get; set; }

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
        /// Sets instance-specific properties for this service; must be called after InitializeComponent in a derived service class.
        /// </summary>
        protected void InitializeInstance()
        {
            ServiceName = GetServiceName(ServiceType);
        }

        /// <summary>
        /// Begins trying to connect to the database.
        /// </summary>
        protected sealed override void OnStart(string[] args)
        {
            TryStart();
        }

        /// <summary>
        /// Tries to connect to the database, and if successful, starts up the service core.
        /// </summary>
        void TryStart()
        {
            if (!TryConnect())
            {
                tryStartTimer.Start();
                return;
            }

            OnStartCore();
            checkInTimer.Start();
        }

        /// <summary>
        /// Tries to connect to the database.
        /// </summary>
        static bool TryConnect()
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

            // Update the ServiceStatusEntity start and check in times.
            CurrentServiceStatusEntity.LastStartDateTime = DateTime.UtcNow;
            CurrentServiceStatusEntity.LastCheckInDateTime = CurrentServiceStatusEntity.LastStartDateTime;
            CurrentServiceStatusEntity.ServiceFullName = ServiceName;

            var manager = new WindowsServiceController(this);       //TODO: refactor to remove host-specific knowledge
            CurrentServiceStatusEntity.ServiceDisplayName =
                manager.IsServiceInstalled() ? manager.GetServiceDisplayName() : GetDisplayName(ServiceType);

            ServiceStatusManager.SaveServiceStatus(CurrentServiceStatusEntity);
        }

        static void InitializeForApplication()
        {
            LogSession.Initialize();

            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            UserSession.InitializeForCurrentDatabase();

            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentSession();
        }

        void OnCheckInTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ServiceStatusManager.CheckIn(CurrentServiceStatusEntity);
        }

        /// <summary>
        /// Stops the service core, if it was started.
        /// </summary>
        protected sealed override void OnStop()
        {
            tryStartTimer.Stop();

            if (checkInTimer.Enabled)
            {
                checkInTimer.Stop();
                OnStopCore();
            }
        }

        /// <summary>
        /// Performs core shutdown.  Only called if OnStartCore was called.
        /// </summary>
        protected virtual void OnStopCore()
        {
            // Update the ServiceStatusEntity stop and check in times.
            CurrentServiceStatusEntity.LastStopDateTime = DateTime.UtcNow;
            CurrentServiceStatusEntity.LastCheckInDateTime = CurrentServiceStatusEntity.LastStopDateTime;
            ServiceStatusManager.SaveServiceStatus(CurrentServiceStatusEntity);
        }
    }
}
