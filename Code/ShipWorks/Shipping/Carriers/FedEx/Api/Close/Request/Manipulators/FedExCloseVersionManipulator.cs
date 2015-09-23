using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a GroundCloseRequest object.
    /// </summary>
    public class FedExCloseVersionManipulator : ICarrierRequestManipulator
    {
        private bool isSmartPostRequest;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            if (isSmartPostRequest)
            {
                // We can safely cast this since we've passed validation
                SmartPostCloseRequest nativeRequest = request.NativeRequest as SmartPostCloseRequest;
                nativeRequest.Version = GetVersion();
            }
            else
            {
                // We can safely cast this since we've passed validation
                GroundCloseRequest nativeRequest = request.NativeRequest as GroundCloseRequest;
                nativeRequest.Version = GetVersion();
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns></returns>
        private VersionId GetVersion()
        {
            return new VersionId
            {
                ServiceId = "clos",
                Major = 4,
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
        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be eitehr a GroundCloseRequest or SmartPostCloseRequest
            SmartPostCloseRequest smartPostRequest = request.NativeRequest as SmartPostCloseRequest;
            if (smartPostRequest != null)
            {
                isSmartPostRequest = true;
            }
            else
            {
                // See if the native request is for ground close 
                GroundCloseRequest nativeRequest = request.NativeRequest as GroundCloseRequest;
                if (nativeRequest == null)
                {
                    // Abort - we have an unexpected native request
                    throw new CarrierException("An unexpected request type was provided.");
                }

                isSmartPostRequest = false;
            }
        }
    }
}
