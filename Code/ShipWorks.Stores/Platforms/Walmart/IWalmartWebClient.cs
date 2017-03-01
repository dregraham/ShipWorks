namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Web client for interacting with Walmart
    /// </summary>
    public interface IWalmartWebClient
    {
        /// <summary>
        /// Tests the connection to Walmart
        /// </summary>
        bool TestConnection();
    }
}