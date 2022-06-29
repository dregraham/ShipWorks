using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
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

        ///// <summary>
        ///// Gets rates for the given ShipmentRequestDetails
        ///// </summary>
        //public GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, AmazonSFPShipmentEntity shipment)
        //{
        //    var rates = GetRates(shipment.Shipment);

        //    var result = new GetEligibleShippingServicesResponse()
        //    {
        //        GetEligibleShippingServicesResult = new GetEligibleShippingServicesResult()
        //        {
        //            ShippingServiceList = new ShippingServiceList()
        //            {
        //                ShippingService = new List<ShippingService>()
        //            }

        //        }
        //    };

        //    foreach (var rate in rates.Rates)
        //    {
        //        var ss = new ShippingService()
        //        {
        //            CarrierName = rate.CarrierDescription,
        //            EarliestEstimatedDeliveryDate = "",
        //            LatestEstimatedDeliveryDate = "",
        //            ShippingServiceName = rate.OriginalTag.ToString(),
        //            ShippingServiceId = rate.OriginalTag.ToString(),
        //            Rate = new Rate()
        //            {
        //                Amount = rate.AmountOrDefault,
        //                CurrencyCode = "USD"
        //            }

        //        };
        //        result.GetEligibleShippingServicesResult.ShippingServiceList.ShippingService.Add(ss);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Get rates via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
                var rateRequestFactory = rateRequestFactoryIndex[ShipmentTypeCode.AmazonSFP];
                
                IEnumerable<string> availableServiceTypeApiCodes = amazonSFPServiceTypeRepository.Get()
                    .Select(s => s.PlatformApiCode)
                    .ToList();

                RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                RateResponse rates = null;

                var ratesTask = Task.Run(async () => rates = await GetRates(request).ConfigureAwait(false));
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
                
                throw new ShippingException("No rates available.");

                //IShipEngineWebClient shipEngineWebClient = lifetimeScope.Resolve<IShipEngineWebClient>();
                //IShipEngineRateGroupFactory rateGroupFactory = lifetimeScope.Resolve<IShipEngineRateGroupFactory>();
                //IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> rateRequestFactoryIndex = lifetimeScope.Resolve<IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory>>();
                //IShipmentTypeManager shipmentTypeManager = lifetimeScope.Resolve<IShipmentTypeManager>();

                //var rateRequestFactory = rateRequestFactoryIndex[ShipmentTypeCode.AmazonSFP];
                //var shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.AmazonSFP);

                //// We don't have any Amazon Buy Shipping accounts, so let the user know they need an account.
                //if (!accountRepository.AccountsReadOnly.Any())
                //{
                //    throw new ShippingException("An account is required to view Amazon Buy Shipping rates.");
                //}

                //try
                //{
                //    RateShipmentRequest request = rateRequestFactory.CreateRateShipmentRequest(shipment);
                //    RateShipmentResponse rateShipmentResponse = Task.Run(async () =>
                //        await shipEngineWebClient.RateShipment(request, ApiLogSource.Amazon).ConfigureAwait(false)).Result;

                //    IEnumerable<string> availableServiceTypeApiCodes = shipmentType.GetAvailableServiceTypes()
                //        //.Cast<DhlEcommerceServiceType>()
                //        //.Select(t => EnumHelper.GetApiValue(t));
                //        .Select(i => i.ToString());

                //    return rateGroupFactory.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.AmazonSFP, availableServiceTypeApiCodes);
                //}
                //catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
                //{
                //    throw new ShippingException(ex.GetBaseException().Message);
                //}
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

            request.AddHeader("PlatformEndpoint", "shipengine/v1/rates");
            request.AddHeader("PlatformMethod", "POST");

            var rateShipmentResponse = await warehouseRequestClient.MakeRequest<RateShipmentResponse>(request, "GetRates")
                .ConfigureAwait(false);

            return rateShipmentResponse.RateResponse;
        }
    }
}
