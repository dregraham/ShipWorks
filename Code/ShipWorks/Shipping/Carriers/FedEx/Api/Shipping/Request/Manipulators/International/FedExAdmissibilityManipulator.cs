using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for modifying the packaging properties of
    /// the package line items in the FedEx IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExAdmissibilityManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExAdmissibilityManipulator" /> class.
        /// </summary>
        public FedExAdmissibilityManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExAdmissibilityManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExAdmissibilityManipulator(FedExSettings fedExSettings)
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

            if (request.ShipmentEntity.AdjustedShipCountryCode() == "CA")
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging = GetApiAdmissibilityPackagingType((FedExPhysicalPackagingType) request.ShipmentEntity.FedEx.CustomsAdmissibilityPackaging);
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified = true;
            }
        }

        /// <summary>
        /// Determine the API value corresponding to our internal type
        /// </summary>
        private PhysicalPackagingType GetApiAdmissibilityPackagingType(FedExPhysicalPackagingType type)
        {
            switch (type)
            {
                case FedExPhysicalPackagingType.Bag: return PhysicalPackagingType.BAG;
                case FedExPhysicalPackagingType.Barrel: return PhysicalPackagingType.BARREL;
                case FedExPhysicalPackagingType.BasketOrHamper: return PhysicalPackagingType.BASKET;
                case FedExPhysicalPackagingType.Box: return PhysicalPackagingType.BOX;
                case FedExPhysicalPackagingType.Bucket: return PhysicalPackagingType.BUCKET;
                case FedExPhysicalPackagingType.Bundle: return PhysicalPackagingType.BUNDLE;
                case FedExPhysicalPackagingType.Carton: return PhysicalPackagingType.CARTON;
                case FedExPhysicalPackagingType.Case: return PhysicalPackagingType.CASE;
                case FedExPhysicalPackagingType.Container: return PhysicalPackagingType.CONTAINER;
                case FedExPhysicalPackagingType.Crate: return PhysicalPackagingType.CRATE;
                case FedExPhysicalPackagingType.Cylinder: return PhysicalPackagingType.CYLINDER;
                case FedExPhysicalPackagingType.Drum: return PhysicalPackagingType.DRUM;
                case FedExPhysicalPackagingType.Envelope: return PhysicalPackagingType.ENVELOPE;
                case FedExPhysicalPackagingType.Pail: return PhysicalPackagingType.PAIL;
                case FedExPhysicalPackagingType.Pallet: return PhysicalPackagingType.PALLET;
                case FedExPhysicalPackagingType.Pieces: return PhysicalPackagingType.PIECE;
                case FedExPhysicalPackagingType.Reel: return PhysicalPackagingType.REEL;
                case FedExPhysicalPackagingType.Roll: return PhysicalPackagingType.ROLL;
                case FedExPhysicalPackagingType.Skid: return PhysicalPackagingType.SKID;
                case FedExPhysicalPackagingType.Tank: return PhysicalPackagingType.TANK;
                case FedExPhysicalPackagingType.Tube: return PhysicalPackagingType.TUBE;
            }

            throw new InvalidOperationException("Invalid FedEx Admissibility Type: " + type);
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

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null || nativeRequest.RequestedShipment.RequestedPackageLineItems.Length == 0)
            {
                // Make sure the line item object is are there
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[0] == null)
            {
                // Be sure the first element is has been instantiated
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }
        }
    }
}
