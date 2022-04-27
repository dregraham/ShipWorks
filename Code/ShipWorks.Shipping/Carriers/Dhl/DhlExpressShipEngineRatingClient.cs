using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using System.Collections.Generic;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express ShipEngine rating client
    /// </summary>
    [Component]
    public class DhlExpressShipEngineRatingClient : IDhlExpressShipEngineRatingClient
    {
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipEngineRatingClient(
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory, 
            IShipEngineWebClient shipEngineWebClient, 
            IShipEngineRateGroupFactory rateGroupFactory,
            IDhlExpressAccountRepository accountRepository, 
            IShipmentTypeManager shipmentTypeManager)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.DhlExpress];
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.accountRepository = accountRepository;
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.DhlExpress);
        }

        /// <summary>
        /// Get rates from DHL Express via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any DHL Express accounts, so let the user know they need an account.
            if (!accountRepository.AccountsReadOnly.Any())
            {
                throw new ShippingException("An account is required to view DHL Express rates.");
            }

            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.DHLExpress).ConfigureAwait(false)).Result;

                IEnumerable<string> availableServiceTypeApiCodes = shipmentType.GetAvailableServiceTypes()
                    .Cast<DhlExpressServiceType>()
                    .Select(t => EnumHelper.GetApiValue(t));

                return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.DhlExpress, availableServiceTypeApiCodes);
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }
    }
}
