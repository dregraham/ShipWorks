using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

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

            builder.RegisterType<BrokerExceptionsRateFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<CounterRatesInvalidStoreAddressFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.BestRate)
                .SingleInstance();

            builder.RegisterType<BestRateShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.BestRate)
                .ExternallyOwned();

            builder.RegisterType<NullShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.BestRate)
                .SingleInstance();

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

            builder.RegisterType<ShippingAccountRequiredForRatingFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();
        }

        /// <summary>
        /// Creates a BestRateBrokerFactory based on the BestRateConsolidatePostalRates enum
        /// </summary>
        private IBestRateShippingBrokerFactory GenerateBestRateBrokerFactory(IComponentContext c, IEnumerable<Parameter> p)
        {
            Parameter[] parameters = p.ToArray();
            if (parameters.FirstOrDefault() == null || parameters.TypedAs<BestRateConsolidatePostalRates>() == BestRateConsolidatePostalRates.No)
            {
                // return BestRateShippingBrokerFactory with default behavior
                return new BestRateShippingBrokerFactory(new List<IShippingBrokerFilter>
                {
                    new UpsWorldShipBrokerFilter(),
                    new PostalCounterBrokerFilter(),
                    new UpsBestRateRestrictionBrokerFilter()
                });
            }

            // return BestRateShippingBrokerFactory with filters to consolidate postal rates
            // used in the rating panel
            return new BestRateShippingBrokerFactory(new List<IShippingBrokerFilter>
            {
                new PostalCounterBrokerFilter(),
                new PostalOnlyBrokerFilter()
            });
        }
    }
}
