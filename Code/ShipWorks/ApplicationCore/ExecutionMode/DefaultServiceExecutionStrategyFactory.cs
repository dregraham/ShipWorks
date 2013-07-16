using ShipWorks.ApplicationCore.WindowsServices;
using System;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// The default strategy factory, which chooses the Windows strategy
    /// if the service is registered in Windows, otherwise the Background strategy.
    /// </summary>
    public class DefaultServiceExecutionStrategyFactory : IServiceExecutionStrategyFactory
    {
        public IServiceExecutionStrategy GetStrategyFor(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");

            if (new ShipWorksServiceManager(service).IsServiceInstalled())
                return new WindowsServiceExecutionStrategy(service);

            return new BackgroundServiceExecutionStrategy(service);
        }
    }
}
