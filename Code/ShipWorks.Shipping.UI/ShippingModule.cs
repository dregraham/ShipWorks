using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using ShipWorks.AddressValidation;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Accounts;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.MessageHandlers;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    [NDependIgnore]
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddressValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<CarrierAccountRetrieverFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CarrierConfigurationShipmentRefresher>()
                .AsImplementedInterfaces();

            builder.RegisterType<CarrierShipmentAdapterFactory>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<CustomsManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<DimensionsManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ExcludedPackageTypeRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ExcludedServiceTypeRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<InsuranceViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<OrderSelectionChangedHandler>()
                .AsImplementedInterfaces();

            builder.RegisterType<OtherShipmentViewModel>();

            builder.RegisterType<RatingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<RateSelectionFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RatingPanel.RatingPanel>();

            builder.RegisterType<RatingPanelViewModel>();

            builder.RegisterType<ShipmentViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<ShipmentAddressValidator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentLoaderService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentPackageTypesBuilderFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentProcessor>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentProcessorService>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingProfileManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentServicesBuilderFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentTypeProvider>();

            builder.RegisterType<ShippingAccountListProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingErrorManager>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ShippingManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingOriginManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<ShippingPanelViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<ShippingProfileEditorDlg>();

            builder.RegisterType<ShippingViewModelFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<StampsAddressValidationWebClient>()
                .AsImplementedInterfaces();

            builder.Register((container, parameters) =>
            {
                return parameters.TypedAs<ShipmentTypeCode>() == ShipmentTypeCode.Other ?
                    (IShipmentViewModel) container.Resolve<OtherShipmentViewModel>() :
                    container.Resolve<ShipmentViewModel>();
            });

            builder.RegisterType<ShippingProfileEditorDlg>();

            builder.RegisterType<CachedRatesService>()
                .AsImplementedInterfaces();

            builder.RegisterType<RateHashingService>();

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

            builder.Register(
                (container, parameters) =>
                    container.ResolveKeyed<ShipmentType>(parameters.TypedAs<ShipmentTypeCode>()));

            builder.RegisterType<ExcludedServiceTypeRepository>()
                .AsImplementedInterfaces();
        }
    }
}
