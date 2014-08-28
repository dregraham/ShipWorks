namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Interface to Get Postal Balance and Purchase Postage.
    /// </summary>
    public interface IPostageWebClient
    {
        ShipmentTypeCode ShipmentTypeCode { get; }
        string AccountIdentifier { get; }

        decimal GetBalance();
        void Purchase(decimal amount);
    }
}
