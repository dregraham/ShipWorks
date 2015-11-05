using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using System.Drawing;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets the rates from Amazon via the IAmazonShippingWebClient
    /// </summary>
    public class AmazonRates : IAmazonRates
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;
        private readonly IOrderManager orderManager;
        private readonly AmazonShipmentType amazonShipmentType;
        private readonly IAmazonShipmentRequestDetailsFactory requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRates"/> class.
        /// </summary>
        public AmazonRates(IAmazonShippingWebClient webClient, IAmazonMwsWebClientSettingsFactory settingsFactory, IOrderManager orderManager, IAmazonShipmentRequestDetailsFactory requestFactory, AmazonShipmentType amazonShipmentType)
        {
            this.webClient = webClient;
            this.settingsFactory = settingsFactory;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.amazonShipmentType = amazonShipmentType;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            orderManager.PopulateOrderDetails(shipment);
            AmazonOrderEntity order = shipment.Order as AmazonOrderEntity;

            if (order == null)
            {
                throw new AmazonShippingException("Not an Amazon Order");
            }

            if (order.IsPrime != (int)AmazonMwsIsPrime.Yes)
            {
                throw new AmazonShippingException("Not an Amazon Prime Order");
            }

            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, order);

            GetEligibleShippingServicesResponse response = webClient.GetRates(requestDetails, settingsFactory.Create(shipment.Amazon));

            RateGroup rateGroup = GetRateGroupFromResponse(response);

            return rateGroup;
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

                rateGroup.AddFootnoteFactory(new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(amazonShipmentType, carrierNames));
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