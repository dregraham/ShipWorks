using Autofac;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.UI.Carriers.None
{
    /// <summary>
    /// IoC registration module for the None shipment type
    /// </summary>
    public class NoneShippingModule : Module
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
