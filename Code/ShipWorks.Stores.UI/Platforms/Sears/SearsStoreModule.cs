using Autofac;
using Autofac.Core;
using Interapptive.Shared.Security;
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
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(IEncryptionProvider),
                        (pi, ctx) => ctx.ResolveKeyed<IEncryptionProvider>(EncryptionProviderType.AesForSears)));
        }
    }
}