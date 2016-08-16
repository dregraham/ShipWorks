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
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var comparison = StringComparison.InvariantCultureIgnoreCase;

            return Directory.GetFiles(path, "*.dll")
                // has Interapptive or ShipWorks
                .Where(n => Path.GetFileName(n).IndexOf("Interapptive", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                            Path.GetFileName(n).IndexOf("Shipworks", comparison) >= 0)
                // doesn't have tests
                .Where(n => Path.GetFileName(n).IndexOf("Tests", 0, StringComparison.InvariantCultureIgnoreCase) == -1)
                .Select(dll => Assembly.LoadFile(dll))
                .OrderBy(a => a.FullName);
        }
    }
}
