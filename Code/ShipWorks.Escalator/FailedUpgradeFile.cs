using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;
using log4net;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Keeps track if the upgrade failed or now using a file called "FailedAutoUpdate.txt"
    /// </summary>
    [Component(SingleInstance = true)]
    public class FailedUpgradeFile : IFailedUpgradeFile
    {
        private readonly string failedAutoUpdateFilePath;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public FailedUpgradeFile(IServiceName serviceName, Func<Type, ILog> logFactory)
        {
            failedAutoUpdateFilePath =
                Path.Combine(EscalatorDataPath.InstanceRoot, serviceName.GetInstanceID().ToString("B"), "FailedAutoUpdate.txt");

            log = logFactory(typeof(ShipWorksUpgrade));
        }
        
        /// <summary>
        /// Creates "FailedAutoUpdate.txt"
        /// </summary>
        public void CreateFailedAutoUpdateFile()
        {
            try
            {
                log.Info($"Creating file {failedAutoUpdateFilePath}'");
                // If the auto update failed and the file does not exist, create it.
                if (!File.Exists(failedAutoUpdateFilePath))
                {
                    log.Info("File does not exist");
                    using (var file = File.Create(failedAutoUpdateFilePath))
                    {
                    }

                    var accessControl = File.GetAccessControl(failedAutoUpdateFilePath);
                    accessControl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.Modify, AccessControlType.Allow));
                    accessControl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.Write, AccessControlType.Allow));
                    File.SetAccessControl(failedAutoUpdateFilePath, accessControl);

                    log.Info("File created sucessfully.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in CreateFailedAutoUpdateFile.", ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes "FailedAutoUpdate.txt"
        /// </summary>
        public void DeleteFailedAutoUpdateFile()
        {
            try
            {
                log.Info($"Deleting file {failedAutoUpdateFilePath}'");

                // Removing the failure flag, so delete the file if it exists.
                if (File.Exists(failedAutoUpdateFilePath))
                {
                    log.Info("File exists");
                    File.Delete(failedAutoUpdateFilePath);
                    log.Info("File deleted sucessfully");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in DeleteFailedAutoUpdateFile.", ex);
                throw;
            }
        }
    }
}
