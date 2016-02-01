using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.Usps
{
    public class UspsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UspsRateDiscountedFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<UspsRatePromotionFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<UspsShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Usps)
                .SingleInstance();

            builder.RegisterType<UspsAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.Usps)
                .SingleInstance();

            builder.RegisterType<UspsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Usps)
                .ExternallyOwned();

            builder.RegisterType<UspsShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.Usps)
                .SingleInstance();

            builder.RegisterType<UspsLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Usps)
                .Keyed<ISupportExpress1Rates>(ShipmentTypeCode.Usps)
                .AsSelf();

            builder.RegisterType<UspsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsAccountRepository>()
                .As<ICarrierAccountRepository<UspsAccountEntity>>();
        }
    }
}
