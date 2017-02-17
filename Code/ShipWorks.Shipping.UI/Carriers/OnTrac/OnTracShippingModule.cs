using Autofac;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Shipping module for the OnTrac carrier
    /// </summary>
    public class OnTracShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OnTracShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.OnTrac);

            builder.RegisterType<OnTracShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.OnTrac)
                .SingleInstance();

            builder.RegisterType<OnTracShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.OnTrac)
                .ExternallyOwned();

            builder.RegisterType<OnTracShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.OnTrac)
                .SingleInstance();

            builder.RegisterType<OnTracRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.OnTrac);

            builder.RegisterType<OnTracRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.OnTrac);
        }
    }
}
