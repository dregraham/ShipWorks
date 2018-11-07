using Autofac;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// IoC registration overrides needed for the order lookup mode
    /// </summary>
    public class OrderLookupRegistrationOverride : IOrderLookupRegistrationOverride
    {
        /// <summary>
        /// Register the changes
        /// </summary>
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<OrderLookupShipmentModel>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
