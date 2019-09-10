using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Create order splitters based on OrderSplitterType
    /// </summary>
    [Component]
    public class OrderSplitterFactory : IOrderSplitterFactory
    {
        private readonly IIndex<OrderSplitterType, IOrderSplitter> splitters;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitterFactory(IIndex<OrderSplitterType, IOrderSplitter> splitters)
        {
            this.splitters = splitters;
        }

        /// <summary>
        /// Create an order splitter based on OrderSplitterType
        /// </summary>
        public IOrderSplitter Create(OrderSplitterType orderSplitterType) => splitters[orderSplitterType];
    }
}
