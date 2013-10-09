using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Response
{
    /// <summary>
    /// Process CaptureVersion reply
    /// </summary>
    public class FedExVersionCaptureResponse : ICarrierResponse
    {
        private VersionCaptureReply reply;
        private CarrierRequest request;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExVersionCaptureResponse(VersionCaptureReply reply, CarrierRequest request)
        {
            this.reply = reply;
            this.request = request;
        }


        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        public CarrierRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return reply; }
        }

        /// <summary>
        /// Process CaptureVersion reply - ErrorHandling
        /// </summary>
        public void Process()
        {
            if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(reply.Notifications);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
