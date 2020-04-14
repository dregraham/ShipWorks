using System.Collections.Generic;
using System.Linq;
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
                ApiSettings settings = scope.Resolve<IApiSettingsRepository>().Load();
                long port;
                bool useHttps;

                if (args.Count != 2)
                {
                    port = settings.Port;
                    useHttps = settings.UseHttps;
                }
                else
                {
                    // Try to get port from args, fallback to settings
                    port = long.TryParse(args[0], out long portNumber) ?
                        portNumber :
                        settings.Port;

                    // Try to get useHttps from args, fallback to settings
                    useHttps = bool.TryParse(args[1], out bool https) ?
                        https :
                        settings.UseHttps;
                }

                bool registrationSuccess = scope.Resolve<IApiPortRegistrationService>().Register(port, useHttps);

                if (!registrationSuccess)
                {
                    log.Error($"Failed to register port {port} for the ShipWorks API");
                }

                return Task.CompletedTask;
            }
        }
    }
}
