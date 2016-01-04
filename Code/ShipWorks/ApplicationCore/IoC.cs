using Autofac;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using System.Reflection;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Editions;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Entry point for the Inversion of Control container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Get the current IoC container
        /// </summary>
        private static IContainer current;

        /// <summary>
        /// Get the current global lifetime scope
        /// </summary>
        /// <remarks>This should ONLY be used in situations where a new lifetime scope cannot be created or disposed.
        /// Any dependency resolved through this will NEVER be released, which could cause a memory leak if the dependency
        /// is not marked as ExternallyOwned or SingleInstance</remarks>
        public static ILifetimeScope UnsafeGlobalLifetimeScope => current;

        /// <summary>
        /// Begin a lifetime scope from which dependencies can be resolved
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope()
        {
            return current.BeginLifetimeScope();
        }

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        public static void Initialize(params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<StoreManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingSettingsWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OrderManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DataResourceManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DateTimeProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterGeneric(typeof(AccountManagerBase<>))
                .AsSelf()
                .SingleInstance();

            builder.Register(context => Messenger.Current)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyModules(assemblies);

            builder.RegisterType<EditionManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<TangoWebClientWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<UserManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicense>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicenseWriter>()
                .AsImplementedInterfaces();

            builder.Register(c => Program.MainForm)
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();

            builder.RegisterType<ConfigurationDataWrapper>()
                .AsImplementedInterfaces();

            current = builder.Build();
        }
    }
}
