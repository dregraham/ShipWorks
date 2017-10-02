using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IAmazonOrderSearchProvider : ICombineOrderSearchProvider<string>
    {
    }
}