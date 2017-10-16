using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Shipment type prefetch path collector
    /// </summary>
    public class ShipmentTypePrefetchPath : IShipmentTypePrefetchPath
    {
        private static ShipmentTypePrefetchPath empty = new ShipmentTypePrefetchPath(Enumerable.Empty<PrefetchPathContainer>());
        private IEnumerable<PrefetchPathContainer> paths;

        /// <summary>
        /// Constructor
        /// </summary>
        private ShipmentTypePrefetchPath(IEnumerable<PrefetchPathContainer> paths)
        {
            this.paths = paths;
        }

        /// <summary>
        /// Get an empty collector
        /// </summary>
        public static IShipmentTypePrefetchPath Empty => empty;

        /// <summary>
        /// Apply to a shipment query
        /// </summary>
        public EntityQuery<ShipmentEntity> ApplyTo(EntityQuery<ShipmentEntity> query) =>
            ApplyTo(query, (q, x) => q.WithPath(x));

        /// <summary>
        /// Apply to an existing prefetch path
        /// </summary>
        public IPrefetchPathElement2 ApplyTo(IPrefetchPathElement2 query) =>
            ApplyTo(query, (q, x) => q.WithSubPath(x));

        /// <summary>
        /// Apply to an arbitrary object using the apply method specified
        /// </summary>
        public T ApplyTo<T>(T query, Func<T, IPrefetchPathElement2, T> applyMethod)
        {
            var graph = paths.Aggregate(Graph.Empty, BuildGraph);

            return paths.GroupBy(x => x.PathKey)
                .Select(x => graph.NodeFor(x.Key))
                .Select(x => ApplyChildren(x, graph, ImmutableHashSet.Create<int>(x.Key)))
                .Aggregate(query, applyMethod);
        }

        /// <summary>
        /// Add a prefetch provider to this collector
        /// </summary>
        public IShipmentTypePrefetchPath With(IShipmentTypePrefetchProvider provider) =>
            new ShipmentTypePrefetchPath(paths.Append(provider.GetPath()));

        /// <summary>
        /// Build the graph
        /// </summary>
        private Graph BuildGraph(Graph graph, PrefetchPathContainer container) =>
            container.Children.Aggregate(graph.AddNodeAndEdges(container), BuildGraph);

        /// <summary>
        /// Apply children paths
        /// </summary>
        private IPrefetchPathElement2 ApplyChildren(KeyValuePair<int, IPrefetchPathElement2> root, Graph graph, IImmutableSet<int> alreadyAdded)
        {
            return graph.EdgesFor(root.Key)
                .Except(alreadyAdded)
                .Select(graph.NodeFor)
                .Aggregate(root.Value, (p, x) => p.WithSubPath(ApplyChildren(x, graph, alreadyAdded.Add(x.Key))));
        }

        /// <summary>
        /// Graph that will help apply prefetch paths
        /// </summary>
        private class Graph
        {
            private static Graph empty = new Graph(ImmutableDictionary.Create<int, IPrefetchPathElement2>(), ImmutableDictionary.Create<int, IImmutableList<int>>());

            private readonly IImmutableDictionary<int, IPrefetchPathElement2> nodes;
            private readonly IImmutableDictionary<int, IImmutableList<int>> edges;

            /// <summary>
            /// Constructor
            /// </summary>
            private Graph(IImmutableDictionary<int, IPrefetchPathElement2> nodes, IImmutableDictionary<int, IImmutableList<int>> edges)
            {
                this.nodes = nodes;
                this.edges = edges;
            }

            /// <summary>
            /// Get an empty graph
            /// </summary>
            public static Graph Empty => empty;

            /// <summary>
            /// Add a set of edges if they don't already exist
            /// </summary>
            internal Graph AddEdges(int pathKey, IEnumerable<int> newEdges) =>
                new Graph(nodes, edges.UpdateValue(pathKey,
                    () => ImmutableList.Create<int>(),
                    x => x.AddRange(newEdges.Distinct())));

            /// <summary>
            /// Add a node if it doesn't already exist
            /// </summary>
            internal Graph AddNode(PrefetchPathContainer path) =>
                nodes.ContainsKey(path.PathKey) ?
                    this :
                    new Graph(nodes.Add(path.PathKey, path.Path), edges);

            /// <summary>
            /// Add a node and its edges
            /// </summary>
            internal Graph AddNodeAndEdges(PrefetchPathContainer container) =>
                this.AddNode(container)
                    .AddEdges(container.PathKey, container.Children.Select(x => x.PathKey));

            /// <summary>
            /// Get the node for a given key
            /// </summary>
            internal KeyValuePair<int, IPrefetchPathElement2> NodeFor(int key) =>
                new KeyValuePair<int, IPrefetchPathElement2>(key, nodes[key]);

            /// <summary>
            /// Get the edges for a given key
            /// </summary>
            internal IEnumerable<int> EdgesFor(int rootKey) =>
                edges.ContainsKey(rootKey) ?
                    (IEnumerable<int>) edges[rootKey] :
                    Enumerable.Empty<int>();
        }
    }
}
