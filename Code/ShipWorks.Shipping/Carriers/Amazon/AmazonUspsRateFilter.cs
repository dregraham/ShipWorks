using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public class AmazonUspsRateFilter : IAmazonRateGroupFilter
    {
        /// <summary>
        /// Filters the specified rate group.
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            List<RateResult> rates = rateGroup.Rates;
            if (rates.None(r => ((AmazonRateTag)r.Tag).CarrierName.IndexOf("usps", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                return rateGroup;
            }

            RateGroup newRateGroup = rateGroup.CopyWithRates(rates.Where(r => ((AmazonRateTag)r.Tag).CarrierName.IndexOf("usps", StringComparison.OrdinalIgnoreCase) == -1));
            newRateGroup.AddFootnoteFactory(new AmazonNotLinkedFootnoteFactory(ShipmentTypeCode.Usps));

            return newRateGroup;
        }
    }
}