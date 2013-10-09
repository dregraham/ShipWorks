﻿using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExDryIceManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExDryIceManipulator" /> class.
        /// </summary>
        public FedExDryIceManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExDryIceManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExDryIceManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            Validate(request);

            int currentPackageIndex = request.SequenceNumber;

            IFedExNativeShipmentRequest nativeRequest = InitializeShipmentRequest(request);

            if (request.ShipmentEntity.FedEx.Service != (int)FedExServiceType.FedExGround && request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DryIceWeight > 0)
            {
                // Dry ice must be configured at the package level for any service other than ground 
                ConfigurePackage(request, nativeRequest, currentPackageIndex);
            }

            if (request.ShipmentEntity.FedEx.Service == (int) FedExServiceType.FedExGround && request.ShipmentEntity.FedEx.Packages.Any(p => p.DryIceWeight > 0))
            {
                // Dry ice must be configured at the shipment level for ground service 
                ConfigureShipment(request, nativeRequest);
            }
        }


        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static void Validate(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.ShipmentEntity.FedEx.PackagingType != (int) FedExPackagingType.Custom && request.ShipmentEntity.FedEx.Packages.Any(p => p.DryIceWeight > 0))
            {
                throw new FedExException("Standard FedEx packaging cannot be used to ship packages containing dry ice. You must use your own packaging for shipments containing dry ice.");
            }
        }

        /// <summary>
        /// Configures dry ice properties at the shipment level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nativeRequest">The native request.</param>
        private void ConfigureShipment(CarrierRequest request, IFedExNativeShipmentRequest nativeRequest)
        {
            ShipmentSpecialServiceType[] shipmentSpecialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Array.Resize(ref shipmentSpecialServiceTypes, shipmentSpecialServiceTypes.Length + 1);
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes;

            shipmentSpecialServiceTypes[shipmentSpecialServiceTypes.Length - 1] = ShipmentSpecialServiceType.DRY_ICE;

            nativeRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail = new ShipmentDryIceDetail
            {
                PackageCount = request.ShipmentEntity.FedEx.Packages.Count(p => p.DryIceWeight > 0).ToString(),
                TotalWeight = new Weight
                {
                    Value = (decimal)(request.ShipmentEntity.FedEx.Packages.Sum(p => p.DryIceWeight) / 2.2046),
                    Units = WeightUnits.KG
                }
            };
        }

        /// <summary>
        /// Configures dry ice properties at the package level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="currentPackageIndex">Index of the current package.</param>
        private void ConfigurePackage(CarrierRequest request, IFedExNativeShipmentRequest nativeRequest, int currentPackageIndex)
        {
            PackageSpecialServiceType[] packageSpecialServiceTypes = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes;
            Array.Resize(ref packageSpecialServiceTypes, packageSpecialServiceTypes.Length + 1);

            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = packageSpecialServiceTypes;
            packageSpecialServiceTypes[packageSpecialServiceTypes.Length - 1] = PackageSpecialServiceType.DRY_ICE;

            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight = new Weight
            {
                Value = (decimal) (request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DryIceWeight/2.2046),
                Units = WeightUnits.KG
            };
        }

        private IFedExNativeShipmentRequest InitializeShipmentRequest(CarrierRequest request)
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

            // Make sure package is there           
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new[]
                {
                    new RequestedPackageLineItem()
                };
            }

            RequestedPackageLineItem packageLineItem = nativeRequest.RequestedShipment.RequestedPackageLineItems[0];

            if (packageLineItem.SpecialServicesRequested == null)
            {
                packageLineItem.SpecialServicesRequested = new PackageSpecialServicesRequested();
            }

            // Add PackageSpecialServiceType
            if (packageLineItem.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                packageLineItem.SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[0];
            }

            // Add ShipmentSpecialServicesRequested
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            // Add ShipemtnSpecialServiceType
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            return nativeRequest;
        }
    }
}
