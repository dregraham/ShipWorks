using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

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
        ILocalRateValidationResult ValidateRecentShipments(IUpsAccountEntity account);


        /// <summary>
        /// Validates the local rate against the api rate for the most recent shipments for all accounts, return the first failure
        /// </summary>
        ILocalRateValidationResult ValidateRecentShipments();

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        void Snooze();
    }
}