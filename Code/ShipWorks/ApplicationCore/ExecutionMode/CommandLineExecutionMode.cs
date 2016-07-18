using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running on the command line.
    /// </summary>
    public class CommandLineExecutionMode : ExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CommandLineExecutionMode));

        readonly string commandName;
        readonly List<string> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExecutionMode"/> class.
        /// </summary>
        public CommandLineExecutionMode(string commandName, List<string> options)
        {
            if (null == commandName)
            {
                throw new ArgumentNullException("commandName");
            }

            this.commandName = commandName;
            this.options = options ?? new List<string>();
        }

        /// <summary>
        /// Name of this execution mode (User interface, command line, service)
        /// </summary>
        public override string Name
        {
            get
            {
                return "CommandLineExecutionMode";
            }
        }

        /// <summary>
        /// Indicates if this execution mode supports displaying a UI, whether or not one is currently displayed or not
        /// </summary>
        public override bool IsUISupported
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsUIDisplayed
        {
            get { return false; }
        }

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run,
        /// ServiceBase.Run, etc.)
        /// </summary>
        public override void Execute()
        {
            Initialize();

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

                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        public override Task HandleException(Exception exception, bool guiThread, string userEmail)
        {
            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);

                if (SqlSession.IsConfigured)
                {
                    log.Fatal(SqlUtility.GetRunningSqlCommands(SqlSession.Current.Configuration.GetConnectionString()));
                }
            }

            return TaskEx.FromResult(true);
        }
    }
}
