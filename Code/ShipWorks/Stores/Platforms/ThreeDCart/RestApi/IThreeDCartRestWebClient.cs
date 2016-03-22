using System;
using System.Collections.Generic;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public interface IThreeDCartRestWebClient
    {
        IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate);

        ThreeDCartProduct GetProduct(int catalogID);

        void UploadShipmentDetails(long orderID, ThreeDCartShipment shipment);

        void UpdateOrderStatus(long orderID, int statusID);

        void LoadProgressReporter(IProgressReporter progressReporter);

        int GetOrderCount();
    }
}