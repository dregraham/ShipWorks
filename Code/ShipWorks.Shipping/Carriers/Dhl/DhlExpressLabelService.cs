using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Implementation
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressLabelService : ShipEngineLabelService
    {
        private readonly IDhlExpressAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IDhlExpressAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory) 
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData)
        {
            log = logFactory(typeof(DhlExpressLabelService));
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.DHLExpress;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.DhlExpress;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierId ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.DhlExpress.ShipEngineLabelID;        
    }
}
