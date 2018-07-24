using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// API endpoints for communicating with Overstock
    /// </summary>
    public interface IOverstockWebClientEndpoints
    {
        /// <summary>
        /// Returns the resource path for accessing a specific order's shipments.  Also used for creating a new shipment.
        /// </summary>
        string GetUploadShipmentResource();

        /// <summary>
        /// Returns the resource path for downloading orders
        /// </summary>
        string GetOrdersResource(Range<DateTime> downloadRange);
    }
}
