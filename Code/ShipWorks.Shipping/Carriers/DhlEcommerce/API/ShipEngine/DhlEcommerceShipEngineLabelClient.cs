using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.API.ShipEngine
{
    /// <summary>
    /// Dhl Ecommerce ShipEngine Label Client
    /// </summary>
    [Component]
    public class DhlEcommerceShipEngineLabelClient : ShipEngineLabelService, IDhlEcommerceLabelClient
    {
        private readonly IDhlEcommerceAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipEngineLabelClient(
            IShipEngineWebClient shipEngineWebClient,
            IDhlEcommerceAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlEcommerceShipEngineDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(DhlEcommerceLabelService));
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.DhlEcommerce;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.DhlEcommerce;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierId ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.DhlEcommerce.ShipEngineLabelID;
    }
}
