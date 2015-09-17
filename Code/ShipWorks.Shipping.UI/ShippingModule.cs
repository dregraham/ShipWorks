using Autofac;
using ShipWorks.Shipping.Rating;

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
            builder.RegisterType<ShipmentPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<RatingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<ShippingPanelViewModel>();

            builder.RegisterType<RatingPanelViewModel>();

            builder.RegisterType<ShippingPanelConfigurator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeProvider>();

            builder.RegisterType<ShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingPanelShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentAddressValidator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<RatingPanel>()
                .AsSelf();

            builder.RegisterType<RatingPanelViewModel>();

            builder.RegisterType<ShipmentProcessor>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShippingErrorManager>()
                .AsImplementedInterfaces();

            builder.RegisterType<CarrierConfigurationShipmentRefresher>()
                .AsImplementedInterfaces();

            builder.RegisterType<ShipmentTypeFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
