using System;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Hub;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Amazon Buy Shipping ShipEngine Label Client
    /// </summary>
    [Component]
    public class AmazonSfpShipEngineLabelClient : ShipEngineLabelService, IAmazonSfpLabelClient
    {
        private readonly IHubPlatformShippingClient hubPlatformShippingClient;
        private readonly Func<ShipmentEntity, Label, IDownloadedLabelData> createDownloadedLabelData;
        protected ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSfpShipEngineLabelClient(
            IHubPlatformShippingClient hubPlatformShippingClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactoryIndex,
            Func<ShipmentEntity, Label, AmazonSfpShipEngineDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory) : 
            base(null, shipmentRequestFactoryIndex, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(AmazonSfpShipEngineLabelClient));
            this.createDownloadedLabelData = createDownloadedLabelData;
            this.hubPlatformShippingClient = hubPlatformShippingClient;

            shipmentRequestFactory = shipmentRequestFactoryIndex[ShipmentTypeCode.AmazonSFP];
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public override async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);

            var purchaseLabelRequest = shipmentRequestFactory.CreatePurchaseLabelRequest(shipment);

            try
            {
                var labelObj = await hubPlatformShippingClient.CallViaPassthrough(purchaseLabelRequest, 
                        $"shipengine/{ShipEngineEndpoints.PurchaseLabel}", 
                        HttpMethod.Post)
                    .ConfigureAwait(false);

                var label = (Label) labelObj;

                telemetricResult.SetValue(createDownloadedLabelData(shipment, label));

                return telemetricResult;
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(GetExceptionMessage(ex.GetBaseException(), GetShipEngineCarrierID(shipment)));
            }
            catch (Exception ex)
            {
                throw new ShippingException(GetExceptionMessage(ex.GetBaseException(), GetShipEngineCarrierID(shipment)));
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void Void(ShipmentEntity shipment)
        {
            VoidLabelResponse response;
            try
            {
                response = Task.Run(async () =>
                {
                    return await shipEngineWebClient.VoidLabel(GetShipEngineLabelID(shipment), ApiLogSource).ConfigureAwait(false);
                }).Result;
            }
            catch (Exception ex) when (ex.GetBaseException().GetType() == typeof(ShipEngineException))
            {
                log.Error(ex.GetBaseException());
            }
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.AmazonSfp;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.AmazonSFP;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment)
        {
            AmazonStoreEntity amazonStore = (AmazonStoreEntity) shipment.Order.Store;
            return amazonStore.PlatformAmazonCarrierID ?? string.Empty;
        }

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.AmazonSFP.ShipEngineLabelID;
    }
}
