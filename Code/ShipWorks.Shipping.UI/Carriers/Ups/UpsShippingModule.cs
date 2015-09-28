using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Shipping.Carriers.Ups
{
    public class UpsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UpsOltShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsWorldShip);
            
            builder.RegisterType<UpsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();

            builder.RegisterType<UpsServiceManagerFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();
        }
    }
}
