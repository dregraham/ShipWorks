using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Order information used for uploading
    /// </summary>
    public class OnlineOrder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineOrder(OnlineOrderDetails baseOrder) :
            this(baseOrder, new[] { baseOrder })
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineOrder(OrderEntity order) :
            this(new OnlineOrderDetails(order))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineOrder(OnlineOrderDetails baseOrder, IEnumerable<OnlineOrderDetails> ordersToUpload)
        {
            OrderID = baseOrder.OrderID;
            OrderNumberComplete = baseOrder.OrderNumberComplete;
            OrderNumber = baseOrder.OrderNumber;
            OrdersToUpload = ordersToUpload.ToReadOnly();
        }

        /// <summary>
        /// ID of the base order
        /// </summary>
        public long OrderID { get; }

        /// <summary>
        /// Order number of the base order
        /// </summary>
        public long OrderNumber { get; }

        /// <summary>
        /// Complete order number of the base order
        /// </summary>
        public string OrderNumberComplete { get; }

        /// <summary>
        /// Orders to use for uploading to BigCommerce
        /// </summary>
        public IEnumerable<OnlineOrderDetails> OrdersToUpload { get; }

        /// <summary>
        /// Are all the orders manual
        /// </summary>
        public bool AreAllManual => OrdersToUpload.All(x => x.IsManual);
    }
}
