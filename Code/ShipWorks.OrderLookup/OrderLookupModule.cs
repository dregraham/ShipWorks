using Autofac;
using ShipWorks.OrderLookup.Controls;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Autofac registration module for the OrderLookup assembly
    /// </summary>
    public class OrderLookupModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(OrderLookupViewModelPanel<>))
                .As(typeof(IOrderLookupPanelViewModel<>));
        }
    }
}
