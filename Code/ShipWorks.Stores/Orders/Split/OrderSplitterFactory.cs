using Autofac;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Create order splitters based on OrderSplitterType
    /// </summary>
    [Component]
    public class OrderSplitterFactory : IOrderSplitterFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitterFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Create an order splitter based on OrderSplitterType
        /// </summary>
        public IOrderSplitter Create(OrderSplitterType orderSplitterType)
        {
            return lifetimeScope.ResolveKeyed<IOrderSplitter>(OrderSplitterType.Local);
        }
    }
}
