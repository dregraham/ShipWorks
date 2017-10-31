using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateWebAuthenticationManipulator: ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExRateWebAuthenticationManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The FedEx settings.</param>
        public FedExRateWebAuthenticationManipulator(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a RateRequest object.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            nativeRequest.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateRateWebAuthenticationDetail(fedExSettings);
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

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
