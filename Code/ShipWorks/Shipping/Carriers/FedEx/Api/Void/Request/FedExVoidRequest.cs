using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx VoidRequest request types.
    /// </summary>
    public class FedExVoidRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway serviceGateway;
        private readonly IFedExResponseFactory responseFactory;
        private readonly FedExAccountEntity accountEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExVoidRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="fedExService">The fed ex service.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="accountEntity">The account entity.</param>
        public FedExVoidRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity, IFedExServiceGateway fedExService, IFedExResponseFactory responseFactory, FedExAccountEntity accountEntity)
            : base(requestManipulators, shipmentEntity)
        {
            this.serviceGateway = fedExService;
            this.responseFactory = responseFactory;
            this.accountEntity = accountEntity;

            this.NativeRequest = new DeleteShipmentRequest();
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
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// An ICarrierResponse containing the carrier-specific results of the request.
        /// </returns>
        /// <exception cref="System.NotImplementedException">The service gateway for  Void needs to be implemented.</exception>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw request for the FedEx service
            ApplyManipulators();

            // The request is ready to be sent to FedEx; we're sure the native request will be a VoidRequest 
            // (since we assigned it as such in the constructor) so we can safely cast it here
            ShipmentReply nativeResponse = serviceGateway.Void(this.NativeRequest as DeleteShipmentRequest);
            return responseFactory.CreateVoidResponse(nativeResponse, this);
        }
    }
}
