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
        /// Root path to all settings that are shared across all ShipWorks users and instances
        /// </summary>
        public static string SharedSettings
        {
            get
            {
                return Path.Combine(GetCommonSettingsPathDefault(), "Shared");
            }
        }

        /// <summary>
        /// Gets the path to the default settings
        /// </summary>
        private static string GetCommonSettingsPathDefault() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Interapptive\ShipWorks");
    }
}
