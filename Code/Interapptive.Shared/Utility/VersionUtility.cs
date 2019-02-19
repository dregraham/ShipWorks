using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class to help with Version objects
    /// </summary>
    public static class VersionUtility
    {
        private readonly static Lazy<Version> assemblyVersion = new Lazy<Version>(() =>
        {
            // Tango requires a specific version in order to know when to return
            // legacy responses or new response for the customer license. This is
            // primarily for debug/internal versions of ShipWorks that have 0.0.0.x
            // version number.
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version minimumVersion = new Version(5, 0, 0, 0);

            return Complete(assemblyVersion.Major == 0 ? minimumVersion : assemblyVersion);
        });

        /// <summary>
        /// Return a new version object where all four components of the version are filled in.  Any missing ones have zero's used.
        /// </summary>
        public static Version Complete(this Version version)
        {
            return new Version(
                version.Major,
                version.Minor,
                version.Build == -1 ? 0 : version.Build,
                version.Revision == -1 ? 0 : version.Revision);
        }

        /// <summary>
        /// Gets the version of the current assembly
        /// </summary>
        public static Version AssemblyVersion => assemblyVersion.Value;
    }
}
