﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Request.Manipulators
{
    public class FedExRegistrationClientDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;
        private Type requestType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRegistrationClientDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRegistrationClientDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            this.fedExSettings = new FedExSettings(settingsRepository);
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
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, fedExSettings);
            }
            else if (requestType == typeof(RegisterWebCspUserRequest))
            {
                // We can safely cast this since we've passed validation
                RegisterWebCspUserRequest nativeRequest = request.NativeRequest as RegisterWebCspUserRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, fedExSettings);
            }
            else
            {
                // We can safely cast this since we've passed validation
                SubscriptionRequest nativeRequest = request.NativeRequest as SubscriptionRequest;
                nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, fedExSettings);
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
            if (requestType != typeof (VersionCaptureRequest) && requestType != typeof (RegisterWebCspUserRequest) && requestType != typeof(SubscriptionRequest))
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
} 
