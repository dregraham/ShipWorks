using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// Service registrations for the Amazon shipping carrier
    /// </summary>
    public class AmazonShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AmazonShippingWebClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonShipmentSetupWizard>()
                .Keyed<ShipmentTypeSetupWizardForm>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonSettingsControl>()
                .Keyed<SettingsControlBase>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonServiceControl>()
                .UsingConstructor(typeof(RateControl), typeof(AmazonServiceViewModel), typeof(AmazonShipmentType))
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonServiceViewModel>();

            builder.RegisterType<AmazonShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonRatingService>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonLabelService>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonShipmentRequestDetailsFactory>()
                .As<IAmazonShipmentRequestDetailsFactory>();

            builder.RegisterType<AmazonNotLinkedFootnoteFactory>()
                .As<IAmazonNotLinkedFootnoteFactory>()
                .ExternallyOwned();

            if (!(InterapptiveOnly.IsInterapptiveUser ^ InterapptiveOnly.MagicKeysDown))
            {
                builder.RegisterType<AmazonUspsRateFilter>()
                    .As<IAmazonRateGroupFilter>();

                builder.RegisterType<AmazonUspsLabelEnforcer>()
                    .AsImplementedInterfaces();

                builder.RegisterType<AmazonUpsRateFilter>()
                    .As<IAmazonRateGroupFilter>();

                builder.RegisterType<AmazonUpsLabelEnforcer>()
                    .AsImplementedInterfaces();

                builder.RegisterType<AmazonAllowedCarriersRateGroupFilter>()
                    .As<IAmazonRateGroupFilter>();
            }

            builder.RegisterType<AmazonAccountValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();
        }
    }
}
