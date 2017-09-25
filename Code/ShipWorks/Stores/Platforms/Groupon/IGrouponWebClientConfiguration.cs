namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Configuration for the Groupon web client
    /// </summary>
    public interface IGrouponWebClientConfiguration
    {
        /// <summary>
        /// Groupon API endpoint
        /// </summary>
        string Endpoint { get; }
    }
}