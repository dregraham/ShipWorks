using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the WebAuthenticationDetail information of a UploadImageRequest object.
    /// </summary>
    public class FedExUploadImagesWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExUploadImagesWebAuthenticationDetailManipulator" /> class
        /// using FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExUploadImagesWebAuthenticationDetailManipulator() 
            : this(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExUploadImagesWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExUploadImagesWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request by settings the WebAuthenticationDetail property of a UploadImagesRequest object.
        /// </summary>
        /// <param name="request"></param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            UploadImagesRequest nativeRequest = request.NativeRequest as UploadImagesRequest;
            nativeRequest.WebAuthenticationDetail =
                FedExRequestManipulatorUtilities.CreateUploadImagesWebAuthenticationDetail(fedExSettings);
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // See if the native request is for UploadImages
            UploadImagesRequest nativeRequest = request.NativeRequest as UploadImagesRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

        }
    }
}