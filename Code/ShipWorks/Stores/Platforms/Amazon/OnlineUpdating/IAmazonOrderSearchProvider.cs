using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IAmazonOrderSearchProvider : ICombineOrderSearchProvider<string>
    {
    }
}