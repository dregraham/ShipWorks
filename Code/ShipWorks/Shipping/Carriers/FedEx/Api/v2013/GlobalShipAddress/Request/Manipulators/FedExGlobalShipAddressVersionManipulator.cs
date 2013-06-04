using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a SearchLocationsRequest object.
    /// </summary>
    public class FedExGlobalShipAddressVersionManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            ValidateRequest(request);

            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;

            nativeRequest.Version = new VersionId()
            {
                ServiceId = "gsai",
                Major = 1,
                Intermediate = 0,
                Minor = 0
            };
        }

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a VersionCaptureRequest
            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
