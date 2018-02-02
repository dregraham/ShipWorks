using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Data for splitting orders
    /// </summary>
    public class SplitOrderDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SplitOrderDefinition(IDictionary<long, float> itemQuanities, IDictionary<long, decimal> chargeAmounts, string newOrderNumber)
        {
            ItemQuantities = itemQuanities;
            ChargeAmounts = chargeAmounts;
            NewOrderNumber = newOrderNumber;
        }

        /// <summary>
        /// List of item quantities for splitting orders
        /// </summary>
        public IDictionary<long, float> ItemQuantities { get; }

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
