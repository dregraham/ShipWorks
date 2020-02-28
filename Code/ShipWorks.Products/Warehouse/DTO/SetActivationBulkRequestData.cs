using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Request data for setting the activation status in bulk for a product on the Hub
    /// </summary>
    [Obfuscation]
    public class SetActivationBulkRequestData : IWarehouseProductRequestData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SetActivationBulkRequestData(IEnumerable<Guid> productIDs, bool activation)
        {
            ProductIds = productIDs.Select(x => x.ToString("D"));
            Activation = activation;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SetActivationBulkRequestData(IEnumerable<Guid> productIDs, string warehouseId, bool activation)
        {
            ProductIds = productIDs.Select(x => x.ToString("D"));
            WarehouseId = warehouseId;
            Activation = activation;
        }

        /// <summary>
        /// Ids of the products to set activation
        /// </summary>
        public IEnumerable<string> ProductIds { get; set; }

        /// <summary>
        /// Id of the warehouse for which to set activation status
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Is the product enabled or not
        /// </summary>
        public bool Activation { get; set; }

    }

    /// <summary>
    /// Response data for setting the activation status in bulk for a product on the Hub
    /// </summary>
    [Obfuscation]
    public class SetActivationBulkResponseData
    {
        /// <summary>
        /// Items that were changed
        /// </summary>
        public IEnumerable<SetActivationBulkResponseItem> Items { get; set; }
    }

    [Obfuscation]
    public class SetActivationBulkResponseItem
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Version of the product on the Hub
        /// </summary>
        /// <remarks>
        /// This is used for optimistic concurrency
        /// </remarks>
        public int Version { get; set; }
    }
}
