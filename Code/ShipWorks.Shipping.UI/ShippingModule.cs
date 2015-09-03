using Autofac;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.Carriers.Amazon;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AmazonShippingWebClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonCredentials>()
                .AsSelf()
                .As<IAmazonCredentials>();

            builder.RegisterType<AmazonShipmentSetupWizard>()
                .Keyed<ShipmentTypeSetupWizardForm>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonAccountEditorDlg>();
            builder.RegisterType<AmazonAccountEditorViewModel>();

            builder.RegisterType<AmazonSettingsControl>()
                .Keyed<SettingsControlBase>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonAccountManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonServiceControl>()
                .UsingConstructor(typeof(RateControl), typeof(AmazonServiceViewModel))
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
        }
    }
}
