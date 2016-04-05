using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.UI.Carriers.Ups
{
    /// <summary>
    /// Service registrations for the Ups shipping carrier
    /// </summary>
    public class UpsShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UpsOltShipmentType>();

            builder.RegisterType<UpsAccountRepository>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<UpsAccountRepository>()
                .Keyed<ICarrierAccountRepository<UpsAccountEntity>>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<UpsOltLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsOltShipmentValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsApiTransitTimeClient>();

            builder.RegisterType<UpsApiRateClient>();

            builder.RegisterType<UpsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IRatingService>(ShipmentTypeCode.UpsWorldShip);
        }
    }
}
