using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using Interapptive.Shared;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Content;
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
        [NDependIgnoreLongMethod]
        public static IContainer Initialize(IContainer container, params Assembly[] assemblies)
        {
            Assembly[] allAssemblies = assemblies.Union(new[] { typeof(IoC).Assembly }).ToArray();
            var builder = new ContainerBuilder();

            builder.RegisterType<ClipboardHelper>()
                .AsSelf()
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

            builder.RegisterInstance(Messenger.Current)
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<FilterHelperWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ValidatedAddressManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<StampsAddressValidationWebClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ValidatedAddressScope>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register((_, p) => new AddressSelector(p.OfType<string>().FirstOrDefault()))
                .AsImplementedInterfaces();

            builder.RegisterType<ShipBillAddressEditorDlg>();

            builder.Register(c => Program.MainForm)
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();

            builder.RegisterType<SchedulerProvider>()
                .AsImplementedInterfaces();

            builder.RegisterInstance(SqlDateTimeProvider.Current)
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<TangoWebClientWrapper>()
                .As<ITangoWebClient>();

            builder.Register(c => UserSession.Security)
                .As<ISecurityContext>()
                .ExternallyOwned();

            builder.RegisterAssemblyModules(allAssemblies);

            builder.RegisterType<EditionManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<LogEntryFactory>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<ObjectReferenceManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<PdfDocument>()
                .AsImplementedInterfaces();

            builder.RegisterType<UserSessionWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<WeightConverter>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(allAssemblies)
                .Where(x => x.IsAssignableTo<IInitializeForCurrentSession>() ||
                    x.IsAssignableTo<IInitializeForCurrentUISession>() ||
                    x.IsAssignableTo<ICheckForChangesNeeded>() ||
                    x.IsAssignableTo<IInitializeForCurrentDatabase>() ||
                    x.IsAssignableTo<IMainFormElementRegistration>())
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register((_, parameters) => LogManager.GetLogger(parameters.TypedAs<Type>()));

            foreach (IComponentRegistration registration in builder.Build().ComponentRegistry.Registrations)
            {
                container.ComponentRegistry.Register(registration);
            }

            current = container;

            return current;
        }
    }
}
