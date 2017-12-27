using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task ShowSuccessConfirmation(IEnumerable<string> orderNumbers);
    }
}
