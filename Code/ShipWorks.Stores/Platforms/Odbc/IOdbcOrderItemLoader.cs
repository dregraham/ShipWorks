using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Loads order items into order
    /// </summary>
    public interface IOdbcOrderItemLoader
    {
        /// <summary>
        /// Loads items from odbcRecords into the order
        /// </summary>
        void Load(IOdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> odbcRecords);
    }
}