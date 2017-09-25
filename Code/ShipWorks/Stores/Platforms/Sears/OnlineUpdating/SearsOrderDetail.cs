using System;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// DTO for Sears orders
    /// </summary>
    public class SearsOrderDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearsOrderDetail(long orderID, string poNumber, DateTime orderDate)
        {
            OrderID = orderID;
            PoNumber = poNumber;
            OrderDate = orderDate;
        }

        /// <summary>
        /// The existing ShipWorks OrderID
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// The existing ShipWorks OrderID
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Sears order PO number
        /// </summary>
        public string PoNumber { get; set; }
    }
}
