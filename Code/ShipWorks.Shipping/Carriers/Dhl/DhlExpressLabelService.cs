using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelService(
            IShipEngineWebClient shipEngineWebClient, 
            IDhlExpressAccountRepository accountRepository, 
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData> createDownloadedLabelData)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.accountRepository = accountRepository;
            this.shipmentRequestFactory = shipmentRequestFactory[ShipmentTypeCode.DhlExpress];
            this.createDownloadedLabelData = createDownloadedLabelData;
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
                Label label = Task.Run(async () =>
                {
                    return await shipEngineWebClient.PurchaseLabel(request, ApiLogSource.DHLExpress).ConfigureAwait(false);
                }).Result;

                return createDownloadedLabelData(shipment, label);
            }
            catch (Exception ex) when(ex.GetType() != typeof(ShippingException))
            {
                throw new ShippingException(ex.GetInnermostException().Message);
            }
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
