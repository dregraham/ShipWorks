using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Loads item attributes into an item
    /// </summary>
    public interface IOdbcItemAttributeLoader
    {
        /// <summary>
        /// Loads item attributes into an item
        /// </summary>
        void Load(IOdbcFieldMap map, OrderItemEntity item);
    }
}