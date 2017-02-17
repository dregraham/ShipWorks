namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Confirm what the user wants to do when auto printing and multiple matching orders are found
    /// </summary>
    public interface ISingleScanOrderConfirmationService
    {
        /// <summary>
        /// Confirms that the order with the given orderId should be printed.
        /// </summary>
        bool Confirm(long orderId, int numberOfMatchedOrders, string scanText);
    }
}