using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Orders
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
