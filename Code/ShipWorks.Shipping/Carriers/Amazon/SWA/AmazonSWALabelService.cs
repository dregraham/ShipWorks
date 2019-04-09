using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using System.Linq;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Collections;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Label service for AmazonSWA
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWALabelService : ShipEngineLabelService
    {
        private readonly IAmazonSWAAccountRepository accountRepository;
        private readonly IRatesRetriever ratesRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWALabelService(
            IShipEngineWebClient shipEngineWebClient,
            IAmazonSWAAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, AmazonSWADownloadedLabelData> createDownloadedLabelData,
            IRatesRetriever ratesRetriever,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData)
        {
            log = logFactory(typeof(AmazonSWALabelService));
            this.accountRepository = accountRepository;
            this.ratesRetriever = ratesRetriever;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.AmazonSWA;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSWA;

        /// <summary>
        /// Create the label
        /// </summary>
        public async override Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            // Amazon does not have a concept of service types, instead they offer different services based on how
            // many days it takes to deliver
            // we have to get rates to get a list of rateIds and then request a label based on the rateid
            // if no rate id is sent they will use the fasted rate
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            // to get a specific service we first have to get rates
            GenericResult<RateGroup> rates = ratesRetriever.GetRates(shipment);
            if (rates.Success && rates.Value.Rates.Any())
            {
                // Select a specific rate
                RateResult rateToUse = rates.Value.Rates.FirstOrDefault(r => r.Days == EnumHelper.GetApiValue((AmazonSWAServiceType) shipment.AmazonSWA.Service));
                if (rateToUse != null)
                {
                    return await CreateLabelInternal(shipment,
                        () => shipEngineWebClient.PurchaseLabelWithRate(rateToUse.Tag.ToString(),
                        shipmentRequestFactory.CreatePurchaseLabelWithoutShipmentRequest(shipment), ApiLogSource));
                }
            }

            throw new ShippingException("The selected service is not available for this shipment.");
        }

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierId ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.AmazonSWA.ShipEngineLabelID;
    }
}
