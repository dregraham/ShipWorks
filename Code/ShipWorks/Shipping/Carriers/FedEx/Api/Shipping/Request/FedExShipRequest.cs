using System.Collections.Generic;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest interface that sends a request to the FedEx API for shipping an order/creating a label.
    /// </summary>
    public class FedExShipRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway fedExService;
        private readonly ICarrierResponseFactory responseFactory;
        private readonly FedExAccountEntity accountEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators to be used to build the request sent to FedEx.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="fedExService">The FedEx service used to route the request to FedEx.</param>
        /// <param name="responseFactory">The response factory that should be used to create the ICarrierResponse object returned from the Submit method.</param>
        [NDependIgnoreTooManyParams]
        public FedExShipRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity, IFedExServiceGateway fedExService, ICarrierResponseFactory responseFactory, ICarrierSettingsRepository settingsRepository, IFedExNativeShipmentRequest shipmentRequest)
            : base(requestManipulators, shipmentEntity)
        {
            this.fedExService = fedExService;
            this.responseFactory = responseFactory;

            accountEntity = (FedExAccountEntity)settingsRepository.GetAccount(shipmentEntity);

            // The native FedEx request needs to be a ProcessShipmentRequest to send the shipment to the FedEx service
            this.NativeRequest = shipmentRequest;
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return accountEntity; }
        }

        /// <summary>
        /// Submits the request to the FedEx API to ship an order/create a label.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw request for the FedEx service
            ApplyManipulators();
            
            // The request is ready to be sent to FedEx; we're sure the native request will be a ProcessShipmentRequest 
            // (since we assigned it as such in the constructor) so we can safely cast it here
            IFedExNativeShipmentReply nativeResponse = fedExService.Ship(this.NativeRequest as IFedExNativeShipmentRequest);

            // Defer to the response factory to create the ship response that wraps the raw/native response obtained from the service
            return responseFactory.CreateShipResponse(nativeResponse, this, this.ShipmentEntity);
        }
    }
}
