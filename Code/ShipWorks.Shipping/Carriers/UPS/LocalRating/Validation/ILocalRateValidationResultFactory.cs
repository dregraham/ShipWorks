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
    }
}