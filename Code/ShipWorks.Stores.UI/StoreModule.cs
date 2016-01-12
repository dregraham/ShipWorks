using Autofac;
using ShipWorks.Stores.Services;

namespace ShipWorks.Stores.UI
{
    /// <summary>
    /// Module for stores assembly
    /// </summary>
    public class StoreModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<StoreTypeManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
