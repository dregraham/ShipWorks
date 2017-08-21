using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping
{
    /// <summary>
    /// Interface for uploading shipping details to the Newegg API.
    /// </summary>
    public interface IShippingRequest
    {
        /// <summary>
        /// Ships the specified shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A ShippingResult object containing the details of the response.</returns>
        Task<ShippingResult> Ship(Shipment shipment);
    }
}
