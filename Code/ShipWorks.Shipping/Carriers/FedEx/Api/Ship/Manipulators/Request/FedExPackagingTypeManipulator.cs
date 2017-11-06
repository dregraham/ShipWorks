using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding package type information to the FedEx request
    /// </summary>
    public class FedExPackagingTypeManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Add the Packaging Type to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            RequestedShipment requestedShipment = request.Ensure(x => x.RequestedShipment);

            var packagingType = (FedExServiceType) shipment.FedEx.Service != FedExServiceType.SmartPost ?
                (FedExPackagingType) shipment.FedEx.PackagingType :
                FedExPackagingType.Custom;

            return GetApiPackagingType(packagingType)
                .Map(x => requestedShipment.PackagingType = x)
                .Map(x => request);
        }

        /// <summary>
        /// Determine the ship service packaging type
        /// </summary>
        private static GenericResult<PackagingType> GetApiPackagingType(FedExPackagingType packagingType)
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

            return new InvalidOperationException("Invalid FedEx Packaging Type");
        }
    }
}
