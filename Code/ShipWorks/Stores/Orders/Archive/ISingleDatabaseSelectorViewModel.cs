using System.Collections.Generic;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.Orders.Archive
{
    public interface ISingleDatabaseSelectorViewModel
    {
        /// <summary>
        /// Gets whether a single database instance will be returned or not.
        /// </summary>
        ISqlDatabaseDetail SelectSingleDatabase(IEnumerable<ISqlDatabaseDetail> databaseDetail);

        /// <summary>
        /// Gets the selected database chosen the user.
        /// </summary>
        ISqlDatabaseDetail SelectedDatabase { get; set; }
    }
}