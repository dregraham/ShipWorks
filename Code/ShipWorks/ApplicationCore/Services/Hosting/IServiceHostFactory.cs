

namespace ShipWorks.ApplicationCore.Services.Hosting
{
    public interface IServiceHostFactory
    {
        /// <summary>
        /// Gets a service host for a service.
        /// </summary>
        IServiceHost GetHostFor(ShipWorksServiceBase service);
    }
}
