using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Service for retreiving UPS rates via ShipEngine
    /// </summary>
    [Component]
    class UpsShipEngineRatingService : IUpsShipEngineRatingService
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;
        private readonly IUpsShipmentValidatorFactory upsShipmentValidatorFactory;
        private readonly UpsAccountRepository accountRepository;
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipEngineRatingService(IShipEngineWebClient shipEngineWebClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipmentTypeManager shipmentTypeManager,
            IShipEngineRateGroupFactory rateGroupFactory,
            IUpsShipmentValidatorFactory upsShipmentValidatorFactory)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.upsShipmentValidatorFactory = upsShipmentValidatorFactory;
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.UpsOnLineTools];

            accountRepository = new UpsAccountRepository();
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.UpsOnLineTools);
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any ShipEngine UPS accounts, so let the user know they need an account.
            if (accountRepository.Accounts.All(x => x.ShipEngineCarrierId == null))
            {
                throw new ShippingException("A UPS from ShipWorks account is required to view UPS rates.");
            }

            upsShipmentValidatorFactory.Create(shipment).ValidateShipment(shipment);

            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.UPS).ConfigureAwait(false)).Result;

                IEnumerable<string> availableServiceTypeApiCodes = shipmentType.GetAvailableServiceTypes()
                    .Cast<UpsServiceType>()
                    .Where(s => UpsShipEngineServiceTypeUtility.IsServiceSupported(s))
                    .Select(t => UpsShipEngineServiceTypeUtility.GetServiceCode(t));

                return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.UpsOnLineTools, availableServiceTypeApiCodes);
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }
    }
}
