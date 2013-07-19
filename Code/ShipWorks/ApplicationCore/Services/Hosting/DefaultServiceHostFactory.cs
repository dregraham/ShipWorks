using ShipWorks.ApplicationCore.Services.Hosting.Background;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;
using System;


namespace ShipWorks.ApplicationCore.Services.Hosting
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

            if (new WindowsServiceController(service).IsServiceInstalled())
                return new WindowsServiceHost(service);

            return new BackgroundServiceHost(service);
        }
    }
}
