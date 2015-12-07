using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

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
                .AsSelf()
                .AsImplementedInterfaces();
            
            builder.RegisterType<RateHashingService>()
                .AsSelf();

            // Return a ICertificateInspector
            // if no string is passed it will return a
            // certificate inspector that always returns trusted
            builder.Register<ICertificateInspector>(
                (contaner, parameters) =>
                {
                    string certVerificationData = parameters.TypedAs<string>();

                    if (string.IsNullOrWhiteSpace(certVerificationData))
                    {
                        return new TrustingCertificateInspector();
                    }
                    return new CertificateInspector(certVerificationData);
                });
            
            builder.Register(
                (container, parameters) =>
                    container.ResolveKeyed<IRateHashingService>(parameters.TypedAs<ShipmentTypeCode>()));

            builder.RegisterType<ExcludedServiceTypeRepository>()
                .AsImplementedInterfaces();
        }
    }
}
