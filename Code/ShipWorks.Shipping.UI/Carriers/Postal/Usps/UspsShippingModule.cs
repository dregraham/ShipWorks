using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.Carriers.Usps
{
    public class UspsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UspsShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Usps)
                .SingleInstance();

            builder.RegisterType<UspsAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.Usps)
                .SingleInstance();
        }
    }
}
