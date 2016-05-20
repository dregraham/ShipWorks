using System;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Factory that analyzes the command line and generates the proper execution mode
    /// </summary>
    public class ExecutionModeFactory
    {
        private readonly ShipWorksCommandLine commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionModeFactory"/> class.
        /// </summary>
        public ExecutionModeFactory(ShipWorksCommandLine commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException("commandLine");
            }

            this.commandLine = commandLine;
        }

        /// <summary>
        /// Creates an IExecutionMode instance based on the command line provided in the constructor.
        /// </summary>
        /// <returns>An IExecutionMode object for running ShipWorks as a windows service, from the command line,
        /// as a background process, or through the normal UI.</returns>
        public ExecutionMode Create()
        {
            if (commandLine.IsServiceSpecified)
            {
                return new ServiceExecutionMode(commandLine.ServiceName, commandLine.ServiceOptions);
            }

            if (commandLine.IsCommandSpecified)
            {
                return new CommandLineExecutionMode(commandLine.CommandName, commandLine.CommandOptions);
            }

            return new UserInterfaceExecutionMode(commandLine.ProgramOptions);
        }
    }
}
