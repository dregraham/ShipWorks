using System;
using System.Collections.Generic;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator for modifying the packaging properties of
    /// the package line items in the FedEx IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExAdmissibilityManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.AdjustedShipCountryCode() == "CA";

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            var package = InitializeRequest(request);

            return GetApiAdmissibilityPackagingType((FedExPhysicalPackagingType) shipment.FedEx.CustomsAdmissibilityPackaging)
                .Map(packaging =>
                {
                    package.PhysicalPackaging = packaging;
                    package.PhysicalPackagingSpecified = true;
                    return request;
                });
        }

        /// <summary>
        /// Determine the API value corresponding to our internal type
        /// </summary>
        private GenericResult<PhysicalPackagingType> GetApiAdmissibilityPackagingType(FedExPhysicalPackagingType type) =>
            packagingLookup.Value
                .Match(type, x => x, () => new InvalidOperationException("Invalid FedEx Admissibility Type: " + type));

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private RequestedPackageLineItem InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment)
            .EnsureAtLeastOne(x => x.RequestedPackageLineItems);

        /// <summary>
        /// Lookup for valid packaging types
        /// </summary>
        private static Lazy<Dictionary<FedExPhysicalPackagingType, PhysicalPackagingType>> packagingLookup =
            new Lazy<Dictionary<FedExPhysicalPackagingType, PhysicalPackagingType>>(() =>
                new Dictionary<FedExPhysicalPackagingType, PhysicalPackagingType>
                {
                    { FedExPhysicalPackagingType.Bag, PhysicalPackagingType.BAG },
                    { FedExPhysicalPackagingType.Barrel, PhysicalPackagingType.BARREL },
                    { FedExPhysicalPackagingType.BasketOrHamper, PhysicalPackagingType.BASKET },
                    { FedExPhysicalPackagingType.Box, PhysicalPackagingType.BOX },
                    { FedExPhysicalPackagingType.Bucket, PhysicalPackagingType.BUCKET },
                    { FedExPhysicalPackagingType.Bundle, PhysicalPackagingType.BUNDLE },
                    { FedExPhysicalPackagingType.Carton, PhysicalPackagingType.CARTON },
                    { FedExPhysicalPackagingType.Case, PhysicalPackagingType.CASE },
                    { FedExPhysicalPackagingType.Container, PhysicalPackagingType.CONTAINER },
                    { FedExPhysicalPackagingType.Crate, PhysicalPackagingType.CRATE },
                    { FedExPhysicalPackagingType.Cylinder, PhysicalPackagingType.CYLINDER },
                    { FedExPhysicalPackagingType.Drum, PhysicalPackagingType.DRUM },
                    { FedExPhysicalPackagingType.Envelope, PhysicalPackagingType.ENVELOPE },
                    { FedExPhysicalPackagingType.Pail, PhysicalPackagingType.PAIL },
                    { FedExPhysicalPackagingType.Pallet, PhysicalPackagingType.PALLET },
                    { FedExPhysicalPackagingType.Pieces, PhysicalPackagingType.PIECE },
                    { FedExPhysicalPackagingType.Reel, PhysicalPackagingType.REEL },
                    { FedExPhysicalPackagingType.Roll, PhysicalPackagingType.ROLL },
                    { FedExPhysicalPackagingType.Skid, PhysicalPackagingType.SKID },
                    { FedExPhysicalPackagingType.Tank, PhysicalPackagingType.TANK },
                    { FedExPhysicalPackagingType.Tube, PhysicalPackagingType.TUBE },
                    { FedExPhysicalPackagingType.Hamper, PhysicalPackagingType.HAMPER },
                    { FedExPhysicalPackagingType.Other, PhysicalPackagingType.OTHER }
                });
    }
}
