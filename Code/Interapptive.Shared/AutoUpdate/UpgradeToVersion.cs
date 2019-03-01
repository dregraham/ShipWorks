using System;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// DTO for sending information about the version to upgrade to
    /// </summary>
    public class UpgradeToVersion
    {
        /// <summary>
        /// Version to upgrade to
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Whether or not to update local DB
        /// </summary>
        public bool UpgradeLocalDB { get; set; }
    }
}