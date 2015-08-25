using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Entry point for the Inversion of Control container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static IoC()
        {
            Current = BuildContainer();
        }

        /// <summary>
        /// Get the current IoC container
        /// </summary>
        public static IContainer Current { get; private set; }

        /// <summary>
        /// Build the main IoC container
        /// </summary>
        /// <returns></returns>
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterAssemblyTypes(typeof(IoC).Assembly)
            //    .AsSelf()
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            
            builder.RegisterType<AmazonShippingWebClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<StoreManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonRates>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

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

            builder.RegisterType<ShippingSettingsWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonShipmentProcessingSynchronizer>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<AmazonServiceControl>()
                .UsingConstructor(typeof(RateControl), typeof(AmazonServiceViewModel))
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonServiceViewModel>();

            return builder.Build();
        }
    }
}
