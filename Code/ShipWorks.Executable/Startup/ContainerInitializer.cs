using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.UI;
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
        public static void Initialize() =>
            IoC.Initialize(typeof(ShippingModule).Assembly,
                typeof(LemonStandStoreModule).Assembly,
                typeof(EnumImageConverter).Assembly);
    }
}
