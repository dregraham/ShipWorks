﻿using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Common.Logging;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Abstract base for ExecutionMode's
    /// </summary>
    public abstract class ExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExecutionMode));
        private SynchronizationContext synchronizationContext;

        public ExecutionMode()
        {
            ServicePointManager.DefaultConnectionLimit = 20;
            ServicePointManager.Expect100Continue = false;
        }

        public virtual SynchronizationContext BaseSynchronizationContext => synchronizationContext;

        /// <summary>
        /// Name of this execution mode (User interface, command line, service)
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Indicates if this execution mode supports displaying a UI, whether or not one is currently displayed or not
        /// </summary>
        public abstract bool IsUISupported
        {
            get;
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsUIDisplayed
        {
            get;
        }

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run,
        /// ServiceBase.Run, etc.)
        /// </summary>
        public abstract Task Execute();

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public abstract Task HandleException(Exception exception, bool guiThread, string userEmail);

        /// <summary>
        /// Provides common initialization and an extension point for additional initialization
        /// </summary>
        protected virtual void Initialize()
        {
            synchronizationContext = SynchronizationContext.Current;

            MyComputer.LogEnvironmentProperties();

            // Looking for all types in this assembly that have the LLBLGen DependencyInjection attribute
            DependencyInjectionDiscoveryInformation.ConfigInformation = new DependencyInjectionConfigInformation();
            DependencyInjectionDiscoveryInformation.ConfigInformation.AddAssembly(Assembly.GetExecutingAssembly());

            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;
            SetAllowedSecurityProtocols();

            // Override the default of 30 seconds.  We are seeing a lot of timeout crashes in the alpha that I think are due
            // to people's machines just not being able to handle the load, and 30 seconds just wasn't enough.
            DbCommandProvider.DefaultTimeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 300 : 120);

            // Do initial edition initialization
            EditionManager.Initialize();

            Telemetry.GetCustomerID = BuildCustromerIDRetrievalFunction();
            Telemetry.SetExecutionMode(Name?.Replace("ExecutionMode", ""));
            Telemetry.SetInstance(ShipWorksSession.InstanceID.ToString("D"));
        }

        /// <summary>
        /// Show a termination message, either through a window or a console message depending on the ExecutionMode
        /// </summary>
        public virtual void ShowTerminationMessage(Form window, string message)
        {
            log.Error(message);

            if (IsUISupported)
            {
                if (window != null)
                {
                    window.ShowDialog();
                }
                else
                {
                    MessageHelper.ShowInformation(null, message);
                }
            }
        }

        /// <summary>
        /// Attempt to add TLS 1.2 support for any customers running Windows 7 or higher with .NET 4.5 or higher
        /// </summary>
        private static void SetAllowedSecurityProtocols()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                log.Info("Successfully set 1.2 protocols.");
            }
            catch (NotSupportedException ex)
            {
                log.Error("Could not add 1.2 protocols: ", ex);
                throw;
            }

            log.Debug("Supported security protocols: " + ServicePointManager.SecurityProtocol);
        }

        /// <summary>
        /// Build a function that will retrieve the customer ID from Tango, caching the results
        /// </summary>
        private Func<string> BuildCustromerIDRetrievalFunction()
        {
            string customerID = null;

            return () => customerID ?? (customerID = GetCustomerIdForTelemetry()) ?? "Not Retrieved";
        }

        /// <summary>
        /// Get a customer id that can be used for telemetry
        /// </summary>
        private string GetCustomerIdForTelemetry()
        {
            try
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    var tangoWebClient = lifetimeScope.Resolve<ITangoWebClient>();
                    string customerID = tangoWebClient.GetTangoCustomerId();
                    return string.IsNullOrEmpty(customerID) ? null : customerID;
                }
            }
            catch (Exception ex)
            {
                // Catch everything because we don't want telemetry to crash ShipWorks
                log.Warn("Could not get customer id for telemetry", ex);
                return null;
            }
        }
    }
}
