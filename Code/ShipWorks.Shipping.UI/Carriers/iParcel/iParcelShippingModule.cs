using Autofac;
using ShipWorks.Data.Model.Custom;

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
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.iParcel)
                .SingleInstance();
        }
    }
}
