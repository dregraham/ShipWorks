using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NDesk.Options;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Services.Hosting.Background;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;

namespace ShipWorks.ApplicationCore.Services.Installers
{
    /// <summary>
    /// Installs all ShipWorks services.  Providing the command line argument /cmd:installservices when
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class InstallServicesCommand : ICommandLineCommandHandler
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(InstallServicesCommand));

        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        public string CommandName
        {
            get { return "installservices"; }
        }

        /// <summary>
        /// Executes the command with the given options.
        /// If the options are not valid for the command, a CommandLineCommandArgumentException is thrown.
        /// </summary>
        public Task Execute(List<string> options)
        {
            IServiceRegistrar registrar = null;

            var parser = new OptionSet()
                .Add("w|windows", "Install as Windows services.", x =>
                {
                    registrar = new WindowsServiceRegistrar();
                })
                .Add("b|background", "Install as background services (run at user log-on).", x =>
                {
                    registrar = new BackgroundServiceRegistrar();
                });

            if (parser.Parse(options).Count > 0 || registrar == null)
            {
                var message = new StringBuilder()
                    .AppendFormat("The {0} command requires one of the following options:", CommandName).AppendLine();

                parser.WriteOptionDescriptions(message);

                throw new CommandLineCommandArgumentException(CommandName, string.Empty, message.ToString());
            }

            List<IServiceRegistrar> registrars = new List<IServiceRegistrar> {
                new WindowsServiceRegistrar(),
                new BackgroundServiceRegistrar()
            };

            // In case there was anything previously registered, unregister it
            registrars.ForEach(r => r.UnregisterAll());

            // Register the current one
            registrar.RegisterAll();

            return Task.CompletedTask;
        }
    }
}
