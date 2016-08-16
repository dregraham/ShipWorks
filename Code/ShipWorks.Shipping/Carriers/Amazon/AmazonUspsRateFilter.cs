using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Filter Usps rates for Amazon carrier, if necessary
    /// </summary>
    public class AmazonUspsRateFilter : IAmazonRateGroupFilter
    {
        private readonly Func<ShipmentTypeCode, IAmazonNotLinkedFootnoteFactory> createFootnoteFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createFootnoteFactory"></param>
        public AmazonUspsRateFilter(Func<ShipmentTypeCode, IAmazonNotLinkedFootnoteFactory> createFootnoteFactory)
        {
            this.createFootnoteFactory = createFootnoteFactory;
        }

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
            newRateGroup.AddFootnoteFactory(createFootnoteFactory(ShipmentTypeCode.Usps));

            return newRateGroup;
        }
    }
}