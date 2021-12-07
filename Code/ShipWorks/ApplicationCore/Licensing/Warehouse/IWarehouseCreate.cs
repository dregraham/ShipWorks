using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipEngine.CarrierApi.Client.Model;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Create warehouse
    /// </summary>
    public interface IWarehouseCreate
    {
        /// <summary>
        /// Creates a warehouse on the hub and returns the id of the new warehouse
        /// </summary>
        Task<GenericResult<string>> Create(Details warehouse);
    }
}
