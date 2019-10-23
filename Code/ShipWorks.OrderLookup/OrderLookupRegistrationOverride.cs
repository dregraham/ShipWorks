using Autofac;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.OrderLookup.ScanToShip;

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
            builder.RegisterType<OrderLookupShipmentModel>().As<IOrderLookupShipmentModel>().SingleInstance();
            builder.RegisterType<ScanToShipViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ScanPackViewModel>().As<IScanPackViewModel>().SingleInstance();
        }
    }
}
