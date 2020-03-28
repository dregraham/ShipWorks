using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Factory for creating an OrderResponse
    /// </summary>
    public interface IOrdersResponseFactory
    {
        /// <summary>
        /// Create an OrderResponse from an OrderEntity
        /// </summary>
        OrderResponse CreateOrdersResponse(IOrderEntity orderEntity);

        /// <summary>
        /// Create a ProcessShipmentResponse from a ProcessShipmentResult
        /// </summary>
        ProcessShipmentResponse CreateProcessShipmentResponse(ProcessShipmentResult processShipmentResult);
    }
}
