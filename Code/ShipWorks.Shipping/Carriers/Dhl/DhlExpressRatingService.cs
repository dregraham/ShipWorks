using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressRatingService(
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactory, 
            IShipEngineWebClient shipEngineWebClient, 
            IShipEngineRateGroupFactory rateGroupFactory,
            IDhlExpressAccountRepository accountRepository)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.DhlExpress];
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
            this.accountRepository = accountRepository;
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
                RateShipmentResponse rateResponse = Task.Run(async () => {
                    return await shipEngineWebClient.RateShipment(request, ApiLogSource.DHLExpress).ConfigureAwait(false);
                }).Result;

                return rateGroupFactory.Create(rateResponse, ShipmentTypeCode.DhlExpress);
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(UnpackExceptionMessage(ex));
            }
        }

        /// <summary>
        /// Find the deepest inner exceptions message
        /// </summary>
        private static string UnpackExceptionMessage(Exception ex)
        {
            string message = ex.Message;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message = ex.Message;
            }

            return message;
        }
    }
}
