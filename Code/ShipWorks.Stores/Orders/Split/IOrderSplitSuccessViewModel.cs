using System.Collections.Generic;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View model for the OrderSplitSuccessDialog
    /// </summary>
    public interface IOrderSplitSuccessViewModel
    {
        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        void ShowSuccessConfirmation(IEnumerable<string> orderNumbers);
    }
}
