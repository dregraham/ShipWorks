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
    }
}
