namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Interface to Get Postal Balance and Purchase Postage.
    /// </summary>
    public interface IPostageWebClient
    {
        /// <summary>
        /// Gets the shipment type code of the web client being used.
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Gets the value that will identify this account to the underlying provider (e.g. account number, username, etc.).
        /// </summary>
        string AccountIdentifier { get; }

        /// <summary>
        /// Gets the balance from the USPS postage provider.
        /// </summary>
        /// <returns>The available postage balance remaining.</returns>
        decimal GetBalance();

        /// <summary>
        /// Purchases additional postage based on the amount specified.
        /// </summary>
        /// <param name="amount">The amount.</param>
        void Purchase(decimal amount);
    }
}
