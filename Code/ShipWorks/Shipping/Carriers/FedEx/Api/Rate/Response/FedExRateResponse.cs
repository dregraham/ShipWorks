using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response
{
    public class FedExRateResponse : ICarrierResponse
    {
        private readonly CarrierRequest request;
        private readonly RateReply rateReply;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateResponse" /> class.
        /// </summary>
        /// <param name="rateReply">The rate reply.</param>
        /// <param name="request">The request.</param>
        public FedExRateResponse(RateReply rateReply, CarrierRequest request)
        {
            this.request = request;
            this.rateReply = rateReply;
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
        /// Gets the native response received from the carrier API, a FedEx RateReply in this case.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return rateReply; }
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        public void Process()
        {
            // Nothing really to process within the response other than checking for any errors
            if (rateReply.HighestSeverity == NotificationSeverityType.ERROR || rateReply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                if (rateReply.Notifications.Any(n => n.Code == "521"))
                {
                    throw new FedExException("No services returned due to invalid postal code.");
                }

                throw new FedExApiCarrierException(rateReply.Notifications);
            }

            if (rateReply.RateReplyDetails == null)
            {
                if (rateReply.Notifications.Any(n => n.Code == "556" || n.Code == "557" || n.Code == "558"))
                {
                    throw new FedExException("There are no FedEx services available for the selected shipment options.");
                }

                throw new FedExException("FedEx did not return any rates for the shipment.");
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
