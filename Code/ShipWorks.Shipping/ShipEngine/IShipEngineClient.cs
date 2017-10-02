namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Web client for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineClient
    {
        /// <summary>
        /// Connects the given DHL account to the users ShipEngine account
        /// </summary>
        string ConnectDhlAccount(string accountNumber);
    }
}