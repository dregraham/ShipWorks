using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding package type information to the FedEx request
    /// </summary>
    public class FedExPackagingTypeManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackagingTypeManipulator" /> class.
        /// </summary>
        public FedExPackagingTypeManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackagingTypeManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExPackagingTypeManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Packaging Type to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Get the FedEx shipment
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            // If we aren't SmartPost, set the packaging type
            if ((FedExServiceType)fedExShipment.Service != FedExServiceType.SmartPost)
            {
                // Set the packaging type for the shipment
                requestedShipment.PackagingType = GetApiPackagingType((FedExPackagingType) fedExShipment.PackagingType);
            }
            else
            {
                requestedShipment.PackagingType = GetApiPackagingType(FedExPackagingType.Custom);
            }
        }

        /// <summary>
        /// Determine the ship service packaging type
        /// </summary>
        private static PackagingType GetApiPackagingType(FedExPackagingType packagingType)
        {
            switch (packagingType)
            {
                case FedExPackagingType.Box: 
                    return PackagingType.FEDEX_BOX;
                case FedExPackagingType.Box10Kg: 
                    return PackagingType.FEDEX_10KG_BOX;
                case FedExPackagingType.Box25Kg: 
                    return PackagingType.FEDEX_25KG_BOX;
                case FedExPackagingType.Custom: 
                    return PackagingType.YOUR_PACKAGING;
                case FedExPackagingType.Envelope: 
                    return PackagingType.FEDEX_ENVELOPE;
                case FedExPackagingType.Pak: 
                    return PackagingType.FEDEX_PAK;
                case FedExPackagingType.Tube: 
                    return PackagingType.FEDEX_TUBE;
                case FedExPackagingType.SmallBox:
                    return PackagingType.FEDEX_SMALL_BOX;
                case FedExPackagingType.MediumBox:
                    return PackagingType.FEDEX_MEDIUM_BOX;
                case FedExPackagingType.LargeBox:
                    return PackagingType.FEDEX_LARGE_BOX;
                case FedExPackagingType.ExtraLargeBox:
                    return PackagingType.FEDEX_EXTRA_LARGE_BOX;
            }

            throw new InvalidOperationException("Invalid FedEx Packaging Type");
        }
    }
}
