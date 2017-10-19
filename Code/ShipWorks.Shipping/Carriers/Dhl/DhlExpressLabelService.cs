﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Implmentation
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressLabelService : ILabelService
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly ICarrierShipmentRequestFactory shipmentRequestFactory;
        private readonly Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData> createDownloadedLabelData;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelService(
            IShipEngineWebClient shipEngineWebClient, 
            IDhlExpressAccountRepository accountRepository, 
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.accountRepository = accountRepository;
            this.shipmentRequestFactory = shipmentRequestFactory[ShipmentTypeCode.DhlExpress];
            this.createDownloadedLabelData = createDownloadedLabelData;
            log = logFactory(typeof(DhlExpressLabelService));
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            PurchaseLabelRequest request = shipmentRequestFactory.CreatePurchaseLabelRequest(shipment);

            try
            {
                Label label = Task.Run(() =>
                {
                    return shipEngineWebClient.PurchaseLabel(request, ApiLogSource.DHLExpress);
                }).Result;

                return createDownloadedLabelData(shipment, label);
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            VoidLabelResponse response;
            try
            {
                response = Task.Run(async () =>
                {
                    return await shipEngineWebClient.VoidLabel(shipment.DhlExpress.ShipEngineLabelID, ApiLogSource.DHLExpress).ConfigureAwait(false);
                }).Result;
            }
            catch (Exception ex) when (ex.GetBaseException().GetType() == typeof(ShipEngineException))
            {
                log.Error(ex.GetBaseException());
            }
        }
    }
}
