using System;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Class to write files used by the upgrade process
    /// </summary>
    public interface IFileWriter
    {
        /// <summary>
        /// Write details about the upgrade to a file that ShipWorks can pick up after the upgrade is over
        /// </summary>
        void WriteUpgradeDetailsToFile(Version upgradingToVersion);
    }
}