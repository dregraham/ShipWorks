using System;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Exception generated while splitting an order on the Hub
    /// </summary>
    public class OrderSplitterHubException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitterHubException(Exception ex) : base(ex.Message, ex)
        {

        }
    }
}