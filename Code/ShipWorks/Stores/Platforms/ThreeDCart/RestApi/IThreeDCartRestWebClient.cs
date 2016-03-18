using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public interface IThreeDCartRestWebClient
    {
        IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate);

        ThreeDCartProduct GetProduct(int catalogID);
    }
}