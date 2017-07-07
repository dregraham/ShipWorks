using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    public interface ILocalRateValidationResultFactory
    {
        /// <summary>
        /// Creates a LocalRateValidationResult
        /// </summary>
        ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, IEnumerable<ShipmentEntity> shipments, Action snooze);

        /// <summary>
        /// Creates a LocalRateValidationResult with the specified rate discrepancies.
        /// </summary>
        /// <param name="rateDiscrepancies">The rate discrepancies.</param>
        /// <param name="shipments"></param>
        ILocalRateValidationResult Create(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, IEnumerable<ShipmentEntity> shipments);
    }
}