namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    public interface ILocalRateValidationResultFactory
    {
        ILocalRateValidationResult Create(int shipmentCount, int discrepancyCount);
    }
}