using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExRegistrationClientDetailManipulator : ICarrierRequestManipulator
    {
        private Type requestType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationClientDetailManipulator" /> class.
        /// </summary>
        public FedExRegistrationClientDetailManipulator()
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);
            
            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;

            if (requestType == typeof (VersionCaptureRequest))
            {
                // We can safely cast this since we've passed validation
                VersionCaptureRequest nativeRequest = request.NativeRequest as VersionCaptureRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);
            }
            else if (requestType == typeof(RegisterWebUserRequest))
            {
                // We can safely cast this since we've passed validation
                RegisterWebUserRequest nativeRequest = request.NativeRequest as RegisterWebUserRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);
            }
            else
            {
                // We can safely cast this since we've passed validation
                SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);
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
            if (requestType != typeof (VersionCaptureRequest) && requestType != typeof (RegisterWebUserRequest) && requestType != typeof(SubscriptionRequest))
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
} 
