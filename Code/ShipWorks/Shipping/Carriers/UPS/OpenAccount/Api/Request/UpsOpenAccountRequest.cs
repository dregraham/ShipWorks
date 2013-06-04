﻿using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest interface that sends a request to the UPS API for opening an account
    /// </summary>
    public class UpsOpenAccountRequest : CarrierRequest
    {
        private readonly IUpsServiceGateway serviceGateway;
        private readonly ICarrierResponseFactory responseFactory;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountRequest" /> class.
        /// </summary>
        /// <param name="serviceGateway">The service gateway.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="request">The request.</param>
        public UpsOpenAccountRequest(IUpsServiceGateway serviceGateway, ICarrierResponseFactory responseFactory, OpenAccountRequest request)
            : base(null,null)
        {
            this.serviceGateway = serviceGateway;
            this.responseFactory = responseFactory;

            NativeRequest = request;
        }

        /// <summary>
        /// Gets the carrier account entity. This will always return null since there is not
        /// an account created yet. (Why else would you be using this request?)
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return null; }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public override ICarrierResponse Submit()
        {
            // The request is ready to be sent to UPS; we're sure the native request will be a OpenAccountResponse 
            // (since we assigned it as such in the constructor) so we can safely cast it here
            OpenAccountResponse nativeResponse = serviceGateway.OpenAccount(NativeRequest as OpenAccountRequest);
            return responseFactory.CreateSubscriptionResponse(nativeResponse, this);
        }
    }
}
