using System;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Api endpoints for communicating with Overstock
    /// </summary>
    public static class OverstockWebClientEndpoints
    {
        /// <summary>
        /// Returns the resource path for accessing a specific order's shipments.  Also used for creating a new shipment.
        /// </summary>
        public static string GetUploadShipmentResource()
        {
            return "shipments";
        }

        /// <summary>
        /// Returns the resource path for downloading orders
        /// </summary>
        public static string GetOrdersResource(DateTime startTime, DateTime endTime)
        {
            return $"salesorders?startTime={startTime.ToString("o")}&endTime={endTime.ToString("o")}";
        }
    }
}
