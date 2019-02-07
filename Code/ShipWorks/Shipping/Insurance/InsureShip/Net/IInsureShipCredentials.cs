namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Credentials for InsureShip
    /// </summary>
    public interface IInsureShipCredentials
    {
        /// <summary>
        /// Client ID
        /// </summary>
        string ClientID { get; }

        /// <summary>
        /// Api Key
        /// </summary>
        string ApiKey { get; }
    }
}