using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShippingProfileEditorDlg>();

            builder.RegisterType<ShippingManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<CachedRatesService>()
                .AsImplementedInterfaces();

            builder.RegisterType<RateHashingService>()
                .AsSelf();

            builder.Register<ICertificateInspector>(
                (contaner, parameters) => new CertificateInspector(parameters.TypedAs<string>()));
        }
    }
}
