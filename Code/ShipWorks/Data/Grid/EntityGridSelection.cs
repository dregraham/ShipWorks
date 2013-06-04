using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;

namespace ShipWorks.Data.Grid
{
    public partial class EntityGrid
    {
        /// <summary>
        /// Represents the selection for the grid
        /// </summary>
        class EntityGridSelection : IGridSelection
        {
            EntityGrid grid;
            int version = 0;

            /// <summary>
            /// Constructor
            /// </summary>
            public EntityGridSelection(EntityGrid grid)
            {
                this.grid = grid;

                grid.SortChanged += new GridEventHandler(OnSortChanged);
                grid.SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged);
            }

            /// <summary>
            /// The selection has changed
            /// </summary>
            void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                version++;
            }

            /// <summary>
            /// The underlying sort has changed
            /// </summary>
            void OnSortChanged(object sender, GridEventArgs e)
            {
                // Changing the sort affects the order in which we return ordered selected rows
                version++;
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
                get { return grid.SelectedElements.Count; }
            }

            /// <summary>
            /// The selected entity keys.  This does not necessarily have to be in sort order
            /// </summary>
            public IEnumerable<long> Keys
            {
                get 
                {
                    // The SelectedElements collection is not necessarily in order, but the Rows collection is
                    return grid.Rows.OfType<EntityGridRow>()
                        .Where(r => r.Selected && r.EntityID != null)
                        .Select(r => (long) r.EntityID);
                }
            }

            /// <summary>
            /// The selected entity keys in grid sort order
            /// </summary>
            public IEnumerable<long> OrderedKeys
            {
                get
                {
                    // For the plain EntityGrid, its already in order
                    return Keys;
                }
            }
        }
    }
}
