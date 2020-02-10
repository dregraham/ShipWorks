using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IApiPortRegistrationService portRegistrationService;
        private readonly ILog log;
        private const long DefaultPortNumber = 8081;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterApiPortCommandLineOption(IApiPortRegistrationService portRegistrationService, Func<Type, ILog> logFactory)
        {
            this.portRegistrationService = portRegistrationService;
            log = logFactory(typeof(RegisterApiPortCommandLineOption));
        }

        /// <summary>
        /// Name of this command
        /// </summary>
        public string CommandName => "registerapiport";

        /// <summary>
        /// Execute this command
        /// </summary>
        public Task Execute(List<string> args)
        {
            bool registrationSuccess = portRegistrationService.Register(DefaultPortNumber);

            if (!registrationSuccess)
            {
                log.Error($"Failed to register port {DefaultPortNumber} for the ShipWorks API");
            }

            return Task.CompletedTask;
        }
    }
}
