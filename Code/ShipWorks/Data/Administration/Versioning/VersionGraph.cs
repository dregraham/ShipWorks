using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
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
        public VersionGraph()
        {
            graph = new AdjacencyGraph<string, Edge<string>>();
            edgeCosts = new Dictionary<Edge<string>, double>(graph.EdgeCount);
        }

        /// <summary>
        /// Gets the upgrade path.
        /// </summary>
        /// <param name="fromVersion">The installed version.</param>
        /// <param name="toVersion">The target version.</param>
        /// <param name="schemaVersions">All the ShipWorks schema versions.</param>
        /// <exception cref="FindVersionUpgradePathException">
        /// </exception>
        public List<VersionUpgradeStep> GetUpgradePath(SchemaVersion fromVersion, SchemaVersion toVersion, IEnumerable<UpgradePath> schemaVersions)
        {
            if (toVersion == fromVersion)
            {
                return new List<VersionUpgradeStep>();
            }

            foreach (UpgradePath schemaVersion in schemaVersions)
            {
                AddVersion(schemaVersion);
            }

            TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths;

            try
            {
                tryGetPaths = graph.ShortestPathsDijkstra(edge => edgeCosts[edge], toVersion.VersionName);
            }
            catch (KeyNotFoundException ex)
            {
                throw new FindVersionUpgradePathException(string.Format("Couldn't find version '{0}' in version file.", toVersion.VersionName), ex);
            }

            IEnumerable<Edge<string>> path;

            if (tryGetPaths(fromVersion.VersionName, out path))
            {
                return path.Reverse().Select(version => new VersionUpgradeStep(version, schemaVersions.First(upgradePath => upgradePath.To == version.Source)))
                .ToList();
            }

            throw new FindVersionUpgradePathException(string.Format("Couldn't find path from {0} to {1}.", fromVersion, toVersion));
        }

        /// <summary>
        /// Adds the version. The target version is listed first. 
        /// In fromVersions, the first version passed in is the preferred version.
        /// </summary>
        private void AddVersion(UpgradePath upgradePath)
        {
            graph.AddVertex(upgradePath.To);

            int edgeCost = upgradePath.PreferredVersion ? 1 : 10000;

            foreach (VersionUpgradeStep versionToUpgrade in upgradePath.From)
            {
                Edge<string> edge = new Edge<string>(upgradePath.To, versionToUpgrade.FromVersion);

                graph.AddEdge(edge);
                edgeCosts.Add(edge, edgeCost);
            }
        }
    }
}