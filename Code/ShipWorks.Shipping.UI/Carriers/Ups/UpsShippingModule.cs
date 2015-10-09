using Autofac;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Shipping.Carriers.UPS.Promo;

namespace ShipWorks.Shipping.UI.Carriers.Ups
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

            builder.RegisterType<UpsPromoPolicy>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}