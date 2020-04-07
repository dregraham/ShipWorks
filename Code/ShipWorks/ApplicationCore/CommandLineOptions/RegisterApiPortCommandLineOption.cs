using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using log4net;
using ShipWorks.Api;
using ShipWorks.Api.Configuration;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for registering a port for the ShipWorks API
    /// </summary>
    public class RegisterApiPortCommandLineOption : ICommandLineCommandHandler
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RegisterApiPortCommandLineOption));

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
                long port = scope.Resolve<IApiSettingsRepository>().Load().Port;
                bool registrationSuccess =  scope.Resolve<IApiPortRegistrationService>().Register(port);

                if (!registrationSuccess)
                {
                    log.Error($"Failed to register port {port} for the ShipWorks API");
                }

                return Task.CompletedTask;
            }
        }
    }
}
