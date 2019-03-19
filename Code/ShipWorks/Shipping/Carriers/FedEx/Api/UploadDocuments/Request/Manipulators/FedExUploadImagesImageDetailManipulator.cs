using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the ImageDetail information of a UploadImagesRequest object.
    /// </summary>
    public class FedExUploadImagesImageDetailManipulator : ICarrierRequestManipulator
    {

        /// <summary>
        /// Manipulate the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            List<UploadImageDetail> images = new List<UploadImageDetail>();

            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;

            // We can safely cast this since we've passed validation
            UploadImagesRequest nativeRequest = (UploadImagesRequest) request.NativeRequest;

            if (account?.Letterhead != null)
            {
                byte[] letterhead = Encoding.ASCII.GetBytes(account.Letterhead);
                UploadImageDetail letterheadDetail = new UploadImageDetail
                {
                    Id = ImageId.IMAGE_1, 
                    Image = letterhead
                };
                
                images.Add(letterheadDetail);
            }

            if (account?.Signature != null)
            {
                byte[] signature = Encoding.ASCII.GetBytes(account.Signature);

                UploadImageDetail signatureDetail = new UploadImageDetail
                {
                    Id = ImageId.IMAGE_2,
                    Image = signature
                };

                images.Add(signatureDetail);
            }

            nativeRequest.Images = images.ToArray();
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