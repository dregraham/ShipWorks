using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select a specific list of entities
    /// </summary>
    public class SpecificEntityGridRowSelector : IEntityGridRowSelector
    {
        private IEnumerable<long> keys;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keys"></param>
        public SpecificEntityGridRowSelector(IEnumerable<long> keys)
        {
            this.keys = keys.ToReadOnly();
        }

        /// <summary>
        /// Select the specific entities in the grid
        /// </summary>
        public void Select(PagedEntityGrid entityGrid)
        {
            entityGrid.SelectRows(Enumerable.Empty<long>());
            IEnumerable<long> keysToSelect = entityGrid.EntityGateway?.GetOrderedKeys().Intersect(keys) ??
                Enumerable.Empty<long>();

            if (keysToSelect.None())
            {
                EntityGridRowSelector.First.Select(entityGrid);
            }
            else
            {
                entityGrid.SelectRows(keysToSelect);
            }
        }
    }
}