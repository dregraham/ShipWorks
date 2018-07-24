using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// API endpoints for communicating with Overstock
    /// </summary>
    [Component]
    public class OverstockWebClientEndpoints : IOverstockWebClientEndpoints
    {
        /// <summary>
        /// Returns the resource path for accessing a specific order's shipments.  Also used for creating a new shipment.
        /// </summary>
        public string GetUploadShipmentResource()
        {
            return "shipments";
        }

        /// <summary>
        /// Returns the resource path for downloading orders
        /// </summary>
        public string GetOrdersResource(Range<DateTime> downloadRange)
        {
            return $"salesorders?startTime={downloadRange.Start.ToString("o")}&endTime={downloadRange.End.ToString("o")}";
        }
    }
}
