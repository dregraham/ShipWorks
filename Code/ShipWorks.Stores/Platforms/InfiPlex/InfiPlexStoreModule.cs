using Autofac;

namespace ShipWorks.Stores.Platforms.InfiPlex
{
    /// <summary>
    /// InfiPlexStoreType registration module
    /// </summary>
    public class InfiPlexStoreModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InfiPlexStoreType>()
                .Keyed<StoreType>(StoreTypeCode.InfiPlex)
                .ExternallyOwned();
        }
    }
}
