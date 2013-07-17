using ShipWorks.ApplicationCore.WindowsServices;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public interface IServiceHostFactory
    {
        /// <summary>
        /// Gets a service host for a service.
        /// </summary>
        IServiceHost GetHostFor(ShipWorksServiceBase service);
    }
}
