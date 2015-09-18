using Autofac;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Shipping.Carriers.Express1
{
    public class ExpressShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Express1UspsShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<Express1EndiciaShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1ShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Usps)
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();
        }
    }
}
