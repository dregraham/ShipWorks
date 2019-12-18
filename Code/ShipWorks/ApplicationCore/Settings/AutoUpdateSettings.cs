using System;
using System.IO;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Settings related to Auto Update
    /// </summary>
    public static class AutoUpdateSettings
    {
        private static readonly string disableAutoUpdateFilePath = Path.Combine(DataPath.SharedSettings, "DisableAutoUpdate.txt");
        private static readonly string failedAutoUpdateFilePath = Path.Combine(DataPath.InstanceRoot, "FailedAutoUpdate.txt");

        /// <summary>
        /// Whether or not auto update is enabled for this machine
        /// </summary>
        public static bool IsAutoUpdateDisabled
        {
            get
            {
                try
                {
                    return File.Exists(disableAutoUpdateFilePath);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                if (IsAutoUpdateDisabled != value)
                {
                    // If we are disabling auto updates and the disable file does not exist, create it.
                    if (value && !File.Exists(disableAutoUpdateFilePath))
                    {
                        File.Create(disableAutoUpdateFilePath);
                    }
                    else
                    {
                        // We are enabling auto updates, so delete the disable file if it exists.
                        if (File.Exists(disableAutoUpdateFilePath))
                        {
                            File.Delete(disableAutoUpdateFilePath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Whether or not most recent attempt to auto update failed
        /// </summary>
        public static bool FailedLastAutoUpdate
        {
            get
            {
                try
                {
                    return File.Exists(failedAutoUpdateFilePath);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
