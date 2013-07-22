using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;
using System.Collections.Generic;


namespace ShipWorks.ApplicationCore.Services.Installers
{
    /// <summary>
    /// Installs all ShipWorks services.  Providing the command line argument /cmd:installservices when
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class InstallServicesCommand : ICommandLineCommandHandler
    {
        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        public string CommandName
        {
            get { return "installservices"; }
        }

        /// <summary>
        /// Executes the command with the given options.
        /// If the options are not valid for the command, a CommandLineCommandException is thrown.
        /// </summary>
        public void Execute(List<string> options)
        {
            new WindowsServiceRegistrar().RegisterAll();
        }
    }
}
