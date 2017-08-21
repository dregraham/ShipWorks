using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface INeweggCombineOrderSearchProvider : ICombineOrderSearchProvider<OrderUploadDetail>
    {
    }
}