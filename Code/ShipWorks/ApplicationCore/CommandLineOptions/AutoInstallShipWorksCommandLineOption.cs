using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.AutoInstall;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for creating a new database and schema, creating a user, etc...
    /// </summary>
    public class AutoInstallShipWorksCommandLineOption : ICommandLineCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoInstallShipWorksCommandLineOption));

        /// <summary>
        /// The CommandName that can be sent to ShipWorks.exe
        /// </summary>
        public string CommandName => "autoinstallsw";

        /// <summary>
        /// Execute the command
        /// </summary>
        public async Task Execute(List<string> args)
        {
            AutoInstaller autoInstaller = new AutoInstaller();
            await autoInstaller.Execute(args).ConfigureAwait(false);
        }
    }
}
