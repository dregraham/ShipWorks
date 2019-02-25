using System.Reflection;
using Autofac;
using Interapptive.Shared.Net;

namespace ShipWorks.Escalator.ApplicationCore
{
    class ContainerInitializer
    {
        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        /// <remarks>
        /// This method sets the current IoC instance, which means it is NOT thread-safe
        /// </remarks>
        public static IContainer Initialize() =>
            IoC.Initialize(x => { }, AllAssemblies);

        /// <summary>
        /// All ShipWorks assemblies to be used for dependency injection
        /// </summary>
        public static Assembly[] AllAssemblies => new[]
        {
            // Interapptive.Shared
            typeof(HttpVariableRequestSubmitter).Assembly,
            typeof(Escalator).Assembly
        };
    }
}
