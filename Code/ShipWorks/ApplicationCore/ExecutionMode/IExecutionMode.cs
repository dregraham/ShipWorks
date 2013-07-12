using System;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An abstraction for handling ShipWorks running in different execution contexts (e.g. running
    /// as a Windows service, with the full UI, as a background process, etc.).
    /// </summary>
    public interface IExecutionMode
    {
        /// <summary>
        /// Determines whether the exection mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        bool IsUserInteractive();

        /// <summary>
        /// Executes ShipWorks within the context of a specific execution mode (e.g. Application.Run, 
        /// ServiceBase.Run, etc.)
        /// </summary>
        void Execute();

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from 
        /// the stack. This would be where things like showing a crash report dialog, would take place 
        /// just before the app terminates. 
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        void HandleException(Exception exception);
    }
}
