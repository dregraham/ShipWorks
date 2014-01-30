using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// An implementation of the IRateGroupFilterFactory interface that will create all of the
    /// filters that are needed for filtering a best-rate rate group.
    /// </summary>
    public class BestRateFilterFactory : IRateGroupFilterFactory
    {
        /// <summary>
        /// Creates the filters that will need to be applied to a rate group.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A collection of IRateGroupFilter objects.</returns>
        public IEnumerable<IRateGroupFilter> CreateFilters(ShipmentEntity shipment)
        {
            // Order is important regarding the footnote filters - promo footnote filter should
            // be after the non-existent shipment type footnote filter
            return new List<IRateGroupFilter>
            {
                new RateGroupFilter((ServiceLevelType)shipment.BestRate.ServiceLevel),
                new BestRateNonExistentShipmentTypeFootnoteFilter(),
                new BestRateExpress1PromotionFootnoteFilter(),
                new CounterRatesInvalidStoreAddressFootnoteFilter()
            };
        }
    }
}
