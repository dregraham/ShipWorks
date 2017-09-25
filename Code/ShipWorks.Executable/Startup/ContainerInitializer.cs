using System.Reflection;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
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
        public static IContainer Initialize() =>
            Initialize(new ContainerBuilder().Build());

        /// <summary>
        /// Initialize the IoC container
        /// </summary>
        public static IContainer Initialize(IContainer container) =>
            IoC.Initialize(BuildRegistrations(container));

        /// <summary>
        /// Build the registrations in IoC container
        /// </summary>
        public static IContainer BuildRegistrations(IContainer container) =>
            IoC.BuildRegistrations(container, AllAssemblies);

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
            typeof(ScannerService).Assembly
        };
    }
}
