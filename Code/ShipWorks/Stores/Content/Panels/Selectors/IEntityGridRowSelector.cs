using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select entities in the entities panel
    /// </summary>
    public interface IEntityGridRowSelector
    {
        /// <summary>
        /// Select entities on an entity grid
        /// </summary>
        void Select(PagedEntityGrid entityGrid);
    }
}
