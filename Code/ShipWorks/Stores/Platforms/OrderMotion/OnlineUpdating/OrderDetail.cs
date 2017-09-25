namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Details about a specific order to upload
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetail(long orderNumber, long orderMotionShipmentID, bool isManual)
        {
            OrderNumber = orderNumber;
            OrderMotionShipmentID = orderMotionShipmentID;
            IsManual = isManual;
        }

        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; }

        /// <summary>
        /// OrderMotion shipment ID
        /// </summary>
        public long OrderMotionShipmentID { get; }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; }
    }
}