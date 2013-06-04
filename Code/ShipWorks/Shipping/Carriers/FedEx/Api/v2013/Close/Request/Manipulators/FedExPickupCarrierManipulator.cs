using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Request.Manipulators
{
    /// <summary>
    /// An IcarrierRequestManipulator implementation that sets the carrier type of a SmartPostloseRequest manipulator.
    /// </summary>
    public class FedExPickupCarrierManipulator : ICarrierRequestManipulator
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
            SmartPostCloseRequest nativeRequest = request.NativeRequest as SmartPostCloseRequest;

            nativeRequest.PickUpCarrier = CarrierCodeType.FXSP;
            nativeRequest.PickUpCarrierSpecified = true;
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

            // The native FedEx request type should be a GroundCloseRequest
            SmartPostCloseRequest nativeRequest = request.NativeRequest as SmartPostCloseRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
