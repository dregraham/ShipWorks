using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select entities in an entity grid
    /// </summary>
    public class FirstEntityGridRowSelector : IEntityGridRowSelector
    {
        /// <summary>
        /// Select the first entity in the grid
        /// </summary>
        public void Select(PagedEntityGrid entityGrid)
        {
            long? firstKey = entityGrid.EntityGateway?.GetKeyFromRow(0);

            if (firstKey.HasValue)
            {
                entityGrid.SelectRows(new[] { firstKey.Value });
            }
        }
    }
}