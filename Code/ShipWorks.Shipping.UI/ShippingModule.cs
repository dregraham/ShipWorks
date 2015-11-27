using Autofac;
using ShipWorks.Shipping.Settings;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Accounts;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.UI.MessageHandlers;
using ShipWorks.Shipping.UI.Services;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;

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
            builder.RegisterType<ShippingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<RatingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<ShippingPanelViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<ShipmentViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<RatingPanelViewModel>();

            builder.RegisterType<ShippingConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeProvider>();

            builder.RegisterType<ShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentAddressValidator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RatingPanel.RatingPanel>();

            builder.RegisterType<AddressViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<ShipmentProcessor>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingErrorManager>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<CarrierConfigurationShipmentRefresher>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentTypeFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ExcludedServiceTypeRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ExcludedPackageTypeRepository>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CustomsManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentServicesBuilderFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentPackageBuilderFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RateSelectionFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingAccountListProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingOriginManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CarrierAccountRetrieverFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CarrierShipmentAdapterFactory>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<ShipmentLoaderService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OrderSelectionChangedHandler>();

            builder.RegisterType<MessageHelperWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingProfileEditorDlg>();

            builder.RegisterType<ShippingManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<AddressValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<StampsAddressValidationWebClient>()
                .AsImplementedInterfaces();

            builder.RegisterType<StoreTypeManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingViewModelFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
