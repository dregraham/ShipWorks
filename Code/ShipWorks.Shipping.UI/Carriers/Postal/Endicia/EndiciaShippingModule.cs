using Autofac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    /// <summary>
    /// Shipping module for endicia
    /// </summary>
    public class EndiciaShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Endicia);

            builder.RegisterType<EndiciaShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Endicia)
                .SingleInstance();

            builder.RegisterType<EndiciaShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Endicia)
                .ExternallyOwned();

            builder.RegisterType<EndiciaShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.Endicia)
                .SingleInstance();

            builder.RegisterType<EndiciaRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Endicia)
                .Keyed<ISupportExpress1Rates>(ShipmentTypeCode.Endicia)
                .Keyed<ISupportExpress1Rates>(ShipmentTypeCode.Express1Endicia)
                .AsSelf();

            builder.RegisterType<EndiciaRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Endicia)
                .AsSelf();
        }
    }
}
