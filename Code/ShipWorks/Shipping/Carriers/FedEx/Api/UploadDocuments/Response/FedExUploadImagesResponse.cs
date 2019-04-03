using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Response
{
    /// <summary>
    /// An implementation of the ICarrierRequest representing the response from a FedEx UploadImages request.
    /// </summary>
    public class FedExUploadImagesResponse : ICarrierResponse
    {
        private UploadImagesReply nativeResponse = new UploadImagesReply();
        private CarrierRequest request;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExUploadImagesResponse(UploadImagesReply reply, CarrierRequest request)
        {
            nativeResponse = reply;
            this.request = request;
        }

        /// <summary>
        /// Get the request that was used to generate the response.
        /// </summary>
        public CarrierRequest Request => request;

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse => nativeResponse;

        /// <summary>
        /// Verify the response.
        /// </summary>
        public void Process()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR ||
                nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(nativeResponse.Notifications);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}