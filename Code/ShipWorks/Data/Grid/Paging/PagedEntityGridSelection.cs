using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Divelements.SandGrid;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Paging
{
    public partial class PagedEntityGrid
    {
        /// <summary>
        /// Represents the selection for the paged grid
        /// </summary>
        class PagedEntityGridSelection : IGridSelection
        {
            PagedEntityGrid grid;
            int version = 0;

            // The selection in the sort order its displayed in the grid.
            List<long> orderedSelection;

            /// <summary>
            /// Constructor
            /// </summary>
            public PagedEntityGridSelection(PagedEntityGrid grid)
            {
                this.grid = grid;

                grid.SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged);
            }

            /// <summary>
            /// The grid selection has changed
            /// </summary>
            void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                VersionChanged();
            }

            /// <summary>
            /// Notify that the ordering\position of the selected items has changed
            /// </summary>
            public void OnOrderingChanged()
            {
                VersionChanged();
            }

            /// <summary>
            /// Increment the version in response to a change
            /// </summary>
            private void VersionChanged()
            {
                version++;
                orderedSelection = null;
            }

            /// <summary>
            /// Any time selection changes this value increments
            /// </summary>
            public int Version
            {
                get { return version; }
            }

            /// <summary>
            /// The selected element count
            /// </summary>
            public int Count
            {
                get
                {
                    return grid.virtualSelection.Count;
                }
            }

            /// <summary>
            /// The selected entity keys, This does not necessarily have to be in sort order
            /// </summary>
            public IEnumerable<long> Keys
            {
                get 
                {
                    return grid.virtualSelection;
                }
            }

            /// <summary>
            /// The selected entity keys in grid sort order
            /// </summary>
            public IEnumerable<long> OrderedKeys
            {
                get
                {
                    // Some of the stuff we do here assumes we are on the UI thread, and rows won't be changing out from under us.
                    Debug.Assert(!grid.InvokeRequired);

                    // We've already cached it, just return it
                    if (orderedSelection == null)
                    {
                        Stopwatch sw = Stopwatch.StartNew();

                        // If just a single is selected, we can just return it.
                        if (grid.virtualSelection.Count <= 1)
                        {
                            orderedSelection = grid.virtualSelection.ToList();
                        }

                        // Otherwise loop through every key in the gateway (in order - b\c that's how we make sure we return this in order), and see which
                        // ones are selected
                        else
                        {
                            orderedSelection = new List<long>(grid.virtualSelection.Count);

                            foreach (long key in grid.EntityGateway.GetOrderedKeys())
                            {
                                if (grid.virtualSelection.Contains(key))
                                {
                                    orderedSelection.Add(key);
                                }

                                // Break early if we have them all
                                if (orderedSelection.Count == grid.virtualSelection.Count)
                                {
                                    break;
                                }
                            }
                        }

                        log.DebugFormat("[Selection] Created ordered selection cache {0}", sw.Elapsed.TotalSeconds);
                    }

                    return orderedSelection;
                }
            }
        }
    }
}
