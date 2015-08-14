using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a TrackRequest object.
    /// </summary>
    public class FedExTrackingVersionManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            TrackRequest nativeRequest = request.NativeRequest as TrackRequest;
            nativeRequest.Version = GetVersion();
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns></returns>
        private static VersionId GetVersion()
        {
            return new VersionId
            {
                ServiceId = "trck",
                Major = 10,
                Intermediate = 0,
                Minor = 0
            };
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // See if the native request is for tracking 
            TrackRequest nativeRequest = request.NativeRequest as TrackRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
