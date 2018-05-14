using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        string GetOrdersResource(DateTime startTime, DateTime endTime);
    }
}
