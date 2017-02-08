using System.Collections.Generic;

namespace ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    /// <summary>
    /// Class that creates SqlInstallerInfo objects
    /// </summary>
    public interface ISqlInstallerRepository
    {
        /// <summary>
        /// Class for fetching SqlInstallerInfo objects
        /// </summary>
        IEnumerable<ISqlInstallerInfo> SqlInstallersForThisMachine();
    }
}