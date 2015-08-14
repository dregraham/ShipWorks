using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response
{
    /// <summary>
    /// Processes SearchLocationsReply from FedEx
    /// </summary>
    public class FedExGlobalShipAddressResponse : ICarrierResponse
    {
        private readonly SearchLocationsReply nativeResponse;
        private CarrierRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressResponse" /> class.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="request">The request.</param>
        public FedExGlobalShipAddressResponse(SearchLocationsReply reply, CarrierRequest request)
        {
            this.nativeResponse = reply;
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
        /// Details populated after process
        /// </summary>
        public DistanceAndLocationDetail[] DistanceAndLocationDetails
        {
            get;
            private set;
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
        /// Function that tells CarrierResponse to process a request's response
        /// </summary>
        public void Process()
        {
            Verify();

            DistanceAndLocationDetail[] distanceAndLocationDetails = nativeResponse.AddressToLocationRelationships[0].DistanceAndLocationDetails;

            DistanceAndLocationDetails = distanceAndLocationDetails.ToList().Take(5).ToArray();
        }

        /// <summary>
        /// Verifies the response from carrier
        /// </summary>
        /// <exception cref="FedExApiCarrierException"></exception>
        /// <exception cref="CarrierException">No locations found.</exception>
        private void Verify()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR || nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(nativeResponse.Notifications);
            }

            if (nativeResponse.ResultsReturned == "0")
            {
                throw new CarrierException("No locations found.");
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
