using Autofac;
using ShipWorks.Shipping.Carriers.Postal.WebTools;

namespace ShipWorks.Shipping.Carriers.WebTools
{
    public class WebToolsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostalWebShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<WebToolsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();
        }
    }
}
