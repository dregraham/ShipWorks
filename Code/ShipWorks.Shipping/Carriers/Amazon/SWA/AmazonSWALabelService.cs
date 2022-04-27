using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
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
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
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
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierId ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.AmazonSWA.ShipEngineLabelID;
    }
}
