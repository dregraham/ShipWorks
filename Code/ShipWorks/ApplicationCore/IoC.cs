using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Autofac.Builder;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.ApplicationCore.Licensing.FeatureRestrictions;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Common;
using ShipWorks.Data;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Data.Connection;
using ShipWorks.Editions;
using ShipWorks.Editions.Brown;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Tokens;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;

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
        /// All ShipWorks assemblies
        /// </summary>
        public static Assembly[] AllAssemblies { get; private set; }

        /// <summary>
        /// Begin a lifetime scope from which dependencies can be resolved
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope() => current.BeginLifetimeScope();

        /// <summary>
        /// Begin a lifetime scope from which dependencies can be resolved
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) =>
            current.BeginLifetimeScope(configurationAction);

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        public static IContainer Initialize(Action<ContainerBuilder> addExtraRegistrations, params Assembly[] assemblies) =>
            current = Build(addExtraRegistrations, assemblies);

        /// <summary>
        /// Build the registrations
        /// </summary>
        /// <remarks>
        /// This should be used for tests since the Initialize method sets the current container, which is not thread safe
        /// </remarks>
        public static IContainer InitializeForUnitTests(IContainer container) =>
            current = container;

        /// <summary>
        /// Build the registrations
        /// </summary>
        /// <remarks>
        /// This should be used for tests since the Initialize method sets the current container, which is not thread safe
        /// </remarks>
        public static IContainer Initialize(IContainer container, params Assembly[] assemblies)
        {
            AllAssemblies = assemblies.Union(new[] { typeof(IoC).Assembly }).ToArray();
            var builder = new ContainerBuilder();

            AddRegistrations(builder, AllAssemblies);

#pragma warning disable CS0618 // Type or member is obsolete
            builder.Update(container);
#pragma warning restore CS0618 // Type or member is obsolete

            return current = container;
        }

        /// <summary>
        /// Build the IoC container
        /// </summary>
        public static IContainer Build(Action<ContainerBuilder> addExtraRegistrations, params Assembly[] assemblies)
        {
            AllAssemblies = assemblies.Union(new[] { typeof(IoC).Assembly }).ToArray();
            var builder = new ContainerBuilder();

            AddRegistrations(builder, AllAssemblies);

            addExtraRegistrations(builder);

            return builder.Build();
        }

        /// <summary>
        /// Add registrations to the given builder
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void AddRegistrations(ContainerBuilder builder, Assembly[] allAssemblies)
        {
            builder.RegisterSource(new OrderedRegistrationSource());

            builder.RegisterType<ClipboardHelper>()
                .AsSelf();

            builder.RegisterType<OrderManager>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ClrHelper>()
                .AsImplementedInterfaces();

            builder.RegisterType<DateTimeProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<EnvironmentWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(AccountManagerBase<>))
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<SqlServerInstaller>()
                .AsSelf();

            builder.RegisterType<ValidatedAddressScope>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register((_, p) => new AddressSelector(p.OfType<string>().FirstOrDefault()))
                .AsImplementedInterfaces();

            builder.RegisterType<TrackedDurationEvent>()
                .As<ITrackedDurationEvent>();

            builder.RegisterType<TrackedEvent>()
                .As<ITrackedEvent>();

            builder.RegisterType<ShipBillAddressEditorDlg>();

            builder.Register(c => Program.MainForm)
                .As<Control>()
                .As<IWin32Window>()
                .As<IMainForm>()
                .ExternallyOwned();

            builder.RegisterInstance(SqlDateTimeProvider.Current)
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.Register(c => UserSession.Security ?? SecurityContext.EmptySecurityContext)
                .As<ISecurityContext>()
                .ExternallyOwned();

            builder.RegisterAssemblyModules(allAssemblies);

            builder.RegisterType<LogEntryFactory>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<UserService>()
                .AsImplementedInterfaces();

            // Pass "Func<Type, ILog> logFactory" as a dependency and get your log with:
            // log = logFactory(typeof (type))
            builder.Register((_, parameters) => LogManager.GetLogger(parameters.TypedAs<Type>()));

            builder.RegisterType<PdfBlackAndWhiteDocument>()
                .AsImplementedInterfaces();

            RegisterWrappers(builder);
            RegisterLicensingDependencies(builder);
            RegisterLicenseEnforcers(builder);
            RegisterDialogs(builder);

            builder.RegisterType<User32Devices>()
                .As<IUser32Devices>();

            builder.RegisterType<User32Input>()
                .As<IUser32Input>();

            builder.RegisterType<WeightConverter>()
                .AsImplementedInterfaces();

            builder.RegisterType<ConfigurationDataWrapper>()
                .As<IConfigurationData>();

            builder.RegisterGeneric(typeof(OrderedCompositeManipulator<,>))
                .As(typeof(IOrderedCompositeManipulator<,>));

            builder.RegisterGeneric(typeof(CompositeValidator<,>))
                .As(typeof(ICompositeValidator<,>));

            builder.RegisterType<ScaleReaderWrapper>()
                .As<IScaleReader>();

            builder.Register(c => SqlSession.Current)
                .AsImplementedInterfaces()
                .ExternallyOwned();

            IDictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> registrationCache =
                new Dictionary<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>();

            ComponentAttribute.Register(builder, registrationCache, allAssemblies);
            ServiceAttribute.Register(builder, registrationCache, allAssemblies);
            KeyedComponentAttribute.Register(builder, registrationCache, allAssemblies);
            NamedComponentAttribute.Register(builder, registrationCache, allAssemblies);

            builder.Register((c, _) => Program.ExecutionMode.IsUISupported ?
                    (IUserLoginWorkflow) c.Resolve<UserLoginWorkflow>() :
                    c.Resolve<BackgroundUserLoginWorkflow>())
                .As<IUserLoginWorkflow>();

            builder.RegisterType<TemplateTokenProcessorWrapper>()
                .As<ITemplateTokenProcessor>()
                .SingleInstance();

            builder.RegisterType<HttpRequestSubmitterFactory>()
                .As<IHttpRequestSubmitterFactory>();

            foreach (var taskDescriptor in ActionTaskManager.TaskDescriptors)
            {
                builder.RegisterType(taskDescriptor.SystemType).AsSelf();
            }

            builder.Register((c, _) =>
                    c.Resolve<IShippingSettings>().FetchReadOnly().ShipSenseEnabled ?
                        (IKnowledgebase) c.Resolve<Knowledgebase>() :
                        c.Resolve<NullKnowledgebase>())
                .As<IKnowledgebase>();

            builder.Register((c, _) =>
                    c.Resolve<IShippingSettings>().FetchReadOnly().ShipSenseEnabled ?
                        (IShipSenseLoaderGateway) c.Resolve<ShipSenseLoaderGateway>() :
                        c.Resolve<NullShipSenseLoaderGateway>())
                .As<IShipSenseLoaderGateway>();

            builder.Register((c, _) =>
                    c.Resolve<IShippingSettings>().FetchReadOnly().ShipSenseEnabled ?
                        (IShipSenseSynchronizer) c.Resolve<ShipSenseSynchronizer>() :
                        c.Resolve<NullShipSenseSynchronizer>())
                .As<IShipSenseSynchronizer>();
        }

        /// <summary>
        /// Registers the dialog/control types.
        /// </summary>
        private static void RegisterDialogs(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaAccountEditorDlg>();
            builder.RegisterType<UspsAccountInfoControl>();

            builder.RegisterType<ShipWorksOpenFileDialog>().AsImplementedInterfaces();
            builder.RegisterType<ShipWorksSaveFileDialog>().AsImplementedInterfaces();
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

            builder.RegisterType<UspsAccountSetupActivity>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<CustomerLicenseWriter>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicenseReader>()
                .AsImplementedInterfaces();

            builder.RegisterType<StoreLicense>()
                .AsSelf();

            builder.RegisterType<CustomerLicenseActivationService>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipWorksLicense>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentTypeSetupActivity>()
                .AsImplementedInterfaces();

            builder.RegisterType<StreamCipherKey>()
                .Keyed<ICipherKey>(CipherContext.Stream);
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
            builder.RegisterType<BrownEditionUtility>()
                .AsImplementedInterfaces();

            builder.RegisterType<TangoWebClientFactory>()
                .AsImplementedInterfaces();

            builder.Register((x) => x.Resolve<ITangoWebClientFactory>().CreateWebClient());

            builder.Register((x) => x.Resolve<ITangoWebClientFactory>().CreateWebRequestClient());

            builder.Register<ICredentialStore>(x => TangoCredentialStore.Instance);

            builder.RegisterType<DataResourceManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DeletionServiceWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<EditionManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<FilterHelperWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ObjectReferenceManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<PostalUtilityWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingSettingsWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ValidatedAddressManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        /// <summary>
        /// Begin a lifetime scope and include any registration overrides that are of type T
        /// </summary>
        public static ILifetimeScope BeginLifetimeScopeWithOverrides<T>() where T : IRegistrationOverride
        {
            var orderLookupRegistrationOverrides = UnsafeGlobalLifetimeScope.Resolve<Owned<IEnumerable<T>>>().Value;

            return BeginLifetimeScope(builder => orderLookupRegistrationOverrides.ForEach(x => x.Register(builder)));
        }
    }
}
