using System;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
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
        public IExecutionMode Create()
        {
            IExecutionMode executionMode = null;

            if (!Environment.UserInteractive)
            {
                // TODO: Fill in the service execution mode when it is available
                // executionMode = new ServiceExecutionMode(...);

                throw new NotImplementedException();
            }
            else if (commandLine.IsCommandSpecified)
            {
                // TODO: Fill in the command line execution mode when it is available
                // executionMode = new CommandLineExecutionMode(...);

                throw new NotImplementedException();
            }
            else
            {
                executionMode = new UserInterfaceExecutionMode(commandLine);
            }

            return executionMode;
        }
    }
}
