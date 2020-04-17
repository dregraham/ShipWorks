using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                if (args.Count == 2)
                {
                    // Try to get port from args, fallback to settings
                    if (long.TryParse(args[0], out long portNumber))
                    {
                        settings.Port = portNumber;
                    }

                    // Try to get useHttps from args, fallback to settings
                    if (bool.TryParse(args[1], out bool https))
                    {
                        settings.UseHttps = https;
                    }
                }

                bool registrationSuccess;
                try
                {
                    registrationSuccess = scope.Resolve<IApiPortRegistrationService>().Register(settings);
                }
                catch (Exception ex)
                {
                    log.Error("Error registering port", ex);
                    registrationSuccess = false;
                }                

                if (!registrationSuccess)
                {
                    var errorMessage = $"Failed to register port {settings.Port} for the ShipWorks API";
                    log.Error(errorMessage);
                    Environment.ExitCode = -1;
                }

                return Task.CompletedTask;
            }
        }
    }
}
