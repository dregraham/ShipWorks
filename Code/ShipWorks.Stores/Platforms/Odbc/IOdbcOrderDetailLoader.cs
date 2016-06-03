using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Loads order information into an order
    /// </summary>
    public interface IOdbcOrderDetailLoader
    {
        /// <summary>
        /// Loads the order details into the given entity
        /// </summary>
        void Load(IOdbcFieldMap map, OrderEntity order);
    }
}