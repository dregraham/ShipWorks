using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label service for FedEx
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.FedEx)]
    public class FedExLabelService : ShipEngineLabelService
    {
        private readonly IFedExAccountRepository accountRepository;
        private readonly IFedExShippingClerkFactory shippingClerkFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IFedExAccountRepository accountRepository,
            IFedExShippingClerkFactory shippingClerkFactory,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, FedExDownloadedLabelData> createDownloadedLabelData,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(FedExLabelService));
            this.accountRepository = accountRepository;
            this.shippingClerkFactory = shippingClerkFactory;
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

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void Void(ShipmentEntity shipment)
        {
            // If this isn't a ShipEngine shipment, void the old way
            if (!shipment.FedEx.ShipEngineLabelId.HasValue())
            {
                var shippingClerk = shippingClerkFactory.Create(shipment);

                try
                {
                    shippingClerk.Void(shipment);

                    return;
                }
                catch (FedExException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }
            }

            base.Void(shipment);
        }
    }
}
