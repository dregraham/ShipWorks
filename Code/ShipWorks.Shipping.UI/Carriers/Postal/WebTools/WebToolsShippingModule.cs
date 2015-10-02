using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Services;

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
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<PostalWebShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();

            builder.RegisterType<PostalWebToolsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.PostalWebTools)
                .ExternallyOwned();
        }
    }
}
