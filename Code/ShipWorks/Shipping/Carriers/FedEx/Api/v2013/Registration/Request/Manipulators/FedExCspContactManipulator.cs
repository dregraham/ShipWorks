using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// contact information of a RegisterWebCspUserRequest object.
    /// </summary>
    public class FedExCspContactManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed validation
            RegisterWebCspUserRequest nativeRequest = request.NativeRequest as RegisterWebCspUserRequest;

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;
            PersonAdapter person = new PersonAdapter(account, string.Empty);

            nativeRequest.BillingAddress = FedExApiCore.CreateAddress<Address>(person);

            nativeRequest.UserContactAndAddress = new ParsedContactAndAddress();
            nativeRequest.UserContactAndAddress.Address = FedExApiCore.CreateAddress<Address>(person);
            nativeRequest.UserContactAndAddress.Contact = FedExApiCore.CreateParsedContact<ParsedContact>(person);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a VersionCaptureRequest
            RegisterWebCspUserRequest nativeRequest = request.NativeRequest as RegisterWebCspUserRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
