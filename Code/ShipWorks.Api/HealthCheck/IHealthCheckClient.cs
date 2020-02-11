namespace ShipWorks.Api.HealthCheck
{
    public interface IHealthCheckClient
    {
        bool IsRunning();
    }
}