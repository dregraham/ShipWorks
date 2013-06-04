using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request
{
    /// <summary>
    /// An implementation of the carrierRequest interface that sends a request to the UPS API for registerring an account.
    /// </summary>
    public class UpsInvoiceRegistrationRequest : CarrierRequest
    {
        private readonly IUpsServiceGateway serviceGateway;
        private readonly ICarrierResponseFactory responseFactory;
        private readonly UpsAccountEntity account;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="serviceGateway">The service gateway.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="account">The account.</param>
        public UpsInvoiceRegistrationRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, IUpsServiceGateway serviceGateway, ICarrierResponseFactory responseFactory, UpsAccountEntity account)
            : base(requestManipulators,null)
        {
            this.serviceGateway = serviceGateway;
            this.responseFactory = responseFactory;
            NativeRequest = new RegisterRequest();
            this.account = account;
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>
        /// The carrier account entity.
        /// </value>
        public override IEntity2 CarrierAccountEntity
        {
            get
            {
                return account;
            }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// An ICarrierResponse containing the carrier-specific results of the request.
        /// </returns>
        public override ICarrierResponse Submit()
        {
            ApplyManipulators();

            RegisterResponse nativeResponse = serviceGateway.RegisterAccount(NativeRequest as RegisterRequest);
            return responseFactory.CreateRegisterUserResponse(nativeResponse, this);
        }
    }
}
