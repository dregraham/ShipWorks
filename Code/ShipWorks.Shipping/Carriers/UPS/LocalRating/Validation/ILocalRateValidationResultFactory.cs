using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    public interface ILocalRateValidationResultFactory
    {
        /// <summary>
        /// Creates a LocalRateValidationResult
        /// </summary>
        ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, int totalShipmentValidated, Action snooze);

        /// <summary>
        /// Creates a LocalRateValidationResult with the specified rate discrepancies.
        /// </summary>
        /// <param name="rateDiscrepancies">The rate discrepancies.</param>
        ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies);
    }
}