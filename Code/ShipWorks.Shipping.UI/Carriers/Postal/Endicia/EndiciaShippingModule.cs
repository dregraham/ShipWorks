using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
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

            builder.RegisterType<EndiciaLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Endicia);

            builder.RegisterType<EndiciaAccountRepository>()
                .As<ICarrierAccountRepository<EndiciaAccountEntity>>()
                .Keyed<ICarrierAccountRepository<EndiciaAccountEntity>>(ShipmentTypeCode.Endicia)
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.Endicia)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaAccountRepository>()
                .Keyed<ICarrierAccountRepository<EndiciaAccountEntity>>(ShipmentTypeCode.Express1Endicia);

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
