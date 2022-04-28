using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Amazon SWA rating service
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWARatingService : IRatingService
    {
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IAmazonSWARateGroupFactory rateGroupFactory;
        private readonly IAmazonSWAAccountRepository accountRepository;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWARatingService(
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipEngineWebClient shipEngineWebClient,
            IAmazonSWARateGroupFactory rateGroupFactory,
            IAmazonSWAAccountRepository accountRepository,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.AmazonSWA];
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.accountRepository = accountRepository;
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.AmazonSWA);
        }

        /// <summary>
        /// Get rates from Amazon SWA via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any Amazon SWA accounts, so let the user know they need an account.
            if (!accountRepository.Accounts.Any())
            {
                throw new ShippingException("An account is required to view Amazon rates.");
            }

            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);

                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.AmazonSWA).ConfigureAwait(false)).Result;

                return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.AmazonSWA, new[] { "amazon_shipping_standard" });
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }
    }
}
