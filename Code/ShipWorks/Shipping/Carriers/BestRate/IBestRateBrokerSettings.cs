namespace ShipWorks.Shipping.Carriers.BestRate
{
    public interface IBestRateBrokerSettings
    {
        /// <summary>
        /// </summary>
        bool CheckExpress1Rates(ShipmentType shipmentType);

        /// <summary>
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <exception cref="System.ArgumentException">shipmentType should be UPS type</exception>
        bool IsMailInnovationsAvailable(ShipmentType shipmentType);

        /// <summary>
        /// Determines whether a customer [can use sure post].
        /// </summary>
        bool CanUseSurePost();

        /// <summary>
        /// Determines if endicia DHL is enabled.
        /// </summary>
        bool IsEndiciaDHLEnabled();

        /// <summary>
        /// Determines if Consolidator enabled.
        /// </summary>
        bool IsEndiciaConsolidatorEnabled();
    }
}