

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Registers services to be run via a service host.
    /// </summary>
    public interface IServiceRegistrar
    {
        /// <summary>
        /// Registers all services.
        /// </summary>
        void RegisterAll();

        /// <summary>
        /// Unregisters all services.
        /// </summary>
        void UnregisterAll();
    }
}
