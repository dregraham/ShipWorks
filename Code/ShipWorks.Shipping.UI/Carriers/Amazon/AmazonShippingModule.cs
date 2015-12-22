using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Platforms.Amazon.Mws;

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
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonServiceViewModel>();

            builder.RegisterType<AmazonShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Amazon);

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

            builder.RegisterType<AmazonCreateShipmentRequest>()
                .Keyed<IAmazonShipmentRequest>(AmazonMwsApiCall.CreateShipment);

            builder.RegisterType<AmazonCancelShipmentRequest>()
                .Keyed<IAmazonShipmentRequest>(AmazonMwsApiCall.CancelShipment);

            builder.RegisterType<AmazonRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Amazon)
                .AsSelf();

            builder.RegisterType<AmazonRateGroupFactory>()
                .AsImplementedInterfaces();
        }
    }
}
