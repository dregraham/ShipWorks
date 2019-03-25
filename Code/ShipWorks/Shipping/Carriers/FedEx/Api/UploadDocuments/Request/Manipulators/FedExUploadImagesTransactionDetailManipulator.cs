using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the ClientDetail information of a UploadImagesRequest object.
    /// </summary>
    public class FedExUploadImagesTransactionDetailManipulator : ICarrierRequestManipulator
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
            nativeRequest.TransactionDetail = new TransactionDetail
            {
                CustomerTransactionId = "UploadImagesRequest_v11"
            };
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            UploadImagesRequest nativeRequest = request.NativeRequest as UploadImagesRequest;
            if (nativeRequest == null)
            {
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}