using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;
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
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.None);

            builder.RegisterType<NoneServiceControl>()
                .Keyed<ServiceControlBase>(ShipmentTypeCode.None);

            builder.RegisterType<NoneShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.None)
                .SingleInstance();

            builder.RegisterType<NoneShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.None)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.None)
                .SingleInstance();
        }
    }
}
