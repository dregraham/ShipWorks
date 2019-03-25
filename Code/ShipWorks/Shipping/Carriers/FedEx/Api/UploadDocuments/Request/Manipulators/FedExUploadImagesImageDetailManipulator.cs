using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Data.Model;
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
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            FedExAccountEntity account = (FedExAccountEntity) request.CarrierAccountEntity;

            // We can safely cast this since we've passed validation
            UploadImagesRequest nativeRequest = (UploadImagesRequest) request.NativeRequest;

            nativeRequest.Images = AddImageDetail(account).ToArray();
        }

        private List<UploadImageDetail> AddImageDetail(FedExAccountEntity account)
        {
            List<UploadImageDetail> images = new List<UploadImageDetail>();

            if (account.Fields[(int) FedExAccountFieldIndex.Letterhead].IsChanged)
            {
                byte[] letterhead = Convert.FromBase64String(account.Letterhead);

                UploadImageDetail letterheadDetail = new UploadImageDetail
                {
                    Id = ImageId.IMAGE_1,
                    IdSpecified = true,
                    Image = letterhead
                };

                images.Add(letterheadDetail);
            }

            if (account.Fields[(int) FedExAccountFieldIndex.Signature].IsChanged)
            {
                byte[] signature = Convert.FromBase64String(account.Signature);

                UploadImageDetail signatureDetail = new UploadImageDetail
                {
                    Id = ImageId.IMAGE_2,
                    IdSpecified = true,
                    Image = signature
                };

                images.Add(signatureDetail);
            }

            return images;
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        [SuppressMessage("ShipWorks", "SW0002", Justification = "The parameter name is only used for exception messages")]
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
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