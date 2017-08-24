using System;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// DTO for Sears shipment tracking
    /// </summary>
    public class SearsTracking
    {
        /// <summary>
        /// The existing ShipWorks OrderID
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// The no longer existing original order ID (for combined orders)
        /// </summary>
        public long OriginalOrderID { get; set; }

        /// <summary>
        /// Sears order PO number
        /// </summary>
        public string PoNumber { get; set; }

        /// <summary>
        /// Sears order date
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Sears order date
        /// </summary>
        public DateTime ShipDate { get; set; }

        /// <summary>
        /// Sears order shipment tracking number
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Sears order shipment carrier
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// Sears order shipment method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Sears order line number
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Sears order item id
        /// </summary>
        public string ItemID { get; set; }

        /// <summary>
        /// Sears order quantity
        /// </summary>
        public double Quantity { get; set; }
    }
}
