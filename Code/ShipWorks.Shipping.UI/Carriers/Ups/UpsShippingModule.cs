using Autofac;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

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

            builder.RegisterType<UpsAccountRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsSettingsRepository>()
                .AsSelf()
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsPromoWebClientFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UpsPromoPolicy>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UpsPromoFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsOltShipmentType>()
                .As<UpsShipmentType>();
        }
    }
}