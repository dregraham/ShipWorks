using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExGlobalShipAddressWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;

            nativeRequest.WebAuthenticationDetail = new WebAuthenticationDetail
            {
                CspCredential = new WebAuthenticationCredential
                {
                    Key = fedExSettings.CspCredentialKey,
                    Password = fedExSettings.CspCredentialPassword
                },
                UserCredential = new WebAuthenticationCredential
                {
                    Key = fedExSettings.UserCredentialsKey,
                    Password = fedExSettings.UserCredentialsPassword
                }
            };
        } 
          
        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a VersionCaptureRequest
            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
