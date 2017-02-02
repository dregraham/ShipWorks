using System;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;

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

            builder.RegisterType<UspsWebClient>()
                .AsImplementedInterfaces()
                .UsingConstructor(typeof(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>),
                    typeof(IUspsWebServiceFactory),
                    typeof(Func<string, ICertificateInspector>),
                    typeof(UspsResellerType));

            builder.RegisterType<UspsResellerType>()
                .AsImplementedInterfaces();

            builder.RegisterType<UspsAccountManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<AssociateShipworksWithItselfRequest>();

            builder.RegisterType<GlobalPostLabelNotification>()
                .As<IGlobalPostLabelNotification>();
        }
    }
}
