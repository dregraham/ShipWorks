using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Order details for performing an online update
    /// </summary>
    public class BigCommerceOnlineOrderDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOnlineOrderDetails(IOrderEntity order)
        {
            OrderID = order.OrderID;
            IsManual = order.IsManual;
            OrderNumber = order.OrderNumber;
            OrderNumberComplete = order.OrderNumberComplete;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOnlineOrderDetails(long orderID, bool isManual, long orderNumber, string orderNumberComplete)
        {
            OrderID = orderID;
            IsManual = isManual;
            OrderNumber = orderNumber;
            OrderNumberComplete = orderNumberComplete;
        }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; set; }

        /// <summary>
        /// Order ID
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// Order number
        /// </summary>
        public long OrderNumber { get; set; }

        /// <summary>
        /// Order number complete
        /// </summary>
        public string OrderNumberComplete { get; set; }
    }
}