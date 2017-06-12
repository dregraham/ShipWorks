using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;

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
            var comparison = StringComparison.InvariantCultureIgnoreCase;

            return AppDomain.CurrentDomain.GetAssemblies()
                // has Interapptive or ShipWorks
                .Where(n => n.FullName.IndexOf("Interapptive", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            n.FullName.IndexOf("Shipworks", comparison) >= 0)
                // doesn't have tests
                .Where(n => n.FullName.IndexOf("Tests", 0, StringComparison.InvariantCultureIgnoreCase) == -1)
                .OrderBy(a => a.FullName);
        }

        /// <summary>
        /// Get concrete types defined in ShipWorks assemblies that are loaded in the current AppDomain
        /// </summary>
        public static IEnumerable<Type> GetShipWorksTypesInAppDomain()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(IsShipWorksAssembly)
                .Where(IsNotTestAssembly)
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract);
        }

        /// <summary>
        /// Is the assembly a test assembly
        /// </summary>
        private static bool IsNotTestAssembly(Assembly assembly) => !assembly.FullName.Contains("Tests");

        /// <summary>
        /// Is the assembly a ShipWorks built assembly
        /// </summary>
        private static bool IsShipWorksAssembly(Assembly assembly)
        {
            return assembly.FullName.StartsWith("ShipWorks", StringComparison.OrdinalIgnoreCase) ||
                assembly.FullName.StartsWith("Interapptive", StringComparison.OrdinalIgnoreCase);
        }
    }
}
