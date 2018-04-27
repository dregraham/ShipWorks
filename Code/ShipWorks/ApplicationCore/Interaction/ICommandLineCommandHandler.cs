using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Can be implemented by classes to signal that they handle command line commands
    /// </summary>
    public interface ICommandLineCommandHandler
    {
        /// <summary>
        /// The name of the command handled by the handler.  Must be unique within the application
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Execute the command with the given arguments.  If the arguments are not valid for the command,
        /// a CommandLineCommandException is thrown.
        /// </summary>
        Task Execute(List<string> args);
    }
}
