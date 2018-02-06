using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    public interface IOrderMotionCombineOrderSearchProvider : ICombineOrderSearchProvider<OrderDetail>
    {
    }
}