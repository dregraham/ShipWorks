﻿using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IOverstockOrderSearchProvider : ICombineOrderSearchProvider<string>
    {
    }
}