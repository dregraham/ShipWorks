using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response
{
    /// <summary>
    /// Rate response for FedEx
    /// </summary>
    [Component]
    public class FedExRateResponse : IFedExRateResponse
    {
        private readonly RateReply response;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateResponse" /> class.
        /// </summary>
        public FedExRateResponse(RateReply response)
        {
            this.response = response;
        }

        /// <summary>
        /// Performs any processing required based on the response from the carrier.
        /// </summary>
        public RateReply Process()
        {
            // Nothing really to process within the response other than checking for any errors
            if (response.HighestSeverity == NotificationSeverityType.ERROR || response.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                if (response.Notifications.Any(n => n.Code == "521"))
                {
                    throw new FedExException("No services returned due to invalid postal code.");
                }

                throw new FedExApiCarrierException(response.Notifications);
            }

            if (response.RateReplyDetails == null)
            {
                if (response.Notifications.Any(n => n.Code == "556" || n.Code == "557" || n.Code == "558"))
                {
                    throw new FedExException("There are no FedEx services available for the selected shipment options.");
                }

                throw new FedExException("FedEx did not return any rates for the shipment.");
            }

            return response;
        }
    }
}
