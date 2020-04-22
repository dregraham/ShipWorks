namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Calls HealthCheck Endpoint
    /// </summary>
    public interface IHealthCheckClient
    {
        /// <summary>
        /// Returns true if running, else false
        /// </summary>
        bool IsRunning(long portNumber, bool useHttps);
    }
}
