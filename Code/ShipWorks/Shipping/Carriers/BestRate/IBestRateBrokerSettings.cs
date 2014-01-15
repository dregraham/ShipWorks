namespace ShipWorks.Shipping.Carriers.BestRate
{
    public interface IBestRateBrokerSettings
    {
        bool CheckExpress1Rates(ShipmentType shipmentType);
        bool IsMailInnovationsAvailable(ShipmentType shipmentType);
    }
}