using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExRegistrationWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;
        private Type requestType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExRegistrationWebAuthenticationDetailManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationWebAuthenticationDetailManipulator"/> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRegistrationWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
            : this(new FedExSettings(settingsRepository))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The FedEx settings.</param>
        public FedExRegistrationWebAuthenticationDetailManipulator(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a VersionCaptureRequest object.
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
                nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(fedExSettings);
            }
            else if (requestType == typeof(RegisterWebUserRequest))
            {
                // We can safely cast this since we've passed validation
                RegisterWebUserRequest nativeRequest = request.NativeRequest as RegisterWebUserRequest;
                nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(fedExSettings);

                // Null out the user credential nodes otherwise FedEx will fail the request due to a schema validation error (since the user doesn't
                // technically have credentials at this point)
                nativeRequest.WebAuthenticationDetail.UserCredential = null;
            }
            else 
            {
                // We can safely cast this since we've passed validation
                SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;
                nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(fedExSettings);
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
