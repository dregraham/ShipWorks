using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Class for represnting the NetworkSolutions status graph using adjacency lists.  Copied directly from V2.
    /// </summary>
    class NetworkSolutionsStatusGraph
    {
        /// <summary>
        /// Comparer for sorting the arraylist of arrays by length
        /// </summary>
        class LengthComparer : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                Array arrX = x as Array;
                Array arrY = y as Array;

                int compareValue = arrX.Length.CompareTo(arrY.Length);

                if (compareValue == 0)
                {
                    // they are the same length, so sort them based on Ids
                    for (int i = 0; i < arrX.Length; i++)
                    {
                        int itemCompare = ((int)arrX.GetValue(i)).CompareTo((int)arrY.GetValue(i));

                        if (itemCompare == 0)
                            continue; // go to the next element
                        else
                            return itemCompare;

                    }

                    // identical arrays
                    return 0;
                }
                else
                {
                    return compareValue;
                }

            }

            #endregion
        }

        /// <summary>
        /// A single node member of the graph
        /// </summary>
        class Vertex
        {
            // records if this node has been visisted in a search
            private bool encountered = false;

            // Records if this node has been visited in a search
            public bool Encountered
            {
                get { return encountered; }
                set { encountered = value; }
            }

            // unique identifier for this vertex
            private long key = 0;

            /// <summary>
            /// key to uniquely identify this vertex
            /// </summary>
            public long Key
            {
                get { return key; }
            }

            // collection of neighbors
            ArrayList neighbors = new ArrayList();
            public Vertex[] Neighbors
            {
                get
                {
                    return (Vertex[])neighbors.ToArray(typeof(Vertex));
                }
            }

            /// <summary>
            /// Add a neighbor vertex to this vertex
            /// </summary>
            public void AddNeighbor(Vertex neighbor)
            {
                if (!neighbors.Contains(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public Vertex(long key)
            {
                this.key = key;
            }
        }

        // holds each vertex
        List<Vertex> vertices = new List<Vertex>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsStatusGraph()
        {

        }

        /// <summary>
        /// Creates and adds a new vertex to the graph, with the provided unique key
        /// </summary>
        public void AddVertex(long key)
        {
            if (FindVertex(key) == null)
            {
                Vertex newVertex = new Vertex(key);

                vertices.Add(newVertex);
            }
            else
            {
                throw new InvalidOperationException(String.Format("{0} already exists in the graph.", key));
            }
        }

        /// <summary>
        /// Adds a directed edge/path from one vertex to another
        /// </summary>
        public void AddDirectedEdge(long key, long neighbor)
        {
            Vertex source = FindVertex(key);
            Vertex target = FindVertex(neighbor);

            if (source == null)
            {
                throw new InvalidOperationException(String.Format("Unable to find {0} in the graph.", key));
            }

            if (target == null)
            {
                throw new InvalidOperationException(String.Format("Unable to find {0} in the graph.", neighbor));
            }

            source.AddNeighbor(target);
        }

        /// <summary>
        /// Retrieves the vertex with the provided key
        /// </summary>
        Vertex FindVertex(long key)
        {
            // each vertex in the graph is in the vertices collection
            foreach (Vertex vertex in vertices)
            {
                if (vertex.Key == key)
                {
                    return vertex;
                }
            }

            // didn't find it
            return null;
        }

        /// <summary>
        /// Locate paths from startKey to endKey in the graph, returning the shortest one
        /// </summary>
        public long[] GetPath(long startKey, long endKey)
        {
            // collection to hold arrays of potential paths.  We'll chose the shortest path at the end
            Vertex startVertex = FindVertex(startKey);
            if (startVertex == null) throw new InvalidOperationException(String.Format("Unable to locate key {0}", startKey));

            Stack currentPath = new Stack();
            ArrayList paths = new ArrayList();

            // first mark all nodes as being not visited
            foreach (Vertex vertex in vertices)
            {
                vertex.Encountered = false;
            }

            startVertex.Encountered = true;
            FindPath(startVertex, endKey, paths, currentPath);

            // no path found
            if (paths.Count == 0)
            {
                return null;
            }

            // take the shortest path available
            paths.Sort(new LengthComparer());

            // reverse the shortest one, these are the ids in order they need to be requested
            Array.Reverse((Array)paths[0]);

            long[] finalArray = new long[((Array)paths[0]).Length];
            Array.Copy((Array)paths[0], finalArray, finalArray.Length);
            return finalArray;
        }

        /// <summary>
        /// Find paths from the current vertex to the target key
        /// </summary>
        private void FindPath(Vertex currentVertex, long target, ArrayList foundPaths, Stack currentPath)
        {
            currentPath.Push(currentVertex.Key);

            if (currentVertex.Key == target)
            {
                // found the target, record this path
                foundPaths.Add(currentPath.ToArray());
            }
            else
            {
                Vertex[] neighbors = currentVertex.Neighbors;

                // try to quickly find a matching neighbor instead of dumbly following DFS search logic
                bool found = false;
                foreach (Vertex neighbor in neighbors)
                {
                    if (neighbor.Key == target)
                    {
                        // not marking the neighbor as encountered, b/c that would prevent another path from being found
                        FindPath(neighbor, target, foundPaths, currentPath);
                        found = true;
                    }
                }

                // only do regular DFS search if we didn't immediately find the target on a neighbor
                if (!found)
                {
                    foreach (Vertex neighbor in neighbors)
                    {
                        if (!neighbor.Encountered)
                        {
                            neighbor.Encountered = true;
                            FindPath(neighbor, target, foundPaths, currentPath);
                        }
                    }
                }
            }

            currentPath.Pop();
        }
    }
}
