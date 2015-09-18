using Autofac;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
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
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsWorldShip);
            
            builder.RegisterType<UpsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();
        }
    }
}
