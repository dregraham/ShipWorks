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

            if (new ShipWorksServiceManager(service).IsServiceInstalled())
                return new Windows.WindowsServiceHost(service);

            return new Background.BackgroundServiceHost(service);
        }
    }
}
