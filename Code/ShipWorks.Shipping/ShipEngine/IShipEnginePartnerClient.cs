namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    public interface IShipEnginePartnerClient
    {
        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        string CreateNewAccount(string partnerApiKey);

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        string GetApiKey(string partnerApiKey, string shipEngineAccountId);        
    }
}