using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Warehouse.Orders.DTO
{
    /// <summary>
    /// Response from uploading an order
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class WarehouseUploadOrderResponses
    {
        /// <summary>
        /// The responses for the orders submitted in a UploadOrderRequest
        /// </summary>
        public IEnumerable<WarehouseUploadOrderResponse> OrderResponses { get; set; }
    }
}
