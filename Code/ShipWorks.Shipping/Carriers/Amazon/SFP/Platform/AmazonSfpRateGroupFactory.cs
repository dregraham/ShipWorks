using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.RateGroupFilters;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using Rate = ShipWorks.Shipping.ShipEngine.DTOs.Rate;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Factory for creating Amazon Buy Shipping rate groups
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IAmazonSfpRateGroupFactory))]
    public class AmazonSfpRateGroupFactory : ShipEngineRateGroupFactory, IAmazonSfpRateGroupFactory
    {
        private readonly IMessenger messenger;
        private readonly IEnumerable<IAmazonSFPRateGroupFilter> rateFilters;
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Creates an Amazon RateGroup from an GetEligibleShippingServicesResponse
        /// </summary>
        /// <remarks>uses rateFilters to filter out rates</remarks>
        public AmazonSfpRateGroupFactory(IMessenger messenger, IEnumerable<IAmazonSFPRateGroupFilter> rateFilters,
            IAmazonSFPServiceTypeRepository serviceTypeRepository)
        {
            this.messenger = messenger;
            this.rateFilters = rateFilters;
            this.serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Creates a RateGroup from the given RateResponse
        /// </summary>
        public override RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType,
            IEnumerable<string> availableServiceTypeApiCodes)
        {
            var rateGroup = base.Create(rateResponse, shipmentType, availableServiceTypeApiCodes);

            RateGroup filteredRateGroup = rateFilters.Aggregate(rateGroup, (rates, filter) => filter.Filter(rates));

            messenger.Send(new AmazonSFPRatesRetrievedMessage(this, filteredRateGroup));

            return filteredRateGroup;
        }

        /// <summary>
        /// Build the rate result with the given rate
        /// </summary>
        protected override RateResult GetRateResult(Rate apiRate, ShipmentTypeCode shipmentType)
        {
            apiRate.DeliveryDays = null;
            apiRate.CarrierDeliveryDays = string.Empty;
            var rateResult = base.GetRateResult(apiRate, shipmentType);

            var serviceType = serviceTypeRepository.Find(apiRate.ServiceCode);

            AmazonRateTag tag = new AmazonRateTag()
            {
                Description = apiRate.ServiceType ?? "Unknown", //  serviceType.Description ?? shippingService.ShippingServiceName ?? "Unknown",
                ShippingServiceId = serviceType.ApiValue, // shippingService.ShippingServiceId,
                CarrierName = serviceTypeRepository.GetCarrierName(apiRate.ServiceCode),
                ServiceTypeID = serviceType.AmazonSFPServiceTypeID
            };

            rateResult.Tag = tag;
            rateResult.ProviderLogo = GetProviderLogo(tag.CarrierName ?? string.Empty);

            return rateResult;
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

        ///// <summary>
        ///// Gets the rate group from response.
        ///// </summary>
        //public RateGroup GetRateGroupFromResponse(GetEligibleShippingServicesResponse response)
        //{
        //    List<RateResult> rateResults = new List<RateResult>();
        //    RateGroup rateGroup;

        //    ShippingServiceList serviceList = response.GetEligibleShippingServicesResult.ShippingServiceList;

        //    if (serviceList.ShippingService.None() || serviceList.ShippingService.All(x => x.Rate == null))
        //    {
        //        // Return an empty list of rates so that the rate control can display the appropriate text.
        //        rateGroup = new RateGroup(rateResults);
        //    }
        //    else
        //    {
        //        foreach (ShippingService shippingService in serviceList.ShippingService.Where(x => x.Rate != null))
        //        {
        //            AmazonSFPServiceTypeEntity serviceType = serviceTypeRepository.Find(shippingService.ShippingServiceId);

        //            AmazonRateTag tag = new AmazonRateTag()
        //            {
        //                Description = serviceType.Description ?? shippingService.ShippingServiceName ?? "Unknown",
        //                ShippingServiceId = shippingService.ShippingServiceId,
        //                CarrierName = serviceTypeRepository.GetCarrierName(serviceType), 
        //                ServiceTypeID = serviceType.AmazonSFPServiceTypeID
        //            };

        //            RateResult rateResult = new RateResult(tag.Description, "", shippingService.Rate.Amount, tag);
        //            rateResult.ShipmentType = ShipmentTypeCode.AmazonSFP;
        //            rateResult.ProviderLogo = GetProviderLogo(tag.CarrierName ?? string.Empty);
        //            rateResults.Add(rateResult);
        //        }

        //        rateGroup = new RateGroup(rateResults.OrderBy(r => r.Amount));
        //    }

        //    // Add terms and conditions footnote if needed
        //    List<string> carriers = response.GetEligibleShippingServicesResult?.TermsAndConditionsNotAcceptedCarrierList?.TermsAndConditionsNotAcceptedCarrier?.CarrierName;
        //    if (carriers != null && carriers.Any())
        //    {
        //        List<string> carrierNames = carriers.Distinct().ToList();

        //        rateGroup.AddFootnoteFactory(new AmazonSFPCarrierTermsAndConditionsNotAcceptedFootnoteFactory(carrierNames));
        //    }

        //    RateGroup filteredRateGroup = rateFilters.Aggregate(rateGroup, (rates, filter) => filter.Filter(rates));

        //    messenger.Send(new AmazonSFPRatesRetrievedMessage(this, filteredRateGroup));

        //    return filteredRateGroup;
    }
}
