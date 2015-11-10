using System;
using ShipWorks.Shipping.Editing.Rating;
using System.Linq;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Restrict Amazon rates to only Stamps, FedEx, and UPS
    /// </summary>
    public class AmazonAllowedCarriersRateGroupFilter : IAmazonRateGroupFilter
    {
        readonly string[] allowedCarriers = new[] { "STAMPS_DOT_COM", "UPS", "FEDEX" };

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
        private IRateFootnoteFactory FilterCarrierNames(IRateFootnoteFactory factory)
        {
            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory termsFactory = factory as AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory;
            if (termsFactory == null)
            {
                return factory;
            }

            AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory filteredFactory =
                new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(termsFactory.ShipmentTypeCode, termsFactory.CarrierNames.Intersect(allowedCarriers));

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
