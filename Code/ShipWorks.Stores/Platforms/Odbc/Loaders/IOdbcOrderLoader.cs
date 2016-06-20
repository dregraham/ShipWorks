using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Loads order information into an order
    /// </summary>
    public interface IOdbcOrderLoader
    {
        /// <summary>
        /// Loads the order information from the given Odbc records into the given order entity.
        /// </summary>
        void Load(IOdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> odbcRecords);
    }
}