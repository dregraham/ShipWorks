using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

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

            builder.RegisterType<NoneShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.None)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<NoneShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.None)
                .ExternallyOwned();

            builder.RegisterType<NullShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.None)
                .SingleInstance();

            builder.RegisterType<NoneLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.None);

            builder.RegisterType<EmptyRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.None);

            builder.RegisterType<EmptyRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.None);
        }
    }
}
