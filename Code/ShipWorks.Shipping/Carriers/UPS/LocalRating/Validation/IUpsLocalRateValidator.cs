using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Validates the rate charged by UPS matches the calculated local rate
    /// </summary>
    public interface IUpsLocalRateValidator
    {
        /// <summary>
        /// Given a list of processed UPS shipments, if applicable, validate the local rates match the rate charged by UPS
        /// </summary>
        ILocalRateValidationResult ValidateShipments(IEnumerable<ShipmentEntity> shipments);

        /// <summary>
        /// Validates the local rate against the the shipment cost for the most recent shipments for the given account
        /// </summary>
        ILocalRateValidationResult ValidateRecentShipments(UpsAccountEntity account);

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        void Snooze();
    }
}