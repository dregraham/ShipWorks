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
        bool IsRunning { get; }
    }
}
