using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Request to upload order to the hub
    /// </summary>
    public interface IUploadOrdersRequest
    {
        /// <summary>
        /// The batch
        /// </summary>
        Guid? Batch { get; }

        /// <summary>
        /// The order
        /// </summary>
        IEnumerable<WarehouseOrder> Orders { get; }

        /// <summary>
        /// Submit this UploadOrderRequest to the hub and return the response
        /// </summary>
        Task<GenericResult<IEnumerable<WarehouseUploadOrderResponse>>> Submit(IEnumerable<OrderEntity> orders, IStoreEntity store, bool assignBatch);
    }
}