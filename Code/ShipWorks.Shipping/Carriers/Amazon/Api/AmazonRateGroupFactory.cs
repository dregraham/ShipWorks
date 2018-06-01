﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    public class AmazonRateGroupFactory : IAmazonRateGroupFactory
    {
        private readonly IMessenger messenger;
        private readonly IEnumerable<IAmazonRateGroupFilter> rateFilters;
        private readonly IAmazonServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Creates an Amazon RateGroup from an GetEligibleShippingServicesResponse
        /// </summary>
        /// <remarks>uses rateFilters to filter out rates</remarks>
        public AmazonRateGroupFactory(IMessenger messenger, IEnumerable<IAmazonRateGroupFilter> rateFilters, IAmazonServiceTypeRepository serviceTypeRepository)
        {
            this.messenger = messenger;
            this.rateFilters = rateFilters;
            this.serviceTypeRepository = serviceTypeRepository;
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
                    AmazonServiceTypeEntity serviceType = serviceTypeRepository.Get().Single(s => s.ApiValue == shippingService.ShippingServiceId);
                    
                    AmazonRateTag tag = new AmazonRateTag()
                    {
                        Description = serviceType.Description ?? shippingService.ShippingServiceName ?? "Unknown",
                        ShippingServiceId = shippingService.ShippingServiceId,
                        CarrierName = shippingService.CarrierName,
                        ServiceTypeID = serviceType.AmazonServiceTypeID
                    };                    

                    RateResult rateResult = new RateResult(tag.Description, "", shippingService.Rate.Amount, tag);
                    rateResult.ShipmentType = ShipmentTypeCode.Amazon;
                    rateResult.ProviderLogo = GetProviderLogo(tag.CarrierName ?? string.Empty);
                    rateResults.Add(rateResult);
                }

                rateGroup = new RateGroup(rateResults.OrderBy(r => r.Amount));
            }

            // Add terms and conditions footnote if needed
            List<string> carriers = response.GetEligibleShippingServicesResult?.TermsAndConditionsNotAcceptedCarrierList?.TermsAndConditionsNotAcceptedCarrier?.CarrierName;
            if (carriers != null && carriers.Any())
            {
                List<string> carrierNames = carriers.Distinct().ToList();

                rateGroup.AddFootnoteFactory(new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames));
            }

            RateGroup filteredRateGroup = rateFilters.Aggregate(rateGroup, (rates, filter) => filter.Filter(rates));

            messenger.Send(new AmazonRatesRetrievedMessage(this, filteredRateGroup));

            return filteredRateGroup;
        }

        /// <summary>
        /// Determine which carrier the ShippingService belongs to
        /// Return the logo of that carrier returns Null if we cannot
        /// find a match for the carrier
        /// </summary>
        private static Image GetProviderLogo(string carrier)
        {
            switch (carrier.ToLowerInvariant())
            {
                case "ups":
                    return EnumHelper.GetImage(ShipmentTypeCode.UpsOnLineTools);
                case "fedex":
                    return EnumHelper.GetImage(ShipmentTypeCode.FedEx);
                case "usps":
                case "stamps_dot_com":
                    return EnumHelper.GetImage(ShipmentTypeCode.Usps);
                case "ontrac":
                    return EnumHelper.GetImage(ShipmentTypeCode.OnTrac);
                default:
                    return EnumHelper.GetImage(ShipmentTypeCode.None);
            }
        }
    }
}