using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;

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
        private readonly IAmazonRateGroupFactory amazonRateGroupFactory;
        private readonly IAmazonServiceTypeRepository serviceTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRatingService"/> class.
        /// </summary>
        public AmazonRatingService(IAmazonShippingWebClient webClient,
            IOrderManager orderManager,
            IAmazonShipmentRequestDetailsFactory requestFactory,
            IAmazonRateGroupFactory amazonRateGroupFactory,
            IAmazonServiceTypeRepository serviceTypeRepository)
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
                throw new AmazonShippingException("Not an Amazon Order");
            }

            if (amazonOrder?.IsPrime == false)
            {
                throw new AmazonShippingException("Not an Amazon Prime Order");
            }

            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, amazonOrder);
            bool wasSameDay = requestDetails.ShippingServiceOptions.CarrierWillPickUp;

            RateGroup rateGroup = GetRates(shipment.Amazon, requestDetails);

            if (rateGroup.Rates.None() && rateGroup.FootnoteFactories.None() && wasSameDay)
            {
                requestDetails.ShippingServiceOptions.CarrierWillPickUp = false;
                rateGroup = GetRates(shipment.Amazon, requestDetails);
                rateGroup.AddFootnoteFactory(new AmazonSameDayNotAvailableFootnoteFactory());
            }

            return rateGroup;
        }

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        public bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter)
        {
            throw new NotImplementedException("Amazon is not yet supported");
        }

        /// <summary>
        /// Make the actual rate request
        /// </summary>
        private RateGroup GetRates(AmazonShipmentEntity amazonShipment, ShipmentRequestDetails requestDetails)
        {
            GetEligibleShippingServicesResponse response = webClient.GetRates(requestDetails, amazonShipment);

            SaveUnknownServiceTypes(response);

            return amazonRateGroupFactory.GetRateGroupFromResponse(response);
        }

        /// <summary>
        /// Saves unknown service type to the ShippingService repository.
        /// </summary>
        private void SaveUnknownServiceTypes(GetEligibleShippingServicesResponse response)
        {
            List<AmazonServiceTypeEntity> knownServiceTypes = serviceTypeRepository.Get();

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