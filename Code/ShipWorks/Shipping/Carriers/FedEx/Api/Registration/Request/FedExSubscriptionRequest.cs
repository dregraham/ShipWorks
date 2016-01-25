using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request
{
    /// <summary>
    /// A CarrierRequest implementation that issues FedEx SubscriptionRequest request types.
    /// </summary>
    public class FedExSubscriptionRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway serviceGateway;
        private readonly FedExAccountEntity accountEntity;
        private readonly ICarrierResponseFactory responseFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSubscriptionRequest" /> class using
        /// the "live" implementations for the FedExServiceGateway and FedExResponseFactory.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="accountEntity">The account entity.</param>
        public FedExSubscriptionRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, FedExAccountEntity accountEntity)
            : this(requestManipulators, new FedExServiceGateway(new FedExSettingsRepository()), new FedExResponseFactory(new FedExLabelRepository()), accountEntity)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSubscriptionRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="serviceGateway">The service gateway.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="accountEntity">The account entity.</param>
        public FedExSubscriptionRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, IFedExServiceGateway serviceGateway, ICarrierResponseFactory responseFactory, FedExAccountEntity accountEntity)
            : base(requestManipulators, null)
        {
            this.serviceGateway = serviceGateway;
            this.responseFactory = responseFactory;
            this.accountEntity = accountEntity;

            NativeRequest = new SubscriptionRequest();
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

            // The request is ready to be sent to FedEx; we're sure the native request will be a SubscriptionRequest
            // (since we assigned it as such in the constructor) so we can safely cast it here
            SubscriptionReply nativeResponse = serviceGateway.SubscribeShipper(this.NativeRequest as SubscriptionRequest);
            return responseFactory.CreateSubscriptionResponse(nativeResponse, this);
        }
    }
}
