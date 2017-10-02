using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation
{
    /// <summary>
    /// Interface for canceling an order.
    /// </summary>
    public interface ICancelOrderRequest
    {
        /// <summary>
        /// Cancels a Newegg order.
        /// </summary>
        /// <param name="neweggOrder">The newegg order to be canceled.</param>
        /// <param name="reason">The reason for canceling the order.</param>
        /// <returns>A CancellationResult object.</returns>
        Task<CancellationResult> Cancel(Order neweggOrder, CancellationReason reason);
    }
}
