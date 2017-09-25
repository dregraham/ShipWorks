using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IYahooCombineOrderSearchProvider : ICombineOrderSearchProvider<string>
    {
    }
}
