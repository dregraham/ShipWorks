using Autofac;

namespace ShipWorks.Stores.Platforms.SellerCloud
{
    /// <summary>
    /// SellerCloud store registrations.
    /// </summary>
    public class SellerCloudStoreModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SellerCloudStoreType>()
                .Keyed<StoreType>(StoreTypeCode.SellerCloud)
                .ExternallyOwned();
        }
    }
}
