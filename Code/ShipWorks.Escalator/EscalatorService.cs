using System.ServiceProcess;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Service to start the Escalator
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class EscalatorService : ServiceBase
    {
        private readonly Escalator escalator;

        /// <summary>
        /// Constructor
        /// </summary>
        public EscalatorService(Escalator escalator, IServiceName serviceName)
        {
            ServiceName = serviceName.Resolve();
            this.escalator = escalator;
        }

        /// <summary>
        /// Code that runs when the service starts
        /// </summary>
        protected override void OnStart(string[] args)
        {
            escalator.OnStart();
        }

        /// <summary>
        /// Code that runs when the service stops
        /// </summary>
        protected override void OnStop()
        {
            // Do nothing
        }
    }
}
