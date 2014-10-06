using System.Text;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.ApplicationCore.Services.Hosting;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using ShipWorks.ApplicationCore.Crashes;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.Data;
using ShipWorks.ApplicationCore.Logging;
using NDesk.Options;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running ShipWorks as a service.
    /// </summary>
    public class ServiceExecutionMode : ExecutionMode
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceExecutionMode));

        readonly Lazy<IServiceHost> host;

        DateTime startupTimeInUtc;
        int recoveryAttempts;

        bool stopRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExecutionMode" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="options">The options.</param>
        public ServiceExecutionMode(string serviceName, IList<string> options)
        {
            if (null == serviceName)
            {
                throw new ArgumentNullException("serviceName");
            }

            host = new Lazy<IServiceHost>(() => ServiceHostFactory.GetHostFor(GetServiceForName(serviceName)));

            options = options ?? new string[0];

            // Need to extract the arguments
            OptionSet optionSet = new OptionSet()
                {
                    { "recovery=", v => Int32.TryParse(v, out recoveryAttempts) },
                    { "stop", v => stopRequested = true },
                    { "<>", v => { throw new CommandLineCommandArgumentException("service", v, "Invalid arguments passed to service."); } }
                };
            optionSet.Parse(options);

            startupTimeInUtc = DateTime.MinValue;
        }

        /// <summary>
        /// Gets the number of attempts to recover from a service crash have been made.
        /// </summary>
        public int RecoveryAttempts
        {
            get
            {
                if (UpTime.TotalMinutes >= 5)
                {
                    // Reset the recovery attempts if the service has been up and running for at least five minutes the service 
                    // has been running for at least five minutes in the event that it finally did recover for a while before 
                    // crashing again (i.e. any subsequent crashes will be treated as a new sequence of crashes)
                    recoveryAttempts = 0;
                }

                return recoveryAttempts;
            }
        }

        /// <summary>
        /// Gets the host that ShipWorks will running within.
        /// </summary>
        /// <value>The host.</value>
        private IServiceHost Host
        {
            get { return host.Value; }
        }

        /// <summary>
        /// Name of this execution mode (User interface, command line, service)
        /// </summary>
        public override string Name
        {
            get
            {
                return "ServiceExecutionMode"; 
            }
        }

        /// <summary>
        /// Indicates if this execution mode supports displaying a UI, whether or not one is currently displayed or not
        /// </summary>
        public override bool IsUISupported
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        ///   </returns>
        public override bool IsUIDisplayed
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the up time of the service.
        /// </summary>
        public TimeSpan UpTime
        {
            get
            {
                if (startupTimeInUtc != DateTime.MinValue)
                {
                    return DateTime.UtcNow.Subtract(startupTimeInUtc);
                }
                else
                {
                    return new TimeSpan(0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run,
        /// ServiceBase.Run, etc.)
        /// </summary>
        public override void Execute()
        {
            log.Info("Running as a service.");

            if (stopRequested)
            {
                // Only initialize the base stuff - not all the stuff we add on that kicks off processes
                base.Initialize();

                Host.Stop();
            }
            else
            {
                // Do full initialzition
                Initialize();

                startupTimeInUtc = DateTime.UtcNow;

                Host.Run();
            }
        }

        /// <summary>
        /// Do initialization before actual running of the service
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Register some idle cleanup work.
            DataResourceManager.RegisterResourceCacheCleanup();
            DataPath.RegisterTempFolderCleanup();
            LogSession.RegisterLogCleanup();

            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedFilterCounts", FilterContentManager.DeleteAbandonedFilterCounts, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedQuickFilters", QuickFilterHelper.DeleteAbandonedFilters, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedResources", DataResourceManager.DeleteAbandonedResourceData, "cleaning up resources", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("ValidatePendingAddresses", AddressValidationQueue.PerformValidation, "validating addresses", TimeSpan.FromMinutes(2));

            // Start idle processing
            IdleWatcher.Initialize();

            // Listen for entity changes so we know when to validate orders
            DataProvider.OrderEntityChangeDetected += AddressValidationQueue.OnOrderEntityChangeDetected;
            DataProvider.ShipmentEntityChangeDetected += AddressValidationQueue.OnShipmentEntityChangeDetected;
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public override void HandleException(Exception exception, bool guiThread, string userEmail)
        {            
            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);

                if (SqlSession.IsConfigured)
                {
                    log.Fatal(SqlUtility.GetRunningSqlCommands(SqlSession.Current.Configuration.GetConnectionString()));
                }

                if (IsEligibleToSubmitCrashReport())
                {
                    ServiceCrash serviceCrash = new ServiceCrash(exception);
                    serviceCrash.SubmitReport(userEmail);
                }
            }

            Host.HandleServiceCrash(RecoveryAttempts);
        }

        /// <summary>
        /// Determines whether this instance [is eligible to submit crash a report].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [is eligible to submit a crash report]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEligibleToSubmitCrashReport()
        {
            const int MaximumCrashesForSubmittingReports = 5;

            // Only submit the crash report if we haven't exceeded the max (so we don't flood FogBugz with crash
            // reports when a service tries to continuously start itself)
            return RecoveryAttempts < MaximumCrashesForSubmittingReports;
        }
        
        /// <summary>
        /// Gets the name of the service for.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>A ShipWorksServiceBase object.</returns>
        static ShipWorksServiceBase GetServiceForName(string serviceName)
        {
            try
            {
                return ShipWorksServiceManager.GetService(serviceName);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Could not find a service named '{0}'.", serviceName);
                throw new ShipWorksServiceException(errorMessage, ex);
            }
        }
    }
}
