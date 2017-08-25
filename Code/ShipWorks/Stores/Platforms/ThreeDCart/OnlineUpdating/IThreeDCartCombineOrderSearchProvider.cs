﻿using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IThreeDCartCombineOrderSearchProvider : ICombineOrderSearchProvider<ThreeDCartOnlineUpdatingOrderDetail>
    {
    }
}