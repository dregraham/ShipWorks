using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Data that is used during the execution of a MenuCommand
    /// </summary>
    public class MenuCommandExecutionContext : IMenuCommandExecutionContext
    {
        // The callback to call when the command completes
        MenuCommandCompleteEventHandler asyncCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommandExecutionContext(IMenuCommand command, Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
        {
            MenuCommand = command;
            Owner = owner;
            SelectedKeys = selectedKeys;
            this.asyncCallback = callback;
        }

        /// <summary>
        /// The command that is being executed which caused these args to be created
        /// </summary>
        public IMenuCommand MenuCommand { get; }

        /// <summary>
        /// The top-level form which the command is invoked
        /// </summary>
        public Control Owner { get; }

        /// <summary>
        /// The primary keys of the rows selected in the grid when the event is invoked
        /// </summary>
        public IEnumerable<long> SelectedKeys { get; }

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        public void Complete() =>
            Complete(MenuCommandResult.Success, string.Empty);

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        public void Complete(MenuCommandResult result, string message)
        {
            Debug.Assert(!Owner.InvokeRequired, "Complete should be called on the UI thread.");

            asyncCallback?.Invoke(MenuCommand, new MenuCommandCompleteEventArgs(result, message));
        }

        /// <summary>
        /// Must be called when the command has finished executing.  If there are no issues in the list, Complete is assumed success.  If there
        /// are issues, Complete is called with an intelligent display of the issue messages handled using the specified severity.
        /// </summary>
        public void Complete<T>(List<BackgroundIssue<T>> issues, MenuCommandResult issueSeverity) =>
            Complete(issues.Select(i => i.Detail).Cast<Exception>(), issueSeverity);

        /// <summary>
        /// Must be called when the command has finished executing.  If there are no issues in the list, Complete is assumed success.  If there
        /// are issues, Complete is called with an intelligent display of the issue messages handled using the specified severity.
        /// </summary>
        public void Complete(IEnumerable<Exception> issues, MenuCommandResult issueSeverity)
        {
            if (issues.None())
            {
                Complete();
                return;
            }

            List<string> distinctErrors = issues.Select(x => x.Message)
                .Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();

            if (distinctErrors.Count == 1)
            {
                Complete(issueSeverity, distinctErrors[0]);
                return;
            }

            string message = "Some errors occurred while processing your request:\n";

            foreach (string error in distinctErrors.Take(5))
            {
                message += string.Format("\n{0}", error);
            }

            if (distinctErrors.Count > 5)
            {
                message += "\n\nSee the log for the full list of errors.";
            }

            Complete(issueSeverity, message);
        }
    }
}
