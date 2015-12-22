using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.Common;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Content;
using ShipWorks.UI.Controls;
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
        public static ILifetimeScope BeginLifetimeScope() => current.BeginLifetimeScope();

        /// <summary>
        /// Begin a lifetime scope from which dependencies can be resolved
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) =>
            current.BeginLifetimeScope(configurationAction);

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        public static void Initialize(params Assembly[] assemblies)
        {
            var builder = new ContainerBuilder();

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
                .SingleInstance();

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

            builder.RegisterType<AddressSelector>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingProfileManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipBillAddressEditorDlg>();

            builder.Register(c => Program.MainForm)
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();

            builder.RegisterType<SchedulerProvider>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyModules(assemblies.Union(new[] { typeof(IoC).Assembly }).ToArray());

            //builder.Register(context => Messenger.Current)
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            builder.RegisterType<EditionManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<UserSessionWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<WeightConverter>()
                .AsImplementedInterfaces();

            builder.Register((_, parameters) => LogManager.GetLogger(parameters.TypedAs<Type>()));

            builder.Register((_, parameters) => new SqlAdapter(parameters.TypedAs<bool>()));

            current = builder.Build();
        }
    }
}
