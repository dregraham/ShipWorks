namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Details about an order upload
    /// </summary>
    public class OrderUploadDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderUploadDetails(long orderNumber, bool isManual)
        {
            OrderNumber = orderNumber;
            IsManual = isManual;
        }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; }

        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; }
    }
}
