using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// Class describing how to upgrade from one version to another.
    /// </summary>
    public class VersionUpgradeStep
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionUpgradeStep"/> class.
        /// </summary>
        public VersionUpgradeStep()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionUpgradeStep" /> class.
        /// </summary>
        /// <param name="versionEdge">The version edge. Due to how versions are searched, the target is the from version and the source is the to version.</param>
        /// <param name="upgradePath">The upgrade path.</param>
        [CLSCompliant(false)]  // For some reason, Edge<string> is not CLS Complient.        
        public VersionUpgradeStep(QuickGraph.Edge<string> versionEdge, UpgradePath upgradePath)
        {
            ToVersion = versionEdge.Source;
            FromVersion = versionEdge.Target;
            Script = upgradePath.GetScriptName(versionEdge.Target);
            Process = upgradePath.GetUpdateProcessName(versionEdge.Target);
        }

        /// <summary>
        /// Gets from version.
        /// </summary>
        public string FromVersion { get; private set; }

        /// <summary>
        /// Gets to version.
        /// </summary>
        public string ToVersion { get; private set; }

        /// <summary>
        /// Gets the script.
        /// </summary>
        public string Script { get; private set; }

        /// <summary>
        /// Gets the process.
        /// </summary>
        public string Process { get; private set; }
    }
}
