using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IRakutenOrderSearchProvider : ICombineOrderSearchProvider<RakutenUploadDetails>
    {
    }
}
