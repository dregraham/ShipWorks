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
    }
}
