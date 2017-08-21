namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Order details for uploading
    /// </summary>
    public class OrderUploadDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderUploadDetail(long orderID, long orderNumber, bool isManual)
        {
            OrderID = orderID;
            OrderNumber = orderNumber;
            IsManual = isManual;
        }

        /// <summary>
        /// Original ID of the order
        /// </summary>
        public long OrderID { get; }

        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; }
    }
}