using ShipWorks.ApplicationCore.Services;
using System;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// The default host factory, which chooses the <see cref="WindowsServiceHost"/>
    /// if the service is registered in Windows, otherwise the <see cref="BackgroundServiceHost"/>.
    /// </summary>
    public class DefaultServiceHostFactory : IServiceHostFactory
    {
        public IServiceHost GetHostFor(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");

            if (new ShipWorksServiceManager(service).IsServiceInstalled())
                return new WindowsServiceHost(service);

            return new BackgroundServiceHost(service);
        }
    }
}
