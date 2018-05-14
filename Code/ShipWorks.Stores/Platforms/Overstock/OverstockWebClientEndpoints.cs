﻿using System;
using Interapptive.Shared.ComponentRegistration;

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
        public string GetOrdersResource(DateTime startTime, DateTime endTime)
        {
            return $"salesorders?startTime={startTime.ToString("o")}&endTime={endTime.ToString("o")}";
        }
    }
}
