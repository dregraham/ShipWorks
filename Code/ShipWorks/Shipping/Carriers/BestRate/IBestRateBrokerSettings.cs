namespace ShipWorks.Shipping.Carriers.BestRate
{
    public interface IBestRateBrokerSettings
    {
        /// <summary>
        /// Determines whether a broker should check the Express1 rates.
        /// </summary>
        bool CheckExpress1Rates(ShipmentType shipmentType);

        /// <summary>
        /// Determines whether MailInnovations is available.
        /// </summary>
        bool IsMailInnovationsAvailable(ShipmentType shipmentType);

        /// <summary>
        /// Determines whether a customer [can use sure post].
        /// </summary>
        bool CanUseSurePost();
    }
}