using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System.Linq;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the InternationalTrafficInArmsRegulationsDetail
    /// attributes within the FedEx API's IFedExNativeShipmentRequest object if the shipment has a traffic in arms license number.
    /// </summary>
    public class FedExTrafficInArmsManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTrafficInArmsManipulator" /> class.
        /// </summary>
        public FedExTrafficInArmsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTrafficInArmsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExTrafficInArmsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            if (!string.IsNullOrEmpty(request.ShipmentEntity.FedEx.TrafficInArmsLicenseNumber))
            {
                AddTrafficInArmsOption(nativeRequest);

                InternationalTrafficInArmsRegulationsDetail armsDetail = new InternationalTrafficInArmsRegulationsDetail();
                armsDetail.LicenseOrExemptionNumber = request.ShipmentEntity.FedEx.TrafficInArmsLicenseNumber;

                nativeRequest.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail = armsDetail;
            }
        }

        /// <summary>
        /// Adds the traffic in arms option to the reqeust.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        private void AddTrafficInArmsOption(IFedExNativeShipmentRequest nativeRequest)
        {
            List<ShipmentSpecialServiceType> services = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes?.ToList() ?? new List<ShipmentSpecialServiceType>();
            services.Add(ShipmentSpecialServiceType.INTERNATIONAL_TRAFFIC_IN_ARMS_REGULATIONS);
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = services.ToArray();
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

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

        }
    }
}
