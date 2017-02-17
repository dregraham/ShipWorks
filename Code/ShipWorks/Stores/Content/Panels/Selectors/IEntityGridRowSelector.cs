using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Stores.Content.Panels.Selectors
{
    /// <summary>
    /// Select entities in an entity grid
    /// </summary>
    public interface IEntityGridRowSelector
    {
        /// <summary>
        /// Select entities in an entity grid
        /// </summary>
        void Select(PagedEntityGrid entityGrid);
    }
}
