using Autofac;
using ShipWorks.Data.Model.Custom;
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
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Usps);

            builder.RegisterType<Express1EndiciaShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Express1Endicia);

            builder.RegisterType<Express1UspsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Usps)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();

            builder.RegisterType<Express1EndiciaAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.Express1Endicia)
                .SingleInstance();
        }
    }
}
