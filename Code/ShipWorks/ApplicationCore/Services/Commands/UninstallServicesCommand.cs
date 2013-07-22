using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;
using System.Collections.Generic;


namespace ShipWorks.ApplicationCore.Services.Installers
{
    /// <summary>
    /// Uninstalls all ShipWorks services.  Providing the command line argument /cmd:uninstallservices when 
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class UninstallServicesCommand : ICommandLineCommandHandler
    {
        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        public string CommandName
        {
            get { return "uninstallservices"; }
        }

        /// <summary>
        /// Executes the command with the given options.
        /// If the options are not valid for the command, a CommandLineCommandException is thrown.
        /// </summary>
        public void Execute(List<string> options)
        {
            new WindowsServiceRegistrar().UnregisterAll();
        }
    }
}
