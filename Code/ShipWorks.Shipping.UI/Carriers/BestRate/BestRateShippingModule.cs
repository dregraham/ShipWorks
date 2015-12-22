using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;

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
                .Keyed<IRatingService>(ShipmentTypeCode.BestRate);
            
            builder.Register(GenerateBestRateBrokerFactory);

            builder.RegisterType<BestRateRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.BestRate)
                .AsSelf();

            builder.RegisterType<BestRateFilterFactory>()
                .As<IRateGroupFilterFactory>()
                .AsSelf();
        }

        /// <summary>
        /// Creates a BestRateBrokerFactory based on the BestRateConsolidatePostalRates enum
        /// </summary>
        private IBestRateShippingBrokerFactory GenerateBestRateBrokerFactory(IComponentContext c, IEnumerable<Parameter> p)
        {
            // return BestRateShippingBrokerFactory with default behavior
            return new BestRateShippingBrokerFactory(new List<IShippingBrokerFilter>
            {
                new UpsWorldShipBrokerFilter(),
                new PostalCounterBrokerFilter(),
                new UpsBestRateRestrictionBrokerFilter()
            });
        }
    }
}
