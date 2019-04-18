﻿using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.RateGroupFilters
{
    /// <summary>
    /// Restrict Amazon rates to only Stamps, FedEx, and UPS
    /// </summary>
    public class AmazonSFPAllowedCarriersRateGroupFilter : IAmazonSFPRateGroupFilter
    {
        readonly string[] allowedCarriers = new[] { "STAMPS_DOT_COM", "USPS", "UPS", "FEDEX", "DYNAMEX", "ONTRAC" };

        /// <summary>
        /// Filter the rate groups
        /// </summary>
        public RateGroup Filter(RateGroup rateGroup)
        {
            RateGroup filteredRateGroup = new RateGroup(rateGroup.Rates.Where(IsAllowedCarrier))
            {
                Carrier = rateGroup.Carrier,
                OutOfDate = rateGroup.OutOfDate
            };

            foreach (IRateFootnoteFactory factory in rateGroup.FootnoteFactories.Select(FilterCarrierNames).Where(x => x != null))
            {
                filteredRateGroup.AddFootnoteFactory(factory);
            }

            return filteredRateGroup;
        }


        /// <summary>
        /// Filter carriers from any T&C footnote factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3215:\"interface\" instances should not be cast to concrete types", Justification = "CarrierNames is specific only to Amazon.")]
        private IRateFootnoteFactory FilterCarrierNames(IRateFootnoteFactory factory)
        {
            AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory termsFactory = factory as AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory;
            if (termsFactory == null)
            {
                return factory;
            }

            AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory filteredFactory =
                new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(termsFactory.CarrierNames.Intersect(allowedCarriers, new CaseInsensitiveEqualityComparer()));

            return filteredFactory.CarrierNames.Any() ? filteredFactory : null;
        }

        /// <summary>
        /// Is the rate from an allowed carrier
        /// </summary>
        private bool IsAllowedCarrier(RateResult rateResult)
        {
            AmazonRateTag tag = rateResult.Tag as AmazonRateTag;
            return allowedCarriers.Contains(tag?.CarrierName?.ToUpper());
        }
    }
}
