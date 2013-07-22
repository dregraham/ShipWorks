using log4net;


namespace ShipWorks.ApplicationCore.Services.Hosting.Background
{
    public class BackgroundServiceRegistrar : IServiceRegistrar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceRegistrar));

        public void RegisterAll()
        {
            log.Info("Registering all services as background processes.");
        }

        public void UnregisterAll()
        {
            log.Info("Unregistering all background processes.");
        }
    }
}
