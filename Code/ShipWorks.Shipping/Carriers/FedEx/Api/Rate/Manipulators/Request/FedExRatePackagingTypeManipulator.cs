using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRatePackagingTypeManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Add the Packaging Type to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Get the FedEx shipment
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            // Set the packaging type for the shipment
            nativeRequest.RequestedShipment.PackagingType = GetApiPackagingType((FedExPackagingType) fedExShipment.PackagingType);
            nativeRequest.RequestedShipment.PackagingTypeSpecified = true;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        public void InitializeRequest(CarrierRequest request)
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
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }

        /// <summary>
        /// Determine the ship service packaging type
        /// </summary>
        private PackagingType GetApiPackagingType(FedExPackagingType packagingType)
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
