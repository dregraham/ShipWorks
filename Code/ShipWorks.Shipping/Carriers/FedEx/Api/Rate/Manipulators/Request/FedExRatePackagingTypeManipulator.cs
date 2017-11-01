using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Add packaging type details for FedEx rate request
    /// </summary>
    public class FedExRatePackagingTypeManipulator : IFedExRateRequestManipulator
    {
        private static Lazy<Dictionary<FedExPackagingType, PackagingType>> packageTypeLookup =
            new Lazy<Dictionary<FedExPackagingType, PackagingType>>(() =>
                new Dictionary<FedExPackagingType, PackagingType> {
                    { FedExPackagingType.Box, PackagingType.FEDEX_BOX },
                    { FedExPackagingType.Box10Kg, PackagingType.FEDEX_10KG_BOX },
                    { FedExPackagingType.Box25Kg, PackagingType.FEDEX_25KG_BOX },
                    { FedExPackagingType.Custom, PackagingType.YOUR_PACKAGING },
                    { FedExPackagingType.Envelope, PackagingType.FEDEX_ENVELOPE },
                    { FedExPackagingType.Pak, PackagingType.FEDEX_PAK },
                    { FedExPackagingType.Tube, PackagingType.FEDEX_TUBE },
                    { FedExPackagingType.SmallBox, PackagingType.FEDEX_SMALL_BOX },
                    { FedExPackagingType.MediumBox, PackagingType.FEDEX_MEDIUM_BOX },
                    { FedExPackagingType.LargeBox, PackagingType.FEDEX_LARGE_BOX },
                    { FedExPackagingType.ExtraLargeBox, PackagingType.FEDEX_EXTRA_LARGE_BOX },
                });

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Add the Packaging Type to the FedEx carrier request
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            request.RequestedShipment.PackagingType = GetApiPackagingType((FedExPackagingType) shipment.FedEx.PackagingType);
            request.RequestedShipment.PackagingTypeSpecified = true;

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        public void InitializeRequest(RateRequest request) =>
            request.Ensure(x => x.RequestedShipment);

        /// <summary>
        /// Determine the ship service packaging type
        /// </summary>
        private PackagingType GetApiPackagingType(FedExPackagingType packagingType)
        {
            if (packageTypeLookup.Value.ContainsKey(packagingType))
            {
                return packageTypeLookup.Value[packagingType];
            }

            throw new InvalidOperationException("Invalid FedEx Packaging Type");
        }
    }
}
