using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that configures the 
    /// FedEx IFedExNativeShipmentRequest object with to indicate that the One Rate special
    /// service has been requested based on the service type of the FedEx shipment.
    /// </summary>
    public class FedExOneRateManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Manipulates the carrier request to conditionally set whether the request should
        /// reflect the One Rate special service type.
        /// </summary>
        /// <param name="request">The request.</param>
        public override void Manipulate(CarrierRequest request)
        {
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            FedExServiceType selectedServiceType = (FedExServiceType)request.ShipmentEntity.FedEx.Service;
            if (FedExUtility.OneRateServiceTypes.Contains(selectedServiceType))
            {
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

            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
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
