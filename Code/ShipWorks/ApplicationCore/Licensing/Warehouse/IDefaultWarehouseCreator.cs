using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Creates a default warehouse
    /// </summary>
    public interface IDefaultWarehouseCreator
    {
        /// <summary>
        /// Creates a default warehouse if needed
        /// </summary>
        Task Create(StoreEntity store);

        /// <summary>
        /// Returns true if no linked warehouse and one needs to be created
        /// </summary>
        Task<bool> NeedsDefaultWarehouse();
    }
}
