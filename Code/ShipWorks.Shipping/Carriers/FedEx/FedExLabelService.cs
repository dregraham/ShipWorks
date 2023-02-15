using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine.DTOs;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label service for FedEx
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.FedEx)]
    public class FedExLabelService : ShipEngineLabelService
    {
        private readonly IFedExAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IFedExAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, FedExDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(FedExLabelService));
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.FedEx;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.FedEx;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierID ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.FedEx.ShipEngineLabelId;
    }
}
