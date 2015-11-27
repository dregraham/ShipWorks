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
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.IsAssignableTo<IShippingPanelObservableRegistration>())
                .AsImplementedInterfaces();
        }
    }
}