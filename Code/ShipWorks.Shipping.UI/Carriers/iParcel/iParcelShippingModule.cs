using Autofac;

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
                .Keyed<ShipmentType>(ShipmentTypeCode.iParcel);

            builder.RegisterType<iParcelShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.iParcel)
                .SingleInstance();
        }
    }
}
