using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using ShipWorks.Startup;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Provide assembly-based services
    /// </summary>
    public static class AssemblyProvider
    {
        /// <summary>
        /// Get ShipWorks assemblies from disk
        /// </summary>
        public static IEnumerable<Assembly> GetAssemblies()
        {
            return ContainerInitializer.AllAssemblies
                .OrderBy(a => a.FullName);
        }

        /// <summary>
        /// Get concrete types defined in ShipWorks assemblies that are loaded in the current AppDomain
        /// </summary>
        public static IEnumerable<Type> GetShipWorksTypesInAppDomain()
        {
            return ContainerInitializer.AllAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract);
        }
    }
}
