using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Tests.Shared
{
    public static class AssemblyProvider
    {
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
    }
}
