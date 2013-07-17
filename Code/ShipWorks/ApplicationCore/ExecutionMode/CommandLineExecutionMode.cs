using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using log4net;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running on the command line.
    /// </summary>
    public class CommandLineExecutionMode : IExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandLineExecutionMode));

        readonly string commandName;
        readonly List<string> options;
        private readonly IExecutionModeInitializer initializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecutionMode"/> class.
        /// </summary>
        public CommandLineExecutionMode(string commandName, List<string> options)
            : this(commandName, options, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecutionMode" /> class.
        /// </summary>
        public CommandLineExecutionMode(string commandName, List<string> options, IExecutionModeInitializer initializer)
        {
            if (null == commandName)
                throw new ArgumentNullException("commandName");

            this.commandName = commandName;
            this.options = options ?? new List<string>();
            this.initializer = initializer ?? new CommandLineExecutionModeInitializer();
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInteractive
        {
            get { return false; }
        }

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run,
        /// ServiceBase.Run, etc.)
        /// </summary>
        public void Execute()
        {
            initializer.Initialize();

            log.InfoFormat("Running command '{0}'", commandName);

            // Instantiate the command line handlers from the assembly
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && !type.IsInterface && typeof(ICommandLineCommandHandler).IsAssignableFrom(type));
            List<ICommandLineCommandHandler> handlers = types.Select(Activator.CreateInstance).Cast<ICommandLineCommandHandler>().ToList();

            IGrouping<string, ICommandLineCommandHandler> duplicate = handlers.GroupBy(h => h.CommandName).SingleOrDefault(g => g.Count() > 1);
            if (duplicate != null)
            {
                throw new InvalidOperationException(string.Format("More than one command line handler with command name '{0}' was found.", duplicate.Key));
            }

            ICommandLineCommandHandler handler = handlers.SingleOrDefault(h => h.CommandName == commandName);
            if (handler == null)
            {
                string error = string.Format("'{0}' is not a valid command line command.", commandName);

                log.ErrorFormat(error);
                Console.Error.WriteLine(error);

                Environment.ExitCode = -1;
                return;
            }

            try
            {
                handler.Execute(options);
            }
            catch (CommandLineCommandArgumentException ex)
            {
                log.Error(ex.Message, ex);
                Console.Error.WriteLine(ex.Message);

                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public void HandleException(Exception exception)
        {
            if (UserSession.IsLoggedOn)
            {
                UserSession.Logoff(false);
            }
            UserSession.Reset();

            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);
            }
        }
    }
}
