using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    /// <summary>
    /// An abstract class implementing the IExectuionModeInitializer interface. This is intended
    /// for encapsulating operations that may be common across execution mode initializers.
    /// </summary>
    public abstract class ExecutionModeInitializerBase : IExecutionModeInitializer
    {
        /// <summary>
        /// Intended for settng up/initializing any dependencies for an execution context.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Performs the initialization that is common to all execution modes. It's in a separate method
        /// so derived classes can choose where to perform the initialization as it makes sense to the 
        /// implementation.
        /// </summary>
        protected void PerformCommonInitialization()
        {
            MyComputer.LogEnvironmentProperties();

            // Looking for all types in this assembly that have the LLBLGen DependcyInjection attribute
            DependencyInjectionDiscoveryInformation.ConfigInformation = new DependencyInjectionConfigInformation();
            DependencyInjectionDiscoveryInformation.ConfigInformation.AddAssembly(Assembly.GetExecutingAssembly());

            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            // Override the default of 30 seconds.  We are seeing a lot of timeout crashes in the alpha that I think are due
            // to people's machines just not being able to handle the load, and 30 seconds just wasn't enough.
            SqlCommandProvider.DefaultTimeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 300 : 120);

            // Do initial edition initialization
            EditionManager.Initialize();
        }
    }
}
