using System;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Add package details for the shipping request
    /// </summary>
    public class FedExPackageDetailsManipulator : IFedExShipRequestManipulator
    {
        private string shipmentCurrencyType;
        readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageDetailsManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            !FedExUtility.IsFreightLtlService(shipment.FedEx.Service);

        /// <summary>
        /// Add package information such as Count, weight, insured value and dimensions to packages and shipment
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeShipmentRequest(request);
            var fedex = shipment.FedEx;

            shipmentCurrencyType = FedExSettings.GetCurrencyTypeApiValue(shipment, () => settings.GetAccountReadOnly(shipment));

            RequestedShipment shipDetail = request.RequestedShipment;
            shipDetail.PackageCount = fedex.Packages.Count().ToString();

            var package = fedex.Packages.ElementAt(sequenceNumber);

            // Each package should be in it's own request, so we always use the first item in the line item array
            RequestedPackageLineItem packageRequest = request.RequestedShipment.RequestedPackageLineItems[0];
            packageRequest.SequenceNumber = (sequenceNumber + 1).ToString();

            // Package weight and value
            packageRequest.Weight = new Weight { Units = GetApiWeightUnit(shipment), Value = FedExUtility.GetPackageTotalWeight(package) };
            packageRequest.InsuredValue = new Money { Amount = package.DeclaredValue, Currency = shipmentCurrencyType };

            // If custom, add dimensions
            if (fedex.PackagingType == (int) FedExPackagingType.Custom)
            {
                packageRequest.Dimensions = new Dimensions
                {
                    Units = GetApiLinearUnit(shipment),

                    Length = Math.Round(package.DimsLength, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                    Height = Math.Round(package.DimsHeight, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture),
                    Width = Math.Round(package.DimsWidth, MidpointRounding.AwayFromZero).ToString("0", CultureInfo.InvariantCulture)
                };

                //TODO: make this something other than test.
                packageRequest.ItemDescription = "test";
            }

            return request;
        }

        /// <summary>
        /// Initializes request ensuring CarrierRequest is a IFedExNativeShipmentRequest and has
        /// required object initialized
        /// </summary>
        private void InitializeShipmentRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems);

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
