using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering
{
    /// <summary>
    /// An IRateGroupFilter implementation that will remove any footnotes from a rate group
    /// where the footnote's shipment type does not match the shipment type for any rates in the
    /// rate group.
    /// </summary>
    public class BestRateNonExistentShipmentTypeFootnoteFilter : IRateGroupFilter
    {
        /// <summary>
        /// Removes any footnotes from a rate group where the footnote's shipment type does not match 
        /// the shipment type for any rates in the rate group.Method that filters a group of rates and
        /// returns a new group of the filtered rate results.
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            List<RateResult> rates = rateGroup.Rates;

            // Remove the footnote factories that do not have associated rates in the rate group
            // We want to keep the footnote for an invalid store address when getting counter rates though.
            List<IRateFootnoteFactory> footnoteFactories = rateGroup.FootnoteFactories
                                                        .Where(f => f.GetType() == typeof(CounterRatesInvalidStoreAddressFootnoteFactory) 
                                                                    || f.GetType() == typeof(ShippingAccountRequiredForRatingFootnoteFactory)
                                                                    || rates.Select(r => r.ShipmentType).Contains(f.ShipmentTypeCode)).ToList();

            RateGroup filteredRateGroup = new RateGroup(rates);
            footnoteFactories.ForEach(filteredRateGroup.AddFootnoteFactory);

            return filteredRateGroup;
        }
    }
}
