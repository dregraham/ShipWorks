using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    public class EndiciaShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Endicia);

            builder.RegisterType<EndiciaShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Endicia)
                .SingleInstance();

            builder.RegisterType<EndiciaAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.Endicia)
                .SingleInstance();
        }
    }
}
