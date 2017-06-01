using System;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    public interface ILocalRateValidationResultFactory
    {
        /// <summary>
        /// Creates a LocalRateValidationResult
        /// </summary>
        ILocalRateValidationResult Create(int totalShipmentsValidated, int shipmentsWithRateDiscrepancies, Action snooze);
    }
}