using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Data for splitting orders
    /// </summary>
    public class OrderSplitDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitDefinition(OrderEntity order, IDictionary<long, double> itemQuanities, IDictionary<long, decimal> chargeAmounts, string newOrderNumber)
        {
            Order = order;
            ItemQuantities = itemQuanities;
            ChargeAmounts = chargeAmounts;
            NewOrderNumber = newOrderNumber;
        }

        /// <summary>
        /// Order to be split
        /// </summary>
        public OrderEntity Order { get; }

        /// <summary>
        /// List of item quantities for splitting orders
        /// </summary>
        public IDictionary<long, double> ItemQuantities { get; }

        /// <summary>
        /// List of charge amounts for splitting orders
        /// </summary>
        public IDictionary<long, decimal> ChargeAmounts { get; }

        /// <summary>
        /// Get a new order number for splitting orders
        /// </summary>
        public string NewOrderNumber { get; }
    }
}
