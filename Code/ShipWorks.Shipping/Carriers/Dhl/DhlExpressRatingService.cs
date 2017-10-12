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

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express rating service
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressRatingService : IRatingService
    {
        private readonly ICarrierRateShipmentRequestFactory rateRequestFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineRateGroupFactory rateGroupFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressRatingService(
            IIndex<ShipmentTypeCode, ICarrierRateShipmentRequestFactory> rateRequestFactory, 
            IShipEngineWebClient shipEngineWebClient, 
            IShipEngineRateGroupFactory rateGroupFactory)
        {
            this.rateRequestFactory = rateRequestFactory[ShipmentTypeCode.DhlExpress];
            this.shipEngineWebClient = shipEngineWebClient;
            this.rateGroupFactory = rateGroupFactory;
        }

        /// <summary>
        /// Get rates from DHL Express via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                RateShipmentRequest request = rateRequestFactory.Create(shipment);
                RateShipmentResponse rateResponse = Task.Run(async () => {
                    return await shipEngineWebClient.RateShipment(request, ApiLogSource.DHLExpress).ConfigureAwait(false);
                }).Result;
                                
                return rateGroupFactory.Create(rateResponse, ShipmentTypeCode.DhlExpress);
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex.Message);
            }
        }
    }
}
