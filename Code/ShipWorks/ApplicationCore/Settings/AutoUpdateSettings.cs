using System;
using System.IO;
using log4net;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Settings related to Auto Update
    /// </summary>
    public static class AutoUpdateSettings
    {
        private static readonly string disableAutoUpdateFilePath = Path.Combine(DataPath.SharedSettings, "DisableAutoUpdate.txt");
        private static readonly string failedAutoUpdateFilePath = Path.Combine(DataPath.InstanceRoot, "FailedAutoUpdate.txt");
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoUpdateSettings));

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
                catch (Exception ex)
                {
                    log.Error($"Failed to read file '{disableAutoUpdateFilePath}' in IsAutoUpdateDisabled", ex);
                    return false;
                }
            }
            set
            {
                try
                {

                    if (IsAutoUpdateDisabled != value)
                    {
                        log.Info($"Checking if '{disableAutoUpdateFilePath}' exists");
                        // If we are disabling auto updates and the disable file does not exist, create it.
                        if (value && !File.Exists(disableAutoUpdateFilePath))
                        {
                            log.Info($"Creating file {disableAutoUpdateFilePath}");
                            using (File.Create(disableAutoUpdateFilePath))
                            {
                                // make sure the file closes.
                            };
                            log.Info("File created without error");
                        }
                        else
                        {
                            log.Info($"Locating file {disableAutoUpdateFilePath}");
                            // We are enabling auto updates, so delete the disable file if it exists.
                            if (File.Exists(disableAutoUpdateFilePath))
                            {
                                log.Info($"File '{disableAutoUpdateFilePath}' exists. Deleting.");
                                File.Delete(disableAutoUpdateFilePath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error updating value of IsAutoUpdateDisabled. Rethrowing.", ex);
                    throw;
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
                catch (Exception ex)
                {
                    log.Error($"Failed to read file '{failedAutoUpdateFilePath}' in LastAutoUpdateSucceeded", ex);
                    return false;
                }
            }
            set
            {
                try
                {
                    if (LastAutoUpdateSucceeded != value)
                    {
                        log.Info($"Checking if '{failedAutoUpdateFilePath}' exists");
                        if (value && File.Exists(failedAutoUpdateFilePath))
                        {
                            log.Info($"File '{failedAutoUpdateFilePath}' exists. Deleting.");
                            // Auto update succeeded, so delete the failure file if it exists.
                            File.Delete(failedAutoUpdateFilePath);
                        }
                        else if (!value && !File.Exists(failedAutoUpdateFilePath))
                        {
                            log.Info($"Creating file {failedAutoUpdateFilePath}");
                            // If auto update failed and the file does not exist, create it.
                            using (File.Create(failedAutoUpdateFilePath))
                            {
                                // make sure the file closes.
                            }
                            log.Info("File created without error");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error updating value of LastAutoUpdateSucceeded.", ex);

                    // don't want to show an error or hold anything up because of this.
                }
            }
        }
    }
}
