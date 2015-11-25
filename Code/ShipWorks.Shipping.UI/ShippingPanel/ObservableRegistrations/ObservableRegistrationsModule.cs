using Autofac;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register all the pipeline registrations
    /// </summary>
    public class ObservableRegistrationsModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChangeOriginAddressFromTypePipeline>()
                .As<IShippingPanelObservableRegistration>()
                .PreserveExistingDefaults();

            builder.RegisterType<ChangeShipmentTypePipeline>()
                .As<IShippingPanelObservableRegistration>()
                .PreserveExistingDefaults();

            builder.RegisterType<LoadOrderOnSelectionChangedPipeline>()
                .As<IShippingPanelObservableRegistration>()
                .PreserveExistingDefaults();
        }
    }
}