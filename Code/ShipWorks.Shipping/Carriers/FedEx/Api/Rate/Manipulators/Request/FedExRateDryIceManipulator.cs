using System;
using System.Linq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExRateDryIceManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateDryIceManipulator" /> class.
        /// </summary>
        public FedExRateDryIceManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateDryIceManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRateDryIceManipulator(FedExSettings fedExSettings)
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

            RateRequest nativeRequest = InitializeShipmentRequest(request);


            if (request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DryIceWeight > 0)
            {
                // Dry ice must be configured at the package level for all services (see comment below)
                ConfigurePackage(request, nativeRequest, currentPackageIndex);
            }

            // Lines below are commented out due to error responses coming back from FedEx
            // when trying to configure dry ice at the shipment level and was only working 
            // when configured at the package level. These were successful at one point 
            // when passing the certification tests???

            //if (request.ShipmentEntity.FedEx.Service != (int)FedExServiceType.FedExGround && request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DryIceWeight > 0)
            //{
            //    // Dry ice must be configured at the package level for any service other than ground 
            //    ConfigurePackage(request, nativeRequest, currentPackageIndex);
            //}

            //if (request.ShipmentEntity.FedEx.Service == (int)FedExServiceType.FedExGround && request.ShipmentEntity.FedEx.Packages.Any(p => p.DryIceWeight > 0))
            //{
            //    // Dry ice must be configured at the shipment level for ground service 
            //    ConfigureShipment(request, nativeRequest);
            //}
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

        ///// <summary>
        ///// Configures dry ice properties at the shipment level.
        ///// </summary>
        ///// <param name="request">The request.</param>
        ///// <param name="nativeRequest">The native request.</param>
        //private void ConfigureShipment(CarrierRequest request, RateRequest nativeRequest)
        //{
        //    ShipmentSpecialServiceType[] shipmentSpecialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
        //    Array.Resize(ref shipmentSpecialServiceTypes, shipmentSpecialServiceTypes.Length + 1);
        //    nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = shipmentSpecialServiceTypes;

        //    shipmentSpecialServiceTypes[shipmentSpecialServiceTypes.Length - 1] = ShipmentSpecialServiceType.DRY_ICE;

        //    nativeRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail = new ShipmentDryIceDetail
        //    {
        //        PackageCount = request.ShipmentEntity.FedEx.Packages.Count(p => p.DryIceWeight > 0).ToString(),
        //        TotalWeight = new Weight
        //        {
        //            Value = (decimal)(request.ShipmentEntity.FedEx.Packages.Sum(p => p.DryIceWeight) / 2.2046),
        //            ValueSpecified = true,
        //            Units = WeightUnits.KG,
        //            UnitsSpecified = true
        //        }
        //    };
        //}

        /// <summary>
        /// Configures dry ice properties at the package level.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="currentPackageIndex">Index of the current package.</param>
        private void ConfigurePackage(CarrierRequest request, RateRequest nativeRequest, int currentPackageIndex)
        {
            PackageSpecialServiceType[] packageSpecialServiceTypes = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes;
            Array.Resize(ref packageSpecialServiceTypes, packageSpecialServiceTypes.Length + 1);

            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = packageSpecialServiceTypes;
            packageSpecialServiceTypes[packageSpecialServiceTypes.Length - 1] = PackageSpecialServiceType.DRY_ICE;

            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight = new Weight
            {
                Value = (decimal) (request.ShipmentEntity.FedEx.Packages[currentPackageIndex].DryIceWeight/2.2046),
                ValueSpecified = true,
                Units = WeightUnits.KG,
                UnitsSpecified = true
            };
        }

        private RateRequest InitializeShipmentRequest(CarrierRequest request)
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
