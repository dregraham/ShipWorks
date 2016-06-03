using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public interface IOdbcOrderLoader
    {
        void Load(OdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> records);
    }
}