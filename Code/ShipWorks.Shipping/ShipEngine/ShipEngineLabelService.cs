﻿using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Base label service for ShipEngine carriers
    /// </summary>
    public abstract class ShipEngineLabelService : ILabelService
    {
        protected readonly IShipEngineWebClient shipEngineWebClient;
        private readonly Func<ShipmentEntity, DTOs.Label, IDownloadedLabelData> createDownloadedLabelData;
        protected ICarrierShipmentRequestFactory shipmentRequestFactory;
        protected ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShipEngineLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, DTOs.Label, IDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> createLog)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.shipmentRequestFactory = shipmentRequestFactory[ShipmentTypeCode];
            this.createDownloadedLabelData = createDownloadedLabelData;
            log = createLog(typeof(ShipEngineLabelService));
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public abstract ApiLogSource ApiLogSource { get; }

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public abstract ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Create the label
        /// </summary>
        public virtual async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);

            return await CreateLabelInternal(shipment, 
                () => shipEngineWebClient.PurchaseLabel(shipmentRequestFactory.CreatePurchaseLabelRequest(shipment), ApiLogSource, telemetricResult), telemetricResult);
        }
        
        /// <summary>
        /// Create the label
        /// </summary>
        protected async Task<TelemetricResult<IDownloadedLabelData>> CreateLabelInternal(ShipmentEntity shipment, Func<Task<DTOs.Label>> labelRetrievalFunc, 
            TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            try
            {
                var label = await labelRetrievalFunc().ConfigureAwait(false);

                telemetricResult.SetValue(createDownloadedLabelData(shipment, label));

                return telemetricResult;
            }
            catch (Exception ex) when (ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(GetExceptionMessage(ex.GetBaseException(), GetShipEngineCarrierID(shipment)));
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public virtual void Void(ShipmentEntity shipment)
        {
            DTOs.VoidLabelResponse response;
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
        /// Get a user friendly message based on the exception
        /// </summary>
        protected string GetExceptionMessage(Exception ex, string seAccountId)
        {
            string message = ex.Message;
            string carrier = EnumHelper.GetDescription(ShipmentTypeCode);

            if (message.Equals($"A shipping carrier reported an error when processing your request. Carrier ID: {seAccountId}" +
                $", Carrier: {carrier}. One or more errors occurred.", StringComparison.InvariantCultureIgnoreCase))
            {
                return $"There was a problem creating the label while communicating with the {carrier} API";
            }

            if (message.StartsWith($"A shipping carrier reported an error when processing your request. Carrier ID: {seAccountId}" +
                $", Carrier: {carrier}.", StringComparison.InvariantCultureIgnoreCase))
            {
                return message
                    .Replace($"A shipping carrier reported an error when processing your request. Carrier ID: {seAccountId}, Carrier: {carrier}.", string.Empty)
                    .Trim();
            }

            if (message.StartsWith("Unable to create label. Order ID: se-"))
            {
                string newMessage = message.Replace("Unable to create label. Order ID: se-", string.Empty);
                return newMessage.Substring(newMessage.IndexOf('.') + 1).Trim();
            }

            return ex.Message;
        }

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected abstract string GetShipEngineCarrierID(ShipmentEntity shipment);

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected abstract string GetShipEngineLabelID(ShipmentEntity shipment);
    }
}
