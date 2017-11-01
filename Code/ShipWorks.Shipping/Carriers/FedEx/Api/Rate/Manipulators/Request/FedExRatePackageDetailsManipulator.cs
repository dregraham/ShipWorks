using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the attributes of each package of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePackageDetailsManipulator : IFedExRateRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRatePackageDetailsManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            var packageCount = shipment.FedEx.Packages.Count();
            request.RequestedShipment.PackageCount = packageCount.ToString();

            request.RequestedShipment.RequestedPackageLineItems = shipment.FedEx
                .Packages
                .Zip(RequestPackageList(request, packageCount), Tuple.Create)
                .Select((x, i) => BuildPackageDetails(shipment, x.Item1, x.Item2 ?? CreateLineItem(), i))
                .ToArray();

            return request;
        }

        /// <summary>
        /// Get a list of request packages, padding any needed packages with nulls
        /// </summary>
        private static IEnumerable<RequestedPackageLineItem> RequestPackageList(RateRequest request, int packageCount) =>
            request.RequestedShipment.RequestedPackageLineItems.Concat(Enumerable.Repeat<RequestedPackageLineItem>(null, packageCount));

        /// <summary>
        /// Create a new line item
        /// </summary>
        private RequestedPackageLineItem CreateLineItem() => new RequestedPackageLineItem();

        /// <summary>
        /// Build package details
        /// </summary>
        private RequestedPackageLineItem BuildPackageDetails(
            IShipmentEntity shipment,
            IFedExPackageEntity fedExPackage,
            RequestedPackageLineItem packageRequest,
            int packageIndex)
        {
            packageRequest.SequenceNumber = (packageIndex + 1).ToString();
            packageRequest.GroupPackageCount = "1";

            // Package weight and value (default the weight to .1 if none is given, so we can still get rates)
            decimal packageTotalWeight = FedExUtility.GetPackageTotalWeight(fedExPackage);
            packageTotalWeight = packageTotalWeight > 0 ? packageTotalWeight : .1m;

            packageRequest.Weight = BuildWeight(shipment, packageTotalWeight);
            packageRequest.InsuredValue = BuildMoney(shipment, fedExPackage);

            // If custom, add dimensions
            if (shipment.FedEx.PackagingType == (int) FedExPackagingType.Custom)
            {
                packageRequest.Dimensions = BuildDimensions(shipment, fedExPackage);
            }

            return packageRequest;
        }

        /// <summary>
        /// Build Money element
        /// </summary>
        private Money BuildMoney(IShipmentEntity shipment, IFedExPackageEntity fedExPackage)
        {
            return new Money
            {
                Amount = fedExPackage.DeclaredValue,
                AmountSpecified = true,
                Currency = FedExSettings.GetCurrencyTypeApiValue(shipment, () => settings.GetAccountReadOnly(shipment))
            };
        }

        /// <summary>
        /// Build weight element
        /// </summary>
        private Weight BuildWeight(IShipmentEntity shipment, decimal packageTotalWeight)
        {
            return new Weight
            {
                Units = GetApiWeightUnit(shipment),
                UnitsSpecified = true,
                Value = packageTotalWeight,
                ValueSpecified = true
            };
        }

        /// <summary>
        /// Build Dimensions element
        /// </summary>
        public Dimensions BuildDimensions(IShipmentEntity shipment, IFedExPackageEntity package)
        {
            return new Dimensions
            {
                Units = GetApiLinearUnit(shipment),
                UnitsSpecified = true,
                Length = Math.Round(package.DimsLength, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                Height = Math.Round(package.DimsHeight, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                Width = Math.Round(package.DimsWidth, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture)
            };
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems);
        }

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        private WeightUnits GetApiWeightUnit(IShipmentEntity shipment) =>
            shipment.FedEx.WeightUnitType == (int) WeightUnitOfMeasure.Kilograms ?
                WeightUnits.KG :
                WeightUnits.LB;

        /// <summary>
        /// Gets the FedEx API linear unit.
        /// </summary>
        private LinearUnits GetApiLinearUnit(IShipmentEntity shipment) =>
            shipment.FedEx.LinearUnitType == (int) FedExLinearUnitOfMeasure.CM ?
                LinearUnits.CM :
                LinearUnits.IN;
    }
}
