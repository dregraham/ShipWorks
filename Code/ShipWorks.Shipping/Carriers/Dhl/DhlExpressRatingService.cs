using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express rating service
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressRatingService : IRatingService
    {
        private readonly ICarrierShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressRatingService(
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
            try
            {
                // We don't have any DHL Express accounts, so let the user know they need an account.
                if (!accountRepository.Accounts.Any())
                {
                    throw new DhlExpressException($"An account is required to view DHL Express rates.");
                }

                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateShipmentResponse rateResponse = Task.Run(async () => await shipEngineWebClient.RateShipment(request, ApiLogSource.DHLExpress).ConfigureAwait(false)).Result;

                return rateGroupFactory.Create(rateResponse, ShipmentTypeCode.DhlExpress,
                     shipmentType.GetAvailableServiceTypes()
                        .Cast<DhlExpressServiceType>()
                        .Select(t => EnumHelper.GetApiValue(t))
                        .Union(new List<string> { EnumHelper.GetApiValue((DhlExpressServiceType) shipment.DhlExpress.Service) }));
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }
    }
}
