using System;
using System.IO;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Class to write files used by the upgrade process
    /// </summary>
    [Component]
    public class FileWriter : IFileWriter
    {
        private readonly ILog log;
        private readonly IServiceName serviceName;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileWriter(IServiceName serviceName, Func<Type, ILog> createLogger)
        {
            this.serviceName = serviceName;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Write details about the upgrade to a file that ShipWorks can pick up after the upgrade is over
        /// </summary>
        public void WriteUpgradeDetailsToFile(Version upgradingToVersion)
        {
            try
            {
                var details = new UpgradeDetails { UpgradingToVersion = upgradingToVersion };
                var serializedDetails = SerializationUtility.SerializeToXml(details);
                File.WriteAllText(Path.Combine(EscalatorDataPath.InstanceRoot, serviceName.GetInstanceID().ToString("B"), "upgradeDetails.xml"), serializedDetails);
            }
            catch (IOException ex)
            {
                // If we can't write the file, we shouldn't hold up the upgrade
                log.Warn("Could not write upgrade details to file.", ex);
            }
        }
    }
}
