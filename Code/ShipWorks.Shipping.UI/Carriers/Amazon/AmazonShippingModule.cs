using Autofac;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    public class AmazonShippingModule : Module
    {
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

            builder.RegisterType<AmazonRates>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonLabelService>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonShipmentRequestDetailsFactory>()
                .As<IAmazonShipmentRequestDetailsFactory>();

            builder.RegisterType<AmazonAccountValidator>()
                .AsImplementedInterfaces();
        }
    }
}
