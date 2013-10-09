using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response
{
    public class FedExSubscriptionResponse : ICarrierResponse
    {
        private readonly SubscriptionReply nativeResponse;
        private readonly CarrierRequest request;


        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSubscriptionResponse" /> class.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request.</param>
        public FedExSubscriptionResponse(SubscriptionReply nativeResponse, CarrierRequest request)
        {
            this.nativeResponse = nativeResponse;
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
            get { return nativeResponse; }
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier. FedEx will supply a meter number
        /// for the account; this value is saved back to the account entity in the original request.
        /// </summary>
        /// <exception cref="FedExApiCarrierException"></exception>
        public void Process()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR || nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(nativeResponse.Notifications);
            }

            // Save the meter number back to the account in the original request
            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;
            account.MeterNumber = nativeResponse.MeterNumber;
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
