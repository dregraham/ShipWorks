using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Dhl.API.ShipEngine
{
    /// <summary>
    /// Dhl Express ShipEngine Label Client
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DhlExpressShipEngineLabelClient : ShipEngineLabelService, IDhlExpressLabelClient
    {
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public DhlExpressShipEngineLabelClient(
            IShipEngineWebClient shipEngineWebClient,
            IDhlExpressAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlExpressShipEngineDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory,
            IShipEngineTrackingResultFactory trackingResultFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(DhlExpressLabelService));
            this.accountRepository = accountRepository;
            this.trackingResultFactory = trackingResultFactory;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public override Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            if (shipment?.DhlExpress?.Service == (int) DhlExpressServiceType.ExpressWorldWideDocuments)
            {
                throw new ShippingException($"{EnumHelper.GetDescription(DhlExpressServiceType.ExpressWorldWideDocuments)} is not supported by this account.");
            }

            return base.Create(shipment);
        }

        /// <summary>
        /// Get the tracking result for the given shipment
        /// </summary>
        public TrackingResult Track(ShipmentEntity shipment)
        {
            TrackingInformation trackingInfo = Task.Run(() =>
            {
                return shipEngineWebClient.Track(shipment.DhlExpress.ShipEngineLabelID, ApiLogSource.DHLExpress);
            }).Result;

            return trackingResultFactory.Create(trackingInfo);
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
