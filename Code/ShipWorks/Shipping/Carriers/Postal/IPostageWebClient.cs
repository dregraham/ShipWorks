namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Interface to Get Postal Balance and Purchase Postage.
    /// </summary>
    public interface IPostageWebClient
    {
        ShipmentTypeCode ShipmentTypeCode { get; }
        string AccountIdentifier { get; }

        double GetBalance();
        void Purchase(double amount);
    }
}
