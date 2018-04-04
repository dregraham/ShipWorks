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
    /// Uninstalls all ShipWorks services.  Providing the command line argument /cmd:uninstallservices when 
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class UninstallServicesCommand : ICommandLineCommandHandler
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(UninstallServicesCommand));

        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        public string CommandName
        {
            get { return "uninstallservices"; }
        }

        /// <summary>
        /// Executes the command with the given options.
        /// If the options are not valid for the command, a CommandLineCommandArgumentException is thrown.
        /// </summary>
        public Task Execute(List<string> options)
        {
            var registrars = new List<IServiceRegistrar>();

            var parser = new OptionSet()
                .Add("w|windows", "Uninstall Windows services.", x =>
                {
                    registrars.Add(new WindowsServiceRegistrar());
                })
                .Add("b|background", "Uninstall background services.", x =>
                {
                    registrars.Add(new BackgroundServiceRegistrar());
                })
                .Add("all", "Uninstall all services.", x =>
                {
                    registrars.Add(new WindowsServiceRegistrar());
                    registrars.Add(new BackgroundServiceRegistrar());
                });

            if (parser.Parse(options).Count > 0 || registrars.Count == 0)
            {
                var message = new StringBuilder()
                    .AppendFormat("The {0} command requires one of the following options:", CommandName).AppendLine();

                parser.WriteOptionDescriptions(message);

                throw new CommandLineCommandArgumentException(CommandName, string.Empty, message.ToString());
            }

            registrars.ForEach(x => x.UnregisterAll());

            return Task.CompletedTask;
        }
    }
}
