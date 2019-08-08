using System;
using Newtonsoft.Json;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Request to upload order to the hub
    /// </summary>
    public class UploadOrderRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadOrderRequest(Guid batch, WarehouseOrder order)
        {
            Batch = batch;
            Order = order;
        }

        /// <summary>
        /// The batch
        /// </summary>
        [JsonProperty("batch")]
        public Guid Batch { get; }

        /// <summary>
        /// The order
        /// </summary>
        [JsonProperty("order")]
        public WarehouseOrder Order { get; }

        
    }
}
