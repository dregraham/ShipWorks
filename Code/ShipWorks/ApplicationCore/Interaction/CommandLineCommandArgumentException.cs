using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Exception thrown when an argument to a command line command is not valid
    /// </summary>
    public class CommandLineCommandArgumentException : Exception
    {
        string commandName;
        string argumentName;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandLineCommandArgumentException(string commandName, string argumentName, string message)
            : base(message)
        {
            this.commandName = commandName;
            this.argumentName = argumentName;
        }

        /// <summary>
        /// The name of the command
        /// </summary>
        public string CommandName
        {
            get { return commandName; }
        }

        /// <summary>
        /// The argument name that was in error
        /// </summary>
        public string ArgumentName
        {
            get { return argumentName; }
        }
    }
}
