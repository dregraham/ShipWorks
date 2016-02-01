using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    public class AmazonRateGroupFactory : IAmazonRateGroupFactory
    {
        private readonly IMessenger messenger;
        private readonly IEnumerable<IAmazonRateGroupFilter> rateFilters;

        /// <summary>
        /// Creates an Amazon RateGroup from an GetEligibleShippingServicesResponse
        /// </summary>
        /// <remarks>uses rateFilters to filter out rates</remarks>
        public AmazonRateGroupFactory(IMessenger messenger, IEnumerable<IAmazonRateGroupFilter> rateFilters)
        {
            this.messenger = messenger;
            this.rateFilters = rateFilters;
        }

        /// <summary>
        /// Gets the rate group from response.
        /// </summary>
        public RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response)
        {
            List<RateResult> rateResults = new List<RateResult>();
            RateGroup rateGroup;

            ShippingServiceList serviceList = response.GetEligibleShippingServicesResult.ShippingServiceList;

            if (serviceList.ShippingService.None() || serviceList.ShippingService.All(x => x.Rate == null))
            {
                // Return an empty list of rates so that the rate control can display the appropriate text.
                rateGroup = new RateGroup(rateResults);
            }
            else
            {
                foreach (ShippingService shippingService in serviceList.ShippingService.Where(x => x.Rate != null))
                {
                    AmazonRateTag tag = new AmazonRateTag()
                    {
                        Description = shippingService.ShippingServiceName ?? "Unknown",
                        ShippingServiceId = shippingService.ShippingServiceId,
                        ShippingServiceOfferId = shippingService.ShippingServiceOfferId,
                        CarrierName = shippingService.CarrierName
                    };

                    RateResult rateResult = new RateResult(shippingService.ShippingServiceName ?? "Unknown", "", shippingService.Rate.Amount, tag);
                    rateResult.ProviderLogo = GetProviderLogo(shippingService.CarrierName ?? string.Empty);
                    rateResults.Add(rateResult);
                }

                rateGroup = new RateGroup(rateResults);
            }

            // Add terms and conditions footnote if needed
            List<string> carriers = response.GetEligibleShippingServicesResult?.TermsAndConditionsNotAcceptedCarrierList?.TermsAndConditionsNotAcceptedCarrier?.CarrierName;
            if (carriers != null && carriers.Any())
            {
                List<string> carrierNames = carriers.Distinct().ToList();

                rateGroup.AddFootnoteFactory(new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames));
            }

            messenger.Send(new AmazonRatesRetrievedMessage(this, rateGroup));

            return rateFilters.Aggregate(rateGroup, (rates, filter) => filter.Filter(rates));
        }

        /// <summary>
        /// Determine which carrier the ShippingService belongs to
        /// Return the logo of that carrier returns Null if we cannot
        /// find a match for the carrier
        /// </summary>
        private static Image GetProviderLogo(string carrier)
        {
            switch (carrier.ToLower())
            {
                case "ups":
                    return EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools);
                case "fedex":
                    return EnumHelper.GetImage(ShipmentTypeCode.FedEx);
                case "usps":
                case "stamps_dot_com":
                    return EnumHelper.GetImage(ShipmentTypeCode.Usps);
                default:
                    return EnumHelper.GetImage(ShipmentTypeCode.None);
            }
        }
    }
}