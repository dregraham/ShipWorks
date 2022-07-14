using Autofac;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SFP
{
    /// <summary>
    /// Service registrations for the Amazon shipping carrier
    /// </summary>
    public class AmazonSFPShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<AmazonSFPSettingsControl>()
                .Keyed<SettingsControlBase>(ShipmentTypeCode.AmazonSFP)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonSFPShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.AmazonSFP)
                .SingleInstance();

            builder.RegisterType<AmazonSFPServiceControl>()
                .Keyed<ServiceControlBase>(ShipmentTypeCode.AmazonSFP)
                .ExternallyOwned();

            builder.RegisterType<AmazonSFPRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.AmazonSFP);

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonSFPProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.AmazonSFP);

            builder.RegisterType<AmazonSFPShipmentRequestDetailsFactory>()
                .As<IAmazonSFPShipmentRequestDetailsFactory>();

            builder.RegisterType<AmazonSFPAccountValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonSFPShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.AmazonSFP)
                .ExternallyOwned();

            builder.RegisterType<AmazonSFPShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.AmazonSFP)
                .FindConstructorsWith(new NonDefaultConstructorFinder())
                .SingleInstance();

            builder.RegisterType<AmazonSFPRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.AmazonSFP)
                .AsSelf();

            builder.RegisterType<AmazonSfpRateGroupFactory>()
                .AsImplementedInterfaces();
        }
    }
}
