using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ShipWorks.ApplicationCore.Interaction;
using log4net;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExectionMode interface intended to be used when running on the command line.
    /// </summary>
    public class CommandLineExecutionMode : IExecutionMode
    {
        private readonly ILog log;
        private readonly ShipWorksCommandLine commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecutionMode"/> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        public CommandLineExecutionMode(ShipWorksCommandLine commandLine)
            : this(commandLine, LogManager.GetLogger(typeof(CommandLineExecutionMode)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecutionMode"/> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <param name="log">The log.</param>
        public CommandLineExecutionMode(ShipWorksCommandLine commandLine, ILog log)
        {
            this.commandLine = commandLine;
            this.log = log;
        }

        /// <summary>
        /// Gets or sets the command line.
        /// </summary>
        /// <value>The command line.</value>
        public ShipWorksCommandLine CommandLine
        {
            get { return commandLine; }
        }


        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsUserInteractive()
        {
            return false;
        }

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run,
        /// ServiceBase.Run, etc.)
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Execute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
