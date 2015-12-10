using Autofac;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Shipping module for the BestRate carrier
    /// </summary>
    public class BestRateShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BestRateLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.BestRate);

            builder.RegisterType<BestRateShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.BestRate);

            builder.RegisterType<BestRateRatingService>()
                .As<IBestRateBrokerRatingService>()
                .AsSelf()
                .Keyed<IRatingService>(ShipmentTypeCode.BestRate);

            builder.RegisterType<BestRateShippingBrokerFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<BestRateRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.BestRate)
                .AsSelf();

            builder.RegisterType<BestRateFilterFactory>()
                .As<IRateGroupFilterFactory>()
                .AsSelf();

        }
    }
}
