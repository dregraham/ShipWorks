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
        ILocalRateValidationResult Validate(IEnumerable<ShipmentEntity> shipments);

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        void Snooze();
    }
}