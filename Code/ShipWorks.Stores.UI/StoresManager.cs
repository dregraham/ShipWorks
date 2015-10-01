using Autofac;
using ShipWorks.Stores.Services;

namespace ShipWorks.Stores.UI
{
    /// <summary>
    /// Module for registering store specific dependencies
    /// </summary>
    public class StoresModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StoreManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
