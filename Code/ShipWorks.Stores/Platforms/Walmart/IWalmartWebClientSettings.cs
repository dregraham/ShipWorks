namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Interface for handling Walmart API Related Settings
    /// </summary>
    public interface IWalmartWebClientSettings
    {
        /// <summary>
        /// Root endpoint
        /// </summary>
        string Endpoint { get; }
    }
}
