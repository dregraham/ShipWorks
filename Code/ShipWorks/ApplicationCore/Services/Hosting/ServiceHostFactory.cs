using ShipWorks.ApplicationCore.Services.Hosting.Background;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;
using System;

namespace ShipWorks.ApplicationCore.Services.Hosting
{
    /// <summary>
    /// The default host factory, which chooses the <see cref="WindowsServiceHost"/>
    /// if the service is registered in Windows, otherwise the <see cref="BackgroundServiceHost"/>.
    /// </summary>
    public static class ServiceHostFactory
    {
        /// <summary>
        /// Create the correct host based on if the Windows service is installed or not
        /// </summary>
        /// <param name="service"></param>
        public static IServiceHost GetHostFor(ShipWorksServiceBase service)
        {
            if (null == service)
            {
                throw new ArgumentNullException("service");
            }

            if (new WindowsServiceController(service).IsServiceInstalled())
            {
                return new WindowsServiceHost(service);
            }
            else
            {
                return new BackgroundServiceHost(service);
            }
        }
    }
}
