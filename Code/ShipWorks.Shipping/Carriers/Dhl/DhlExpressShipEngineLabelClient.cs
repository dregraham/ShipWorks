using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Autofac.Features.Indexed;
using log4net;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express ShipEngine Label Client
    /// </summary>
    public class DhlExpressShipEngineLabelClient : ShipEngineLabelService, IDhlExpressLabelClient
    {
        private readonly IDhlExpressAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipEngineLabelClient(
            IShipEngineWebClient shipEngineWebClient,
            IDhlExpressAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlExpressShipEngineDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
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
