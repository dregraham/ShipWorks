using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IWalmartCombineOrderSearchProvider : ICombineOrderSearchProvider<WalmartCombinedIdentifier>
    {
    }
}