using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using log4net;
using ShipWorks.Api;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for registering a port for the ShipWorks API
    /// </summary>
    public class RegisterApiPortCommandLineOption : ICommandLineCommandHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RegisterApiPortCommandLineOption));
        private const long DefaultPortNumber = 8081;

        /// <summary>
        /// Name of this command
        /// </summary>
        public string CommandName => "registerapiport";

        /// <summary>
        /// Execute this command
        /// </summary>
        public Task Execute(List<string> args)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                bool registrationSuccess =  scope.Resolve<IApiPortRegistrationService>().Register(DefaultPortNumber);

                if (!registrationSuccess)
                {
                    log.Error($"Failed to register port {DefaultPortNumber} for the ShipWorks API");
                }

                return Task.CompletedTask;
            }
        }
    }
}
