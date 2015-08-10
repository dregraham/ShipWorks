using System;
using System.Globalization;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the attributes of each package of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePackageDetailsManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRatePackageDetailsManipulator" /> class.
        /// </summary>
        public FedExRatePackageDetailsManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRatePackageDetailsManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRatePackageDetailsManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        { }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            FedExShipmentEntity fedex = request.ShipmentEntity.FedEx;
            nativeRequest.RequestedShipment.PackageCount = fedex.Packages.Count.ToString();

            // All of the package details are sent to to FedEx in a single call instead of doing multiple
            // calls like the processing a shipment does
            for (int packageIndex = 0; packageIndex < fedex.Packages.Count; packageIndex++)
            {
                FedExPackageEntity fedExPackage = fedex.Packages[packageIndex];

                // We can't guarantee that all the line items have been added, so we initialize the line item
                // to make sure there is a valid object reference at the current index of the line item array
                // before attempting to access the item in the array
                InitializeLineItem(nativeRequest, packageIndex);
                RequestedPackageLineItem packageRequest = nativeRequest.RequestedShipment.RequestedPackageLineItems[packageIndex];

                packageRequest.SequenceNumber = (packageIndex + 1).ToString();
                packageRequest.GroupPackageCount = "1";

                // Package weight and value (default the weight to .1 if none is given, so we can still get rates)
                decimal packageTotalWeight = FedExUtility.GetPackageTotalWeight(fedExPackage);
                packageTotalWeight = packageTotalWeight > 0 ? packageTotalWeight : .1m;

                packageRequest.Weight = new Weight
                {
                    Units = GetApiWeightUnit(request.ShipmentEntity),
                    UnitsSpecified = true,
                    Value = packageTotalWeight,
                    ValueSpecified = true
                };

                packageRequest.InsuredValue = new Money
                {
                    Amount = fedExPackage.DeclaredValue, 
                    AmountSpecified = true,
                    Currency = GetShipmentCurrencyType(request.ShipmentEntity)
                };

                // If custom, add dimensions
                if (fedex.PackagingType == (int)FedExPackagingType.Custom)
                {
                    packageRequest.Dimensions = new Dimensions
                    {
                        Units = GetApiLinearUnit(request.ShipmentEntity),
                        UnitsSpecified = true,
                        Length = Math.Round(fedExPackage.DimsLength, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                        Height = Math.Round(fedExPackage.DimsHeight, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                        Width = Math.Round(fedExPackage.DimsWidth, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture)
                    };
                }
            }
        }

        /// <summary>
        /// Initializes the line item.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="lineItemIndex">Index of the line item.</param>
        private static void InitializeLineItem(RateRequest nativeRequest, int lineItemIndex)
        {
            if (nativeRequest.RequestedShipment.RequestedPackageLineItems.Length <= lineItemIndex)
            {
                // We need to resize the line item array to accommodate the index
                RequestedPackageLineItem[] packageArray = nativeRequest.RequestedShipment.RequestedPackageLineItems;
                Array.Resize(ref packageArray, lineItemIndex + 1);

                nativeRequest.RequestedShipment.RequestedPackageLineItems = packageArray;
            }

            if (nativeRequest.RequestedShipment.RequestedPackageLineItems[lineItemIndex] == null)
            {
                // We need to create a new package line item
                nativeRequest.RequestedShipment.RequestedPackageLineItems[lineItemIndex] = new RequestedPackageLineItem();
            }
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

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
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
        }

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx WeightUnits value.</returns>
        private WeightUnits GetApiWeightUnit(ShipmentEntity shipment)
        {
            WeightUnits weightUnits = WeightUnits.LB;

            if (shipment.FedEx.WeightUnitType == (int)WeightUnitOfMeasure.Kilograms)
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
