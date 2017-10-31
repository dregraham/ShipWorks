using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will add the appropriate special
    /// shipment type attributes to the RateRequest object for obtaining FedEx One Rate rate results.
    /// </summary>
    public class FedExRateOneRateManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override void Manipulate(CarrierRequest request)
        {
            InitializeRequest(request);

            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Since we'll be assigning this list back to the native request, create a list of the existing 
            // special service types that are on the request already so we don't overwrite anything
            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                specialServiceTypes.AddRange(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }

            specialServiceTypes.Add(ShipmentSpecialServiceType.FEDEX_ONE_RATE);

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
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

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // We'll be accessing/manipulating the special services, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                // We'll be accessing/manipulating the special service types, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }
        }
    }
}
