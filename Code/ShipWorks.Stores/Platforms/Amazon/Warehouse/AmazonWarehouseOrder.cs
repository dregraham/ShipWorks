using System;
using System.Collections.ObjectModel;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    public class AmazonWarehouseOrder : WarehouseOrder
    {
        public string AmazonOrderID { get; set; }

        /// <summary>
        /// The fulfillment channel of the order
        /// </summary>
        public int FulfillmentChannel { get; set; }

        /// <summary>
        /// The orders Amazon Prime status
        /// </summary>
        public int IsPrime { get; set; }

        /// <summary>
        /// The Earliest Expected Delivery Date of the order
        /// </summary>
        public DateTime? EarliestExpectedDeliveryDate { get; set; }

        /// <summary>
        /// The Latest Expected Delivery Date of the order
        /// </summary>
        public DateTime? LatestExpectedDeliveryDate { get; set; }

        /// <summary>
        /// The Purchase Order Number of the order
        /// </summary>
        public string PurchaseOrderNumber { get; set; }
		
        /// <summary>
        /// The items that belong to this order
        /// </summary>
        public Collection<AmazonWarehouseItem> Items { get; set; }
    }
}