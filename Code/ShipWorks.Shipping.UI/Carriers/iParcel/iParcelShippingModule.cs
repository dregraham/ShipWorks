using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<iParcelShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.iParcel)
                .SingleInstance();

            builder.RegisterType<iParcelShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.iParcel)
                .ExternallyOwned();
        }
    }
}
