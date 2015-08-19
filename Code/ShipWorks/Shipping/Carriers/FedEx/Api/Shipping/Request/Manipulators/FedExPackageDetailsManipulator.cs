using System;
using System.Globalization;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPackageDetailsManipulator : FedExShippingRequestManipulatorBase
    {
        private string shipmentCurrencyType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageDetailsManipulator" /> class.
        /// </summary>
        public FedExPackageDetailsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPackageDetailsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExPackageDetailsManipulator(FedExSettings fedExSettings) : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add package information such as Count, weight, insured value and dimensions to packages and shipment
        /// </summary>
        public override void Manipulate(CarrierRequest request)
        {
            IFedExNativeShipmentRequest nativeRequest = InitializeShipmentRequest(request);
            FedExShipmentEntity fedex = request.ShipmentEntity.FedEx;

            shipmentCurrencyType = GetShipmentCurrencyType(fedex.Shipment);

            RequestedShipment shipDetail = nativeRequest.RequestedShipment;
            shipDetail.PackageCount = fedex.Packages.Count.ToString();

            int currentPackageIndex = request.SequenceNumber;
            FedExPackageEntity package = fedex.Packages[currentPackageIndex];
            
            // Each package should be in it's own request, so we always use the first item in the line item aray
            RequestedPackageLineItem packageRequest = nativeRequest.RequestedShipment.RequestedPackageLineItems[0];
            packageRequest.SequenceNumber = (currentPackageIndex + 1).ToString();

            // Package weight and value
            packageRequest.Weight = new Weight {Units = GetApiWeightUnit(request.ShipmentEntity), Value = FedExUtility.GetPackageTotalWeight(package)};
            packageRequest.InsuredValue = new Money { Amount = package.DeclaredValue, Currency = shipmentCurrencyType };

            // If custom, add dimensions
            if (fedex.PackagingType == (int) FedExPackagingType.Custom)
            {
                packageRequest.Dimensions = new Dimensions
                {
                    Units = GetApiLinearUnit(request.ShipmentEntity),

                    Length = Math.Round(package.DimsLength, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                    Height = Math.Round(package.DimsHeight, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                    Width = Math.Round(package.DimsWidth, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture)
                };

                // todo: make this something other than test.
                packageRequest.ItemDescription = "test";
            }
        }

        /// <summary>
        /// Initializes nativeRequest ensuring CarrierRequest is a IFedExNativeShipmentRequest and has
        /// required object initialized
        /// </summary>
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

            //Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            //Make sure all packages are there
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems == null)
            {
                nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                nativeRequest.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            }

            return nativeRequest;
        }

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx WeightUnits value.</returns>
        private WeightUnits GetApiWeightUnit(ShipmentEntity shipment)
        {
            WeightUnits weightUnits = WeightUnits.LB;
            
            if (shipment.FedEx.WeightUnitType == (int) WeightUnitOfMeasure.Kilograms)
            {
                weightUnits = WeightUnits.KG;
            }

            return weightUnits;
        }

        /// <summary>
        /// Gets the FedEx API linear unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx LinearUnits value.</returns>
        private LinearUnits GetApiLinearUnit(ShipmentEntity shipment)
        {
            LinearUnits linearUnits = LinearUnits.IN;

            if (shipment.FedEx.LinearUnitType == (int)FedExLinearUnitOfMeasure.CM)
            {
                linearUnits = LinearUnits.CM;
            }

            return linearUnits;
        }
    }
}
