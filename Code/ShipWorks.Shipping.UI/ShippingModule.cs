using Autofac;
using System.Windows;

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
            builder.RegisterType<ShipmentPanelControl>()
                .Keyed<UIElement>(WpfScreens.ShipmentPanel);
        }
    }
}
