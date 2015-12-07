using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using System.Drawing;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets the rates from Amazon via the IAmazonShippingWebClient
    /// </summary>
    public class AmazonRatingService : IRatingService
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IOrderManager orderManager;
        private readonly IAmazonShipmentRequestDetailsFactory requestFactory;
        private readonly IEnumerable<IAmazonRateGroupFilter> rateFilters;


        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRatingService"/> class.
        /// </summary>
        public AmazonRatingService(IAmazonShippingWebClient webClient,
            IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory,
            IEnumerable<IAmazonRateGroupFilter> rateFilters)
        {
            this.webClient = webClient;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.rateFilters = rateFilters;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            orderManager.PopulateOrderDetails(shipment);
            IAmazonOrder amazonOrder = shipment.Order as IAmazonOrder;

            if (amazonOrder?.IsPrime == false)
            {
                throw new AmazonShippingException("Not an Amazon Prime Order");
            }

            if (amazonOrder == null)
            {
                throw new AmazonShippingException("Not an Amazon Order");
            }

            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, shipment.Order as IAmazonOrder);

            GetEligibleShippingServicesResponse response = webClient.GetRates(requestDetails);

            RateGroup rateGroup = GetRateGroupFromResponse(response);
            
            Messenger.Current.Send(new AmazonRatesRetrievedMessage(this, rateGroup));

            return rateFilters.Aggregate(rateGroup, (rates, filter) => filter.Filter(rates));
        }

        /// <summary>
        /// Gets the rate group from response.
        /// </summary>
        private RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response)
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
            List<string> carriers = response.GetEligibleShippingServicesResult?.TermsAndConditionsNotAcceptedCarrierList?.TermsAndConditionsNotAcceptedCarrier.CarrierName;
            if (carriers != null && carriers.Any())
            {
                List<string> carrierNames = carriers.Distinct().ToList();

                rateGroup.AddFootnoteFactory(new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames));
            }

            return rateGroup;
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