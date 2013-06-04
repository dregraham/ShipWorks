using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Data that is used during the execution of a MenuCommand
    /// </summary>
    public class MenuCommandExecutionContext
    {
        MenuCommand command;

        // The top-level form which the command is invoked
        Control owner;

        // The primary keys of the rows selected in the grid when the event is invoked
        IEnumerable<long> selectedKeys;

        // The callback to call when the command completes
        MenuCommandCompleteEventHandler asyncCallback;

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuCommandExecutionContext(MenuCommand command, Control owner, IEnumerable<long> selectedKeys, MenuCommandCompleteEventHandler callback)
        {
            this.command = command;
            this.owner = owner;
            this.selectedKeys = selectedKeys;
            this.asyncCallback = callback;
        }

        /// <summary>
        /// The command that is being executed which caused these args to be created
        /// </summary>
        public MenuCommand MenuCommand
        {
            get { return command; }
        }

        /// <summary>
        /// The top-level form which the command is invoked
        /// </summary>
        public Control Owner
        {
            get { return owner; }
        }

        /// <summary>
        /// The primary keys of the rows selected in the grid when the event is invoked
        /// </summary>
        public IEnumerable<long> SelectedKeys
        {
            get { return selectedKeys; }
        }

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        public void Complete()
        {
            Complete(MenuCommandResult.Success, string.Empty);
        }

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        public void Complete(MenuCommandResult result, string message)
        {
            Debug.Assert(!owner.InvokeRequired, "Complete should be called on the UI thread.");

            if (asyncCallback != null)
            {
                asyncCallback(command, new MenuCommandCompleteEventArgs(result, message));
            }
        }

        /// <summary>
        /// Must be called when the command has finished executing.  If there are no issues in the list, Complete is assumed success.  If there
        /// are issues, Complete is called with an inteligent display of the issue messages handled using the specified severity.
        /// </summary>
        public void Complete<T>(List<BackgroundIssue<T>> issues, MenuCommandResult issueSeverity)
        {
            if (issues.Count > 0)
            {
                List<string> distinctErrors = issues.Select(i => ((Exception) i.Detail).Message).Distinct().ToList();

                if (distinctErrors.Count == 1)
                {
                    Complete(issueSeverity, distinctErrors[0]);
                }
                else
                {
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
            else
            {
                Complete();
            }
        }
    }
}
