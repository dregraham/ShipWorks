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
    internal class VersionGraph
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
        /// Gets the upgrade path.
        /// </summary>
        public List<String> GetUpgradePath(SchemaVersion fromVersion, SchemaVersion toVersion, IEnumerable<UpgradePath> shipWorksVersions)
        {
            foreach (var shipWorksVersion in shipWorksVersions)
            {
                AddVersion(shipWorksVersion.ToVersion, shipWorksVersion.FromVersion);
            }

            return GetUpgradePath(fromVersion, toVersion);
        }

        /// <summary>
        /// Adds the version. The target version is listed first. 
        /// In versionsToUpgrade, the first version passed in is the preferred version.
        /// </summary>
        /// <param name="targetVersion">Version to upgrade to.</param>
        /// <param name="versionsToUpgrade">The first version passed in is the preferred version.</param>
        private void AddVersion(string targetVersion, IEnumerable<string> versionsToUpgrade)
        {
            graph.AddVertex(targetVersion);

            int edgeCost = 0;

            foreach (string versionToUpgrade in versionsToUpgrade)
            {
                Edge<string> edge = new Edge<string>(targetVersion, versionToUpgrade);

                graph.AddEdge(edge);
                edgeCosts.Add(edge, edgeCost);

                edgeCost = 10000;
            }
        }

        /// <summary>
        /// Gets the shortest path.
        /// </summary>
        /// <param name="installedVersion">The installed version.</param>
        /// <param name="targetVersion">The target version.</param>
        private List<String> GetUpgradePath(SchemaVersion installedVersion, SchemaVersion targetVersion)
        {
            if (targetVersion==installedVersion)
            {
                return new List<string>();
            }

            TryFunc<string, IEnumerable<Edge<string>>> tryGetPaths;

            try
            {

                tryGetPaths = graph.ShortestPathsDijkstra(edge => edgeCosts[edge], targetVersion.VersionName);
            }
            catch (KeyNotFoundException ex)
            {
                throw new FindVersionUpgradePathException("Couldn't find version in version file.", ex);
            }

            IEnumerable<Edge<string>> path;

            if (tryGetPaths(installedVersion.VersionName, out path))
            {
                return path.Reverse().Select(version => version.Source).ToList();
            }

            throw new FindVersionUpgradePathException(string.Format("Couldn't find path from {0} to {1}.", installedVersion, targetVersion));
        }
    }
}