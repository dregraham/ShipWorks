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
                        using (File.Create(disableAutoUpdateFilePath)) 
                        { 
                            // make sure the file closes.
                        };
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
        public static bool LastAutoUpdateSucceeded
        {
            get
            {
                try
                {
                    return !File.Exists(failedAutoUpdateFilePath);
                }
                catch (Exception)
                {
                    return true;
                }
            }
            set
            {
                try
                {
                    if (LastAutoUpdateSucceeded != value)
                    {
                        if (value && File.Exists(failedAutoUpdateFilePath))
                        {
                            // Auto update succeeded, so delete the failure file if it exists.
                            File.Delete(failedAutoUpdateFilePath);
                        }
                        else if (!value && !File.Exists(failedAutoUpdateFilePath))
                        {
                            // If auto update failed and the file does not exist, create it.
                            using (File.Create(failedAutoUpdateFilePath))
                            {
                                // make sure the file closes.
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // don't want to show an error or hold anything up because of this.
                }
            }
        }
    }
}
