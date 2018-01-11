using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response
{
    /// <summary>
    /// Processes SearchLocationsReply from FedEx
    /// </summary>
    [Component]
    public class FedExGlobalShipAddressResponse : IFedExGlobalShipAddressResponse
    {
        private readonly SearchLocationsReply nativeResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressResponse" /> class.
        /// </summary>
        public FedExGlobalShipAddressResponse(SearchLocationsReply reply)
        {
            this.nativeResponse = reply;
        }

        /// <summary>
        /// Function that tells CarrierResponse to process a request's response
        /// </summary>
        public GenericResult<DistanceAndLocationDetail[]> Process() =>
            Verify(nativeResponse)
                .Map(x => nativeResponse.AddressToLocationRelationships[0].DistanceAndLocationDetails.Take(5).ToArray());

        /// <summary>
        /// Verifies the response from carrier
        /// </summary>
        private static GenericResult<SearchLocationsReply> Verify(SearchLocationsReply reply)
        {
            if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                return new FedExApiCarrierException(reply.Notifications);
            }

            if (reply.ResultsReturned == "0")
            {
                return new CarrierException("No locations found.");
            }

            return reply;
        }
    }
}
