using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Sears.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for orders
    /// </summary>
    public interface ISearsCombineOrderSearchProvider : ICombineOrderSearchProvider<SearsOrderDetail>
    {
    }
}
