using Autofac;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NoneShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.None);

            builder.RegisterType<NoneServiceControl>()
                .Keyed<ServiceControlBase>(ShipmentTypeCode.None);

            builder.RegisterType<NoneShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.None)
                .SingleInstance();
        }
    }
}
