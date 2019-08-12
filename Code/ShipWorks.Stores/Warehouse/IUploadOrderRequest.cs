using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    public interface IUploadOrderRequest
    {
        /// <summary>
        /// The batch
        /// </summary>
        Guid Batch { get; }

        /// <summary>
        /// The order
        /// </summary>
        WarehouseOrder Order { get; }

        /// <summary>
        /// Submit this UploadOrderRequest to the hub and return the response
        /// </summary>
        Task<GenericResult<WarehouseUploadOrderResponse>> Submit(OrderEntity order, IStoreEntity store);
    }
}