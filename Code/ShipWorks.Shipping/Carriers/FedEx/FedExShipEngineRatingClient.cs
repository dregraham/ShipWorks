using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Rating service for FedEx
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.FedEx)]
    public class FedExShipEngineRatingClient : IRatingService
    {
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipEngineRatingClient(
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory,
            IShipEngineWebClient shipEngineWebClient,
            IShipEngineRateGroupFactory rateGroupFactory,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.FedEx];
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.FedEx);
        }

        /// <summary>
        /// Get rates from FedEx via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {

            // We don't have any DHL eCommerce accounts, so let the user know they need an account.
            if (!FedExAccountManager.AccountsReadOnly.Any())
            {
                throw new ShippingException("An account is required to view FedEx rates.");
            }

            try
            {
                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                        await shipEngineWebClient.RateShipment(request, ApiLogSource.FedEx).ConfigureAwait(false)).Result;

                IEnumerable<string> availableServiceTypeApiCodes = shipmentType.GetAvailableServiceTypes()
                    .Cast<FedExServiceType>()
                    .Select(t => EnumHelper.GetApiValue(t));

                return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.FedEx, availableServiceTypeApiCodes);
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }
    }
}
