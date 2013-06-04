using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        ShippingResult Ship(Shipment shipment);
    }
}
