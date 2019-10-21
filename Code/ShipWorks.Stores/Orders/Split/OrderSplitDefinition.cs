﻿using System.Collections.Generic;
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
        public OrderSplitDefinition(OrderEntity order, IDictionary<long, decimal> itemQuanities, IDictionary<long, decimal> chargeAmounts, string newOrderNumber, OrderSplitterType orderSplitterType)
        {
            Order = order;
            ItemQuantities = itemQuanities;
            ChargeAmounts = chargeAmounts;
            NewOrderNumber = newOrderNumber;
            OrderSplitterType = orderSplitterType;
        }

        /// <summary>
        /// Order Splitter Type
        /// </summary>
        public OrderSplitterType OrderSplitterType { get; }

        /// <summary>
        /// Order to be split
        /// </summary>
        public OrderEntity Order { get; }

        /// <summary>
        /// List of item quantities for splitting orders
        /// </summary>
        public IDictionary<long, decimal> ItemQuantities { get; }

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
