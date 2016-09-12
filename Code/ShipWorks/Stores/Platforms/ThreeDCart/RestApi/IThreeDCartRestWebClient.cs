﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Threading;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public interface IThreeDCartRestWebClient
    {
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
        void UploadShipmentDetails(ThreeDCartShipment shipment);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        void UpdateOrderStatus(ThreeDCartShipment shipment);

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