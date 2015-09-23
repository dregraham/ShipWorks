using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a VersionCaptureRequest object.
    /// </summary>
    public class FedExRegistrationVersionManipulator : ICarrierRequestManipulator
    {
        private Type requestType;
        private VersionId registrationVersionInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationVersionManipulator" /> class.
        /// </summary>
        public FedExRegistrationVersionManipulator()
        {
            registrationVersionInfo = new VersionId
            {
                ServiceId = "fcas",
                Major = 7,
                Intermediate = 0,
                Minor = 0
            };
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            if (requestType == typeof (VersionCaptureRequest))
            {
                // We can safely cast this since we've passed validation
                VersionCaptureRequest nativeRequest = request.NativeRequest as VersionCaptureRequest;
                nativeRequest.Version = registrationVersionInfo;
            }
            else if (requestType == typeof(RegisterWebUserRequest))
            {
                // We can safely cast this since we've passed validation
                RegisterWebUserRequest nativeRequest = request.NativeRequest as RegisterWebUserRequest;
                nativeRequest.Version = registrationVersionInfo;
            }
            else 
            {
                // We can safely cast this since we've passed validation
                SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;
                nativeRequest.Version = registrationVersionInfo;
            }
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.NativeRequest == null)
            {
                throw new CarrierException("A null request was provided.");
            }

            requestType = request.NativeRequest.GetType();
            if (requestType != typeof(VersionCaptureRequest) && requestType != typeof(RegisterWebUserRequest) && requestType != typeof(SubscriptionRequest))
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
