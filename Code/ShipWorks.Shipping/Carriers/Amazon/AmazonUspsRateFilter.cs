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
            if (rates.None(IsUspsRate))
            {
                return rateGroup;
            }

            RateGroup newRateGroup = rateGroup.CopyWithRates(rates.Where(r => !IsUspsRate(r)));
            newRateGroup.AddFootnoteFactory(createFootnoteFactory(ShipmentTypeCode.Usps));

            return newRateGroup;
        }

        /// <summary>
        /// Is the rate USPS
        /// </summary>
        private bool IsUspsRate(RateResult rate)
        {
            AmazonRateTag tag = rate.Tag as AmazonRateTag;
            if (tag == null)
            {
                return false;
            }

            return tag.CarrierName.IndexOf("usps", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}