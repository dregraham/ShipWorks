using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select entities in the entities panel
    /// </summary>
    public class AllEntityGridRowSelector : IEntityGridRowSelector
    {
        /// <summary>
        /// Select entities on an entity grid
        /// </summary>
        public void Select(PagedEntityGrid entityGrid) => entityGrid.SelectAll();
    }
}