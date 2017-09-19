using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Data that is used during the execution of a MenuCommand
    /// </summary>
    public interface IMenuCommandExecutionContext
    {
        /// <summary>
        /// The command that is being executed which caused these args to be created
        /// </summary>
        IMenuCommand MenuCommand { get; }

        /// <summary>
        /// The top-level form which the command is invoked
        /// </summary>
        Control Owner { get; }

        /// <summary>
        /// The primary keys of the rows selected in the grid when the event is invoked
        /// </summary>
        IEnumerable<long> SelectedKeys { get; }

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        void Complete();

        /// <summary>
        /// Must be called when the command has finished executing
        /// </summary>
        void Complete(MenuCommandResult result, string message);

        /// <summary>
        /// Must be called when the command has finished executing.  If there are no issues in the list, Complete is assumed success.  If there
        /// are issues, Complete is called with an intelligent display of the issue messages handled using the specified severity.
        /// </summary>
        void Complete<T>(List<BackgroundIssue<T>> issues, MenuCommandResult issueSeverity);

        /// <summary>
        /// Must be called when the command has finished executing.  If there are no issues in the list, Complete is assumed success.  If there
        /// are issues, Complete is called with an intelligent display of the issue messages handled using the specified severity.
        /// </summary>
        void Complete(IEnumerable<Exception> issues, MenuCommandResult issueSeverity);
    }
}