using System;
using System.Reflection;
using Autofac;
using System.Windows.Forms;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Pdf;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping.Profiles;

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
            RegisterLicensingDependencies(builder);
            RegisterLicenseEnforcers(builder);
            RegisterDialogs(builder);

            current = builder.Build();
        }

        /// <summary>
        /// Registers the dialog/control types.
        /// </summary>
        private static void RegisterDialogs(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaAccountEditorDlg>();
            builder.RegisterType<EndiciaAccountManagerControl>();
            builder.RegisterType<EndiciaBuyPostageDlg>();
            builder.RegisterType<UspsAccountInfoControl>();
            builder.RegisterType<UspsAccountManagerControl>();
            builder.RegisterType<UspsPurchasePostageDlg>();
        }

        /// <summary>
        /// Registers the license types.
        /// </summary>
        private static void RegisterLicensingDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerLicense>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<LicenseService>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

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

            builder.RegisterType<ShipWorksLicense>()
                .AsImplementedInterfaces();

            builder.RegisterType<SqlSchemaVersion>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentTypeSetupActivity>()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Registers the Enforcers
        /// </summary>
        private static void RegisterLicenseEnforcers(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ILicenseEnforcer)))
                .Where(t => typeof(ILicenseEnforcer).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IFeatureRestriction)))
                .Where(t => typeof(IFeatureRestriction).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
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

            builder.RegisterType<TangoWebClientFactory>()
                .AsImplementedInterfaces();

            builder.Register<ITangoWebClient>((x) =>  x.Resolve<ITangoWebClientFactory>().CreateWebClient());

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

            builder.RegisterType<BrownEditionUtility>()
                .AsImplementedInterfaces();

            builder.RegisterType<PostalUtilityWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingProfileManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<UserSessionWrapper>()
                .AsImplementedInterfaces()
                .UsingConstructor();
        }
    }
}
