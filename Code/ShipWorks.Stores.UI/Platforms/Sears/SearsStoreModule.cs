using Autofac;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Stores.Platforms.Sears;

namespace ShipWorks.Stores.UI.Platforms.Sears
{
    /// <summary>
    /// Make necessary registrations for Sears
    /// </summary>
    public class SearsStoreModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SearsCredentials>()
                .AsSelf();

            builder.RegisterType<SearsCipherKey>()
                .Keyed<ICipherKey>(CipherContext.Sears);
        }
    }
}