using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    public class AmazonWarehouseItem : WarehouseOrderItem
    {
        /// <summary>
        /// The items Amazon order item code
        /// </summary>
        public string AmazonOrderItemCode { get; set; }

        /// <summary>
        /// The items ASIN
        /// </summary>
        public string ASIN { get; set; }

        /// <summary>
        /// The items Condition Note
        /// </summary>
        public string ConditionNote { get; set; }
    }
}