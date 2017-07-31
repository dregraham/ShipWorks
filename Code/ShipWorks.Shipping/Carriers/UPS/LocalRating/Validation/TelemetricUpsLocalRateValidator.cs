using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Decorator for the UpsLocalRateValidator that is responsible for logging telemetry
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation.IUpsLocalRateValidator" />
    public class TelemetricUpsLocalRateValidator : IUpsLocalRateValidator
    {
        private readonly IUpsLocalRateValidator rateValidator;
        private readonly Func<string, ITrackedEvent> telemetryEventFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetricUpsLocalRateValidator"/> class.
        /// </summary>
        /// <param name="rateValidator">The rate validator.</param>
        /// <param name="telemetryEventFunc">The telemetry event function.</param>
        public TelemetricUpsLocalRateValidator(IUpsLocalRateValidator rateValidator,
            Func<string, ITrackedEvent> telemetryEventFunc)
        {
            this.rateValidator = rateValidator;
            this.telemetryEventFunc = telemetryEventFunc;
        }

        /// <summary>
        /// Given a list of processed UPS shipments, if applicable, validate the local rates match the rate charged by UPS
        /// </summary>
        public ILocalRateValidationResult ValidateShipments(IEnumerable<ShipmentEntity> shipments)
        {
            ILocalRateValidationResult result = rateValidator.ValidateShipments(shipments);

            LogTelemetry(result);

            return result;
        }

        /// <summary>
        /// Validates the local rate against the shipment cost for the most recent shipments for the given account
        /// </summary>
        public ILocalRateValidationResult ValidateRecentShipments(IUpsAccountEntity account)
        {
            return rateValidator.ValidateRecentShipments(account);
        }

        /// <summary>
        /// Validates the local rate against the shipment cost for the most recent shipments for all accounts
        /// </summary>
        public ILocalRateValidationResult ValidateRecentShipments()
        {
            return rateValidator.ValidateRecentShipments();
        }

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        public void Snooze()
        {
            rateValidator.Snooze();
        }

        /// <summary>
        /// Logs the telemetry.
        /// </summary>
        private void LogTelemetry(ILocalRateValidationResult result)
        {
            foreach (ShipmentEntity shipment in result.ValidatedShipments)
            {
                UpsLocalRateDiscrepancy discrepancyForShipment =
                    result.RateDiscrepancies.SingleOrDefault(r => r.Shipment.ShipmentID == shipment.ShipmentID);

                bool isAccurate = discrepancyForShipment == null;

                using (ITrackedEvent telemetryEvent = telemetryEventFunc("Shipping.Rating.Ups.LocalRating.Accuracy"))
                {
                    telemetryEvent.AddProperty("Rate.IsAccurate", isAccurate.ToString());

                    if (!isAccurate)
                    {
                        telemetryEvent.AddProperty("Rate.Local.Amount",
                            discrepancyForShipment.LocalRate?.Amount.ToString(CultureInfo.InvariantCulture) ?? "Not found");
                        telemetryEvent.AddProperty("Rate.Local.Zone", discrepancyForShipment.LocalRate?.Zone ?? "Not found");
                        telemetryEvent.AddProperty("Rate.Actual.Amount",
                            shipment.ShipmentCost.ToString(CultureInfo.InvariantCulture));
                        telemetryEvent.AddProperty("Shipment.ServiceType",
                            EnumHelper.GetDescription((UpsServiceType) shipment.Ups.Service));
                        telemetryEvent.AddProperty("Shipment.Origin.StateProvince", shipment.OriginStateProvCode);
                        telemetryEvent.AddProperty("Shipment.Origin.PostalCode", shipment.OriginPostalCode);
                        telemetryEvent.AddProperty("Shipment.Destination.StateProvince", shipment.ShipStateProvCode);
                        telemetryEvent.AddProperty("Shipment.Destination.PostalCode", shipment.OriginPostalCode);

                        for (int i = 0; i < shipment.Ups.Packages.Count; i++)
                        {
                            LogPackageTelemetry(shipment, i, telemetryEvent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Logs the package telemetry.
        /// </summary>
        private void LogPackageTelemetry(ShipmentEntity shipment, int i, ITrackedEvent telemetryEvent)
        {
            UpsPackageEntity package = shipment.Ups.Packages[i];

            telemetryEvent.AddProperty($"Shipment.RatedWeight.{i}",
                package.BillableWeight.ToString(CultureInfo.InvariantCulture));
            telemetryEvent.AddProperty($"Shipment.Dimensions.Width.{i}",
                package.DimsWidth.ToString(CultureInfo.InvariantCulture));
            telemetryEvent.AddProperty($"Shipment.Dimensions.Length.{i}",
                package.DimsLength.ToString(CultureInfo.InvariantCulture));
            telemetryEvent.AddProperty($"Shipment.Dimensions.Height.{i}",
                package.DimsHeight.ToString(CultureInfo.InvariantCulture));
        }
    }
}