using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Jet.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IJetOrderSearchProvider : ICombineOrderSearchProvider<string>
    {
    }
}