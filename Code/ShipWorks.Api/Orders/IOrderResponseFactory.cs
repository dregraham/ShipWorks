using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Factory for creating an OrderResponse
    /// </summary>
    public interface IOrderResponseFactory
    {
        /// <summary>
        /// Create an OrderResponse from an OrderEntity
        /// </summary>
        /// <returns></returns>
        OrderResponse Create(IOrderEntity orderEntity);
    }
}
