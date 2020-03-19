using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Request for getting a product by productId 
    /// </summary>
    [Obfuscation]
    public class GetProductRequestData : IWarehouseProductRequestData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productId"></param>
        public GetProductRequestData(string hubProductId)
        {
            HubProductId = hubProductId;
        }

        /// <summary>
        /// ProductId of product to be retrieved
        /// </summary>
        public string HubProductId { get; set; }

        /// <summary>
        /// WarehouseId, which will not be used as this request is at the customer level
        /// and not warehouse level.
        /// </summary>
        public string WarehouseId { get; set; }
    }
}
