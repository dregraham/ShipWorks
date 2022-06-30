using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Gets the rates from Amazon via the IAmazonShippingWebClient
    /// </summary>
    public class AmazonSFPRatingService : IRatingService
    {
        private readonly IAmazonSfpShippingPlatformWebClient webClient;
        private readonly IOrderManager orderManager;
        private readonly IAmazonSFPShipmentRequestDetailsFactory requestFactory;
        private readonly IAmazonSfpRateGroupFactory amazonRateGroupFactory;
        private readonly IAmazonSFPServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSFPRatingService"/> class.
        /// </summary>
        public AmazonSFPRatingService(IAmazonSfpShippingPlatformWebClient webClient,
            IOrderManager orderManager,
            IAmazonSFPShipmentRequestDetailsFactory requestFactory,
            IAmazonSfpRateGroupFactory amazonRateGroupFactory,
            IAmazonSFPServiceTypeRepository serviceTypeRepository)
        {
            this.webClient = webClient;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.amazonRateGroupFactory = amazonRateGroupFactory;
            this.serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            orderManager.PopulateOrderDetails(shipment);
            IAmazonOrder amazonOrder = shipment.Order as IAmazonOrder;

            if (amazonOrder == null)
            {
                throw new AmazonSFPShippingException("Not an Amazon Order");
            }

            RateGroup rateGroup = webClient.GetRates(shipment);

            // As part of the Amazon SP migration, we are NOT saving new unknown types.
            // SaveUnknownServiceTypes(response);
            
            return rateGroup;
        }

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        public bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter)
        {
            throw new NotImplementedException("Amazon is not yet supported");
        }

        ///// <summary>
        ///// Make the actual rate request
        ///// </summary>
        //private RateGroup GetRates(AmazonSFPShipmentEntity amazonShipment, ShipmentRequestDetails requestDetails)
        //{
        //    var response = webClient.GetRates(requestDetails, amazonShipment);

        //    // As part of the Amazon SP migration, we are NOT saving new unknown types.
        //    // SaveUnknownServiceTypes(response);

        //    return amazonRateGroupFactory.GetRateGroupFromResponse(response);
        //}

        /// <summary>
        /// Saves unknown service type to the ShippingService repository.
        /// </summary>
        private void SaveUnknownServiceTypes(GetEligibleShippingServicesResponse response)
        {
            List<AmazonSFPServiceTypeEntity> knownServiceTypes = serviceTypeRepository.Get();

            List<ShippingService> services = response.GetEligibleShippingServicesResult.ShippingServiceList.ShippingService;
            if (services != null)
            {
                foreach (ShippingService service in services)
                {
                    if (knownServiceTypes.None(s => s.ApiValue == service.ShippingServiceId))
                    {
                        serviceTypeRepository.SaveNewService(service.ShippingServiceId, service.ShippingServiceName);
                    }
                }
            }
        }
    }
}