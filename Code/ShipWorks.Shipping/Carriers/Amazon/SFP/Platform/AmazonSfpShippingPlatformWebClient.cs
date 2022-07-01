using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Amazon SFP Shipping web client using Platform
    /// </summary>
    [Component]
    public class AmazonSfpShippingPlatformWebClient : IAmazonSfpShippingPlatformWebClient
    {
        private readonly ILogEntryFactory createApiLogEntry;
        private readonly IAmazonSFPServiceTypeRepository amazonSFPServiceTypeRepository;
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactoryIndex;
        private readonly IAmazonSfpRateGroupFactory rateGroupFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpShippingPlatformWebClient(ILogEntryFactory createApiLogEntry, 
            IAmazonSFPServiceTypeRepository amazonSFPServiceTypeRepository,
            IWarehouseRequestFactory warehouseRequestFactory,
            IWarehouseRequestClient warehouseRequestClient,
            IAmazonSfpRateGroupFactory rateGroupFactory,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactoryIndex)
        {
            this.createApiLogEntry = createApiLogEntry;
            this.amazonSFPServiceTypeRepository = amazonSFPServiceTypeRepository;
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
            this.rateGroupFactory = rateGroupFactory;
            this.rateRequestFactoryIndex = rateRequestFactoryIndex;
        }

        /// <summary>
        /// Get rates via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                var rateRequestFactory = rateRequestFactoryIndex[ShipmentTypeCode.AmazonSFP];

                IEnumerable<string> availableServiceTypeApiCodes = amazonSFPServiceTypeRepository.Get()
                    .Select(s => s.PlatformApiCode)
                    .ToList();

                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateResponse rates = null;
                
                var ratesTask = Task
                    .Run(async () => rates = await GetRates(request).ConfigureAwait(false))
                    .RethrowException();

                Task.WaitAll(ratesTask);

                if (rates.Errors?.Any() == true)
                {
                    var errorsMsg = string.Join(Environment.NewLine, rates.Errors.Select(e => e.Message));
                    throw new AmazonSFPShippingException(errorsMsg);
                }

                if (rates.Rates?.Any() == true)
                {
                    return rateGroupFactory.Create(rates, ShipmentTypeCode.AmazonSFP, availableServiceTypeApiCodes);
                }
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex.GetBaseException().Message );
            }

            throw new ShippingException("No rates available.");
        }

        /// <summary>
        /// Call Hub to get rates for a carrier
        /// </summary>
        /// <returns></returns>
        public async Task<RateResponse> GetRates(object obj)
        {
            var request = warehouseRequestFactory.Create(
                WarehouseEndpoints.PlatformPassthrough,
                Method.POST,
                obj);

            request.AddHeader("PlatformEndpoint", "v1/rates");
            request.AddHeader("PlatformMethod", "POST");

            var rateShipmentResponse = await warehouseRequestClient.MakeRequest<RateShipmentResponse>(request, "GetRates")
                .ConfigureAwait(false);

            return rateShipmentResponse.RateResponse;
        }
    }
}
