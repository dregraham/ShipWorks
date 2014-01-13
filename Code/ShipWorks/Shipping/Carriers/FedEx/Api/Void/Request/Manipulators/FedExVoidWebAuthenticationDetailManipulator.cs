using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the WebAuthenticationDetail information of a VoidRequest object.
    /// </summary>
    public class FedExVoidWebAuthenticationDetailManipulator : ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExVoidWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExVoidWebAuthenticationDetailManipulator()
            : this(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExVoidWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExVoidWebAuthenticationDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a VoidRequest object.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            DeleteShipmentRequest nativeRequest = request.NativeRequest as DeleteShipmentRequest;

            nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateVoidWebAuthenticationDetail(fedExSettings);
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

            // The native FedEx request type should be a DeleteShipmentRequest
            DeleteShipmentRequest nativeRequest = request.NativeRequest as DeleteShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
