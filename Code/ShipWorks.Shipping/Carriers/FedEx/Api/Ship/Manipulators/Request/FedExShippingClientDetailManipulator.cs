using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExShippingClientDetailManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClientDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExShippingClientDetailManipulator() : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClientDetailManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The FedEx settings.</param>
        public FedExShippingClientDetailManipulator(FedExSettings fedExSettings) : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClientDetailManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExShippingClientDetailManipulator(ICarrierSettingsRepository settingsRepository)
            : base(settingsRepository)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            // We can safely cast this since we've passed validation
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity; 
            nativeRequest.ClientDetail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account);
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

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
