using ShipWorks.ApplicationCore.WindowsServices;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public interface IServiceExecutionStrategyFactory
    {
        /// <summary>
        /// Gets an execution strategy for a service.
        /// </summary>
        IServiceExecutionStrategy GetStrategyFor(ShipWorksServiceBase service);
    }
}
