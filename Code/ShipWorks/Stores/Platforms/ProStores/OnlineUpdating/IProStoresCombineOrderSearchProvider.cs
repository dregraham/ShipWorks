using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IProStoresCombineOrderSearchProvider : ICombineOrderSearchProvider<OrderUploadDetails>
    {
    }
}