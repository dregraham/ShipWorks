using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Responsible for top-level processing of the ShipWorks command line
    /// </summary>
    public class ShipWorksCommandLine
    {
        // Any program options specified.  These come before the /command switch.  Right
        // now we don't support any, but if they do, here's where to find them.
        List<string> programOptions = new List<string>();

        // The service and service arguments specified.  Optional
        string serviceName;
        List<string> serviceOptions = new List<string>();

        // The command and command arguments specified.  Optional
        string commandName;
        List<string> commandOptions = new List<string>();

        private string recoveryAttempts;

        /// <summary>
        /// Parse the given command line arguments into a command line object
        /// </summary>
        public static ShipWorksCommandLine Parse(IEnumerable<string> args)
        {
            return new ShipWorksCommandLine(args);
        }

        /// <summary>
        /// An empty command line.
        /// </summary>
        public static readonly ShipWorksCommandLine Empty = new ShipWorksCommandLine(new string[0]);

        /// <summary>
        /// Can't instantiate publically
        /// </summary>
        private ShipWorksCommandLine(IEnumerable<string> args)
        {
            OptionSet options = new OptionSet();

            options.Add("recovery=", v =>
            {
                recoveryAttempts = v;
                options.Remove("recovery");
            });

            options.Add("s|service=", v => { serviceName = v; options.Remove("s"); options.Remove("c"); });
            options.Add("c|cmd|command=", v => { commandName = v; options.Remove("s"); options.Remove("c"); });

            options.Add("<>", v =>
            {
                if (IsServiceSpecified)
                    serviceOptions.Add(v);
                else if (IsCommandSpecified)
                    commandOptions.Add(v);
                else
                    programOptions.Add(v);
            });

            options.Parse(args);
        }

        /// <summary>
        /// Indicates if a service was specified and ShipWorks should run a service.
        /// </summary>
        public bool IsServiceSpecified
        {
            get { return !string.IsNullOrEmpty(serviceName); }
        }

        /// <summary>
        /// The ShipWorks service name passed on the command line, if any.
        /// </summary>
        public string ServiceName
        {
            get { return serviceName; }
        }

        /// <summary>
        /// The service options passed on the command line.
        /// </summary>
        public List<string> ServiceOptions
        {
            get { return serviceOptions; }
        }

        /// <summary>
        /// Gets the number of attempts to recover from a service crash have been made.
        /// </summary>
        public int RecoveryAttempts
        {
            get 
            { 
                // This is done here instead of at command line constructor to avoid 
                // crashing if a non-integer was provided.
                int attemptNumber;
                if (!int.TryParse(recoveryAttempts, out attemptNumber))
                {
                    attemptNumber = 0;
                }

                return attemptNumber;
            }
        }

        /// <summary>
        /// Indicates if a command was specified and ShipWorks should run on the command line
        /// </summary>
        public bool IsCommandSpecified
        {
            get { return !string.IsNullOrEmpty(commandName); }
        }

        /// <summary>
        /// The ShipWorks command passed on the command line if any
        /// </summary>
        public string CommandName
        {
            get { return commandName; }
        }

        /// <summary>
        /// The CommandOptions passed on the command line.
        /// </summary>
        public List<string> CommandOptions
        {
            get { return commandOptions; }
        }

        /// <summary>
        /// The options passed on the command line that were BEFORE any /command options, and unrelated to any specific shipworks commands
        /// </summary>
        public List<string> ProgramOptions
        {
            get { return programOptions; }
        }
    }
}
