using System;
using System.Reflection;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls;
using ShipWorks.Shipping.Services.Dialogs;
using ShipWorks.Shipping.UI;
using ShipWorks.SingleScan;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.UI.Platforms.LemonStand;
using ShipWorks.UI.ValueConverters;

namespace ShipWorks.Startup
{
    /// <summary>
    /// IoC container initializer
    /// </summary>
    /// <remarks>This class is so that integration tests and other code can initialize
    /// the IoC container exactly as ShipWorks does at runtime.</remarks>
    public static class ContainerInitializer
    {
        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        /// <remarks>
        /// This method sets the current IoC instance, which means it is NOT thread-safe
        /// </remarks>
        public static IContainer Initialize() =>
            IoC.Initialize(x => { }, AllAssemblies);

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        /// <remarks>
        /// This method sets the current IoC instance, which means it is NOT thread-safe
        /// </remarks>
        public static IContainer Initialize(IContainer container) =>
            IoC.Initialize(container, AllAssemblies);

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        /// <param name="addExtraRegistrations">
        /// Extra registrations that will be applied after the defaults
        /// </param>
        /// <remarks>
        /// This method sets the current IoC instance, which means it is NOT thread-safe
        /// </remarks>
        public static IContainer Initialize(Action<ContainerBuilder> addExtraRegistrations) =>
            IoC.Initialize(addExtraRegistrations, AllAssemblies);

        /// <summary>
        /// Build the IoC container
        /// </summary>
        /// <remarks>
        /// This method does NOT set the current IoC instance, which means it is thread-safe
        /// </remarks>
        public static IContainer Build() =>
            IoC.Build(x => { }, AllAssemblies);

        /// <summary>
        /// Build the IoC container
        /// </summary>
        /// <param name="addExtraRegistrations">
        /// Extra registrations that will be applied after the defaults
        /// </param>
        /// <remarks>
        /// This method does NOT set the current IoC instance, which means it is thread-safe
        /// </remarks>
        public static IContainer Build(Action<ContainerBuilder> addExtraRegistrations) =>
            IoC.Build(addExtraRegistrations, AllAssemblies);

        /// <summary>
        /// All ShipWorks assemblies to be used for dependency injection
        /// </summary>
        public static Assembly[] AllAssemblies => new[]
        {
            // Interapptive.Shared
            typeof(HttpVariableRequestSubmitter).Assembly,
            // ShipWorks.Shared
            typeof(StoreTypeCode).Assembly,
            // ShipWorks.Core
            typeof(MainForm).Assembly,
            // ShipWorks.Shipping
            typeof(ShippingDialogService).Assembly,
            // ShipWorks.Shipping.UI
            typeof(ShippingModule).Assembly,
            // ShipWorks.Stores
            typeof(MagentoTwoRestClient).Assembly,
            // ShipWorks.Stores.UI
            typeof(LemonStandStoreModule).Assembly,
            // ShipWorks.UI
            typeof(EnumImageConverter).Assembly,
            // ShipWorks.SingleScan
            typeof(ScannerService).Assembly,
            // ShipWorks.Data.Model
            typeof(CommonEntityBase).Assembly,
            // ShipWorks.OrderLookup
            typeof(OrderLookupControlHost).Assembly
        };
    }
}
