using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx TrackRequest request types.
    /// </summary>
    public class FedExUploadImagesRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway serviceGateway;
        private readonly IFedExResponseFactory responseFactory;
        private readonly FedExAccountEntity accountEntity;

        public FedExUploadImagesRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators,
            IFedExServiceGateway fedExService, IFedExResponseFactory responseFactory, FedExAccountEntity accountEntity)
            : base(requestManipulators, null)
        {
            serviceGateway = fedExService;
            this.responseFactory = responseFactory;
            this.accountEntity = accountEntity;

            NativeRequest = new UploadImagesRequest();
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity => accountEntity;

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw input request for the FedEx service
            ApplyManipulators();

            UploadImagesReply nativeResponse = serviceGateway.UploadImages(NativeRequest as UploadImagesRequest);
            return responseFactory.CreateUploadImagesResponse(nativeResponse, this);
        }
    }
}
