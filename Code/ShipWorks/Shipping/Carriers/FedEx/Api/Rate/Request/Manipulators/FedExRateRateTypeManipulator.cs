using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the rate type property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRateRateTypeManipulator : ICarrierRequestManipulator
    {
        private readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateRateTypeManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRateRateTypeManipulator(ICarrierSettingsRepository settingsRepository)
        {
            // Need to consult the repository to find out what type of rates to use
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Use the repository to see what type of rates we should be using
            RateRequestType[] rateTypes = { settingsRepository.UseListRates ? RateRequestType.LIST : RateRequestType.NONE };
            nativeRequest.RequestedShipment.RateRequestTypes = rateTypes;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a ProcessShipmentRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }
    }
}
