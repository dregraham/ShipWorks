using System;
using System.IO;
using System.Reflection;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Data path for the escalator
    /// </summary>
    public static class EscalatorDataPath
    {
        /// <summary>
        /// Instance root
        /// </summary>
        public static string InstanceRoot =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Interapptive\ShipWorks\Instances");

        /// <summary>
        /// Gets the folder path containing the ShipWorks executable.
        /// </summary>
        public static string AppLocation => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    }
}
