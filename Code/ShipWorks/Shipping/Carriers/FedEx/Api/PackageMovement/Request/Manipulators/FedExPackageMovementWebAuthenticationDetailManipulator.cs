using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators
{
    public class FedExPackageMovementWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageMovementWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        public FedExPackageMovementWebAuthenticationDetailManipulator(IFedExSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a PostalCodeInquiryRequest object.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            PostalCodeInquiryRequest nativeRequest = request.NativeRequest as PostalCodeInquiryRequest;

            nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(fedExSettings);
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

            // The native FedEx request type should be a PostalCodeInquiryRequest
            PostalCodeInquiryRequest nativeRequest = request.NativeRequest as PostalCodeInquiryRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
