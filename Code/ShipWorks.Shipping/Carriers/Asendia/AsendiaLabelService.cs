﻿using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipEngine.CarrierApi.Client.Model;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Label service for Asendia
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Asendia)]
    public class AsendiaLabelService : ShipEngineLabelService
    {
        private readonly IAsendiaAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IAsendiaAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, AsendiaDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory) 
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(AsendiaLabelService));
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.Asendia;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Asendia;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierId ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.Asendia.ShipEngineLabelID;
    }
}
