using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a UploadImagesRequest object.
    /// </summary>
    public class FedExUploadImagesVersionManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            UploadImagesRequest nativeRequest = request.NativeRequest as UploadImagesRequest;
            nativeRequest.Version = new VersionId
            {
                ServiceId = "cdus",
                Major = 5,
                Intermediate = 0,
                Minor = 0
            };
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

            // The native FedEx request type should be a UploadImagesRequest
            UploadImagesRequest nativeRequest = request.NativeRequest as UploadImagesRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}