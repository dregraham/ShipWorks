﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Threading;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    /// <summary>
    /// ThreeDCart REST downloader
    /// </summary>
    public interface IThreeDCartRestWebClient
    {
        /// <summary>
        /// Attempt to get an order count to test connecting to ThreeDCart.  If any error, assume connection failed.
        /// </summary>
        void TestConnection();

        /// <summary>
        /// Gets the orders.
        /// </summary>
        IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate, int offset);

        /// <summary>
        /// Gets the product.
        /// </summary>
        ThreeDCartProduct GetProduct(int catalogID);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        IResult UploadShipmentDetails(ThreeDCartShipment shipment);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        IResult UpdateOrderStatus(ThreeDCartShipment shipment);

        /// <summary>
        /// Loads the progress reporter.
        /// </summary>
        void LoadProgressReporter(IProgressReporter progressReporter);

        /// <summary>
        /// Gets the order count.
        /// </summary>
        int GetOrderCount(DateTime startDate, int offset);
    }
}