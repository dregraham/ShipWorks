using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx GroundCloseRequest request types.
    /// </summary>
    public class FedExGroundCloseRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway serviceGateway;
        private readonly IFedExResponseFactory responseFactory;
        private readonly FedExAccountEntity accountEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="fedExService">The fed ex service.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="accountEntity">The account entity.</param>
        public FedExGroundCloseRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity, IFedExServiceGateway fedExService, IFedExResponseFactory responseFactory, FedExAccountEntity accountEntity)
            : base(requestManipulators, shipmentEntity)
        {
            this.serviceGateway = fedExService;
            this.responseFactory = responseFactory;
            this.accountEntity = accountEntity;

            this.NativeRequest = new GroundCloseRequest();
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
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw request for the FedEx service
            ApplyManipulators();

            // The request is ready to be sent to FedEx; we're sure the native request will be a GroundCloseRequest
            // (since we assigned it as such in the constructor) so we can safely cast it here
            GroundCloseReply nativeResponse = serviceGateway.Close(this.NativeRequest as GroundCloseRequest);
            return responseFactory.CreateGroundCloseResponse(nativeResponse, this);
        }
    }
}
