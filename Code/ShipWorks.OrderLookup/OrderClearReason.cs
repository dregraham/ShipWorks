namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Reason the order is being cleared from the view model
    /// </summary>
    public enum OrderClearReason
    {
        /// <summary>
        /// The search is being reset
        /// </summary>
        Reset,

        /// <summary>
        /// An order was not found
        /// </summary>
        OrderNotFound,
    }
}