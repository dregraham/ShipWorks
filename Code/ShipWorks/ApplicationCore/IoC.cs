using Autofac;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Entry point for the Inversion of Control container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static IoC()
        {
            Current = BuildContainer();
        }

        /// <summary>
        /// Get the current IoC container
        /// </summary>
        public static IContainer Current { get; private set; }

        /// <summary>
        /// Build the main IoC container
        /// </summary>
        /// <returns></returns>
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(IoC).Assembly).ExternallyOwned();

            builder.RegisterType<AmazonShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            return builder.Build();
        }
    }
}
