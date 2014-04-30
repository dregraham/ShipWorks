using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;

namespace ShipWorks.Data.Administration.Versioning
{
    /// <summary>
    /// When all the versions are added to VersionGraph, this will return the best shortest path.
    /// </summary>
    public class VersionGraph
    {
        private AdjacencyGraph<string, Edge<string>> graph;
        private Dictionary<Edge<string>, double> edgeCosts;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionGraph"/> class.
        /// </summary>
        /// <param name="installedVersion">The installed version of the Database.</param>
        public VersionGraph()
        {
            graph = new AdjacencyGraph<string, Edge<string>>();
            edgeCosts = new Dictionary<Edge<string>, double>(graph.EdgeCount);
        }

        /// <summary>
        /// Gets the adjacency graph.
        /// </summary>
        /// <value>
        /// The adjacency graph.
        /// </value>
        [CLSCompliant(false)]
        public AdjacencyGraph<string, Edge<string>> VersionAdjacencyGraph
        {
            get
            {
                return graph;
            }
        }

        /// <summary>
        /// Gets the upgrade path.
        /// </summary>
        /// <param name="fromVersion">The installed version.</param>
        /// <param name="toVersion">The target version.</param>
        public List<VersionUpgradeStep> GetUpgradePath(SchemaVersion fromVersion, SchemaVersion toVersion, IEnumerable<UpgradePath> shipWorksVersions)
        {
            if (toVersion == fromVersion)
            {
                return new List<VersionUpgradeStep>();
            }

            foreach (var shipWorksVersion in shipWorksVersions)
            {
                AddVersion(shipWorksVersion.To, shipWorksVersion.From);
            }

            TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths;

            try
            {

                tryGetPaths = graph.ShortestPathsDijkstra(edge => edgeCosts[edge], toVersion.VersionName);
            }
            catch (KeyNotFoundException ex)
            {
                throw new FindVersionUpgradePathException("Couldn't find version in version file.", ex);
            }

            IEnumerable<Edge<string>> path;

            if (tryGetPaths(fromVersion.VersionName, out path))
            {
                return path.Reverse().Select(version => new VersionUpgradeStep()
                {
                    Version = version.Source, 
                    Script = shipWorksVersions.First(x => x.To == version.Source).GetScriptName(version.Target),
                    Process = shipWorksVersions.First(x => x.To == version.Source).GetUpdateProcessName(version.Target)
                })
                .ToList();
            }

            throw new FindVersionUpgradePathException(string.Format("Couldn't find path from {0} to {1}.", fromVersion, toVersion));
        }

        /// <summary>
        /// Adds the version. The target version is listed first. 
        /// In versionsToUpgrade, the first version passed in is the preferred version.
        /// </summary>
        /// <param name="targetVersion">Version to upgrade to.</param>
        /// <param name="versionsToUpgrade">The first version passed in is the preferred version.</param>
        private void AddVersion(string targetVersion, IEnumerable<VersionUpgradeStep> versionsToUpgrade)
        {
            graph.AddVertex(targetVersion);

            int edgeCost = 0;

            foreach (VersionUpgradeStep versionToUpgrade in versionsToUpgrade)
            {
                Edge<string> edge = new Edge<string>(targetVersion, versionToUpgrade.Version);

                graph.AddEdge(edge);
                edgeCosts.Add(edge, edgeCost);

                edgeCost = 10000;
            }
        }
    }
}