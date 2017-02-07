using System.Collections.Generic;

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
        bool Confirm(IEnumerable<long?> orderIds, int numberOfMatchedOrders, string scanText);
    }
}