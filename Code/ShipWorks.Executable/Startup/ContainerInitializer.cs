using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Services.Dialogs;
using ShipWorks.Shipping.UI;
using ShipWorks.SingleScan;
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
            IoC.BuildRegistrations(container,
                typeof(ShippingDialogService).Assembly,
                typeof(ShippingModule).Assembly,
                typeof(LemonStandStoreModule).Assembly,
                typeof(EnumImageConverter).Assembly,
                typeof(MagentoTwoRestClient).Assembly,
                typeof(ScannerService).Assembly);
    }
}
