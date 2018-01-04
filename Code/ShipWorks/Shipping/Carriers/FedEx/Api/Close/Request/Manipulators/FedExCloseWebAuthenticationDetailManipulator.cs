using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the WebAuthenticationDetail information of a GroundCloseRequest object.
    /// </summary>
    public class FedExCloseWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private bool isSmartPostRequest;
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCloseWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExCloseWebAuthenticationDetailManipulator()
            : this(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExCloseWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExCloseWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a GroundCloseRequest object.
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
                nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateCloseWebAuthenticationDetail(fedExSettings);
            }
            else
            {
                // We can safely cast this since we've passed validation
                GroundCloseRequest nativeRequest = request.NativeRequest as GroundCloseRequest;
                nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateCloseWebAuthenticationDetail(fedExSettings);
            }
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
