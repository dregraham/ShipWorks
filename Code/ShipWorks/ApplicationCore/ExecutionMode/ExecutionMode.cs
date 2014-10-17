using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Abstract base for ExecutionMode's
    /// </summary>
    public abstract class ExecutionMode
    {
        private const int SecurityProtocolTypeTls12 = 3072;
        private const int SecurityProtocolTypeTls11 = 768;
        private static readonly ILog log = LogManager.GetLogger(typeof(ExecutionMode));

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
        public abstract void Execute();

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public abstract void HandleException(Exception exception, bool guiThread, string userEmail);

        /// <summary>
        /// Provides common initialization and an extension point for additional initialization
        /// </summary>
        protected virtual void Initialize()
        {
            MyComputer.LogEnvironmentProperties();

            // Looking for all types in this assembly that have the LLBLGen DependencyInjection attribute
            DependencyInjectionDiscoveryInformation.ConfigInformation = new DependencyInjectionConfigInformation();
            DependencyInjectionDiscoveryInformation.ConfigInformation.AddAssembly(Assembly.GetExecutingAssembly());

            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;
            SetAllowedSecurityProtocols();

            // Override the default of 30 seconds.  We are seeing a lot of timeout crashes in the alpha that I think are due
            // to people's machines just not being able to handle the load, and 30 seconds just wasn't enough.
            SqlCommandProvider.DefaultTimeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 300 : 120);

            // Do initial edition initialization
            EditionManager.Initialize();
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
        /// Attempt to add TLS 1.1 and 1.2 support for any customers running Windows 7 or higher with .NET 4.5 or higher
        /// </summary>
        private static void SetAllowedSecurityProtocols()
        {
            try
            {
                if (MyComputer.IsWindowsVista)
                {
                    // Vista will allow the protocols to be added to the available list, but does not actually support them.
                    // The only reason we call this out seperately is to avoid confusion where we say support is loaded, but actually isn't available
                    log.Info("Could not add TLS1.1 and 1.2 protocols: Windows Vista does not support these protocols");
                }
                else
                {
                    ServicePointManager.SecurityProtocol |=
                        (SecurityProtocolType) SecurityProtocolTypeTls12 |
                        (SecurityProtocolType) SecurityProtocolTypeTls11;
                    log.Info("Successfully added TLS1.1 and 1.2 protocols.");
                }
            }
            catch (NotSupportedException ex)
            { 
                log.Info("Could not add TLS1.1 and 1.2 protocols: ", ex);
            }

            log.Debug("Supported security protocols: " + ServicePointManager.SecurityProtocol);
        }
    }
}
