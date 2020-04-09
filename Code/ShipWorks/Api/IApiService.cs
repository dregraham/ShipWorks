namespace ShipWorks.Api
{
    /// <summary>
    /// Interface for ApiService
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Is the service running
        /// </summary>
        ApiStatus Status { get; }

        /// <summary>
        /// The port the service is currently running on
        /// </summary>
        long? Port { get; }
    }
}
