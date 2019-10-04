using System;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View model for a routing error
    /// </summary>
    public interface IRoutingErrorViewModel
    {
        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        Task ShowError(Exception exception);
    }
}