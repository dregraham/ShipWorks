using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Empty Rating Service
    /// </summary>
    public class EmptyRatingService : IRatingService
    {
        /// <summary>
        /// GetRates not supported by carrier
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rateGroupWithNoRatesFooter = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroupWithNoRatesFooter.AddFootnoteFactory(new InformationFootnoteFactory("Select another provider to get rates."));
            return rateGroupWithNoRatesFooter;
        }
    }
}