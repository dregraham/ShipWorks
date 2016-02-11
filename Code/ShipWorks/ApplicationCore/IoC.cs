using System;
using System.Reflection;
using Autofac;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Pdf;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

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
            
            builder.RegisterType<OrderManager>()
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

            builder.RegisterInstance(Messenger.Current)
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyModules(assemblies);
            
            builder.RegisterType<LogEntryFactory>()
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.RegisterType<UserService>()
                .AsImplementedInterfaces();

            builder.Register(c => Program.MainForm)
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();

            // Pass "Func<Type, ILog> logFactory" as a dependency and get your log with:
            // log = logFactory(typeof (type))
            builder.Register((_, parameters) => LogManager.GetLogger(parameters.TypedAs<Type>()));

            builder.RegisterType<DatabaseIdentifier>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<PdfDocument>()
                .AsImplementedInterfaces();

            RegisterWrappers(builder);
            RegisterLicenseTypes(builder);

            current = builder.Build();
        }

        /// <summary>
        /// Registers the license types.
        /// </summary>
        private static void RegisterLicenseTypes(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerLicense>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<LicenseService>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<CustomerLicenseActivationActivity>()
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.RegisterType<UspsAccountSetupActivity>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<CustomerLicenseWriter>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicenseReader>()
                .AsImplementedInterfaces();

            builder.RegisterType<StoreLicense>()
                .AsSelf();

            builder.RegisterType<LicenseEncryptionProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CustomerLicenseActivationService>()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Registers the wrappers.
        /// </summary>
        private static void RegisterWrappers(ContainerBuilder builder)
        {
            builder.RegisterType<DeletionServiceWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ObjectReferenceManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<TangoWebClientWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<StoreManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingSettingsWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DataResourceManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<EditionManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<UserSessionWrapper>()
                .AsImplementedInterfaces();
        }
    }
}
