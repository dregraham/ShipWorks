using log4net;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.ApplicationCore.Services.Hosting;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using ShipWorks.ApplicationCore.Crashes;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running ShipWorks as a service.
    /// </summary>
    public class ServiceExecutionMode : IExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceExecutionMode));

        private readonly Lazy<IServiceHost> host;
        private readonly IList<string> options;
        private readonly IExecutionModeInitializer initializer;

        private DateTime startupTimeInUtc;
        private int recoveryAttempts;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExecutionMode" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="options">The options.</param>
        /// <param name="recoveryAttempts">The number of attempts that have been made to recover from a crash.</param>
        public ServiceExecutionMode(string serviceName, IList<string> options, int recoveryAttempts)
            : this(serviceName, options, recoveryAttempts, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExecutionMode" /> class.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="options">The options.</param>
        /// <param name="recoveryAttempts">The number of attempts that have been made to recover from a crash.</param>
        /// <param name="hostFactory">The host factory.</param>
        /// <param name="initializer">The initializer.</param>
        /// <exception cref="System.ArgumentNullException">serviceName</exception>
        public ServiceExecutionMode(string serviceName, IList<string> options, int recoveryAttempts, IServiceHostFactory hostFactory, IExecutionModeInitializer initializer)
        {
            if (null == serviceName)
            {
                throw new ArgumentNullException("serviceName");
            }

            if (null == hostFactory)
            {
                hostFactory = new DefaultServiceHostFactory();
            }

            host = new Lazy<IServiceHost>(() => hostFactory.GetHostFor(GetServiceForName(serviceName)));

            this.options = options ?? new string[0];
            this.initializer = initializer ?? new ServiceExecutionModeInitializer();
            
            this.recoveryAttempts = recoveryAttempts;
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
        IServiceHost Host
        {
            get { return host.Value; }
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        ///   </returns>
        public bool IsUserInteractive
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
        public void Execute()
        {
            log.Info("Running as a service.");
            
            if (options.Contains("/stop"))
            {
                Host.Stop();
            }
            else
            {
                startupTimeInUtc = DateTime.UtcNow;

                initializer.Initialize();
                Host.Run();
            }
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public void HandleException(Exception exception)
        {            
            string userEmail = string.Empty;

            if (UserSession.IsLoggedOn)
            {
                userEmail = UserSession.User.Email;
                UserSession.Logoff(false);
            }
            UserSession.Reset();

            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);
            }

            ServiceCrash serviceCrash = new ServiceCrash(this, exception);
            serviceCrash.SubmitReport(userEmail);

            Host.HandleServiceCrash(serviceCrash);
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
        /// <exception cref="ExecutionModeConfigurationException"></exception>
        static ShipWorksServiceBase GetServiceForName(string serviceName)
        {
            try
            {
                return ShipWorksServiceBase.GetService(serviceName);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Could not find a service named '{0}'.", serviceName);
                throw new ExecutionModeConfigurationException(errorMessage, ex);
            }
        }
    }
}
