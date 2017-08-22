using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IProStoresCombineOrderSearchProvider : ICombineOrderSearchProvider<OrderUploadDetails>
    {
    }
}