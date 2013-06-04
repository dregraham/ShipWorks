using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Tracking.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the WebAuthenticationDetail information of a TrackRequest object.
    /// </summary>
    public class FedExTrackingWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTrackingWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExTrackingWebAuthenticationDetailManipulator()
            : this(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTrackingWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExTrackingWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a TrackRequest object.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            TrackRequest nativeRequest = request.NativeRequest as TrackRequest;
            nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateTrackWebAuthenticationDetail(fedExSettings);
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
