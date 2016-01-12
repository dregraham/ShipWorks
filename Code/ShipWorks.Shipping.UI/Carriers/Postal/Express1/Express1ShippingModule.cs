using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.Express1
{
    /// <summary>
    /// Module for the Express1 carrier
    /// </summary>
    public class Express1ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Express1UspsShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<Express1EndiciaShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1UspsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Usps)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();

            builder.RegisterType<Express1UspsAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.Express1Usps)
                .SingleInstance();

            builder.RegisterType<UspsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Express1Usps)
                .ExternallyOwned();

            builder.RegisterType<EndiciaShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Express1Endicia)
                .ExternallyOwned();

            builder.RegisterType<Express1EndiciaShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();

            builder.RegisterType<Express1UspsShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.Express1Usps)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaLabelService>()
                .AsSelf()
                .Keyed<ILabelService>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1UspsLabelService>()
                .AsSelf()
                .Keyed<ILabelService>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<UspsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<EndiciaRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1UspsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<Express1EndiciaRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1UspsAccountRepository>();
        }
    }
}
