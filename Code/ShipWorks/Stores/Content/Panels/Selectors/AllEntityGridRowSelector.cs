using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select entities in an entity grid
    /// </summary>
    public class AllEntityGridRowSelector : IEntityGridRowSelector
    {
        /// <summary>
        /// Select all entities in the grid
        /// </summary>
        public void Select(PagedEntityGrid entityGrid) => entityGrid.SelectAll();
    }
}