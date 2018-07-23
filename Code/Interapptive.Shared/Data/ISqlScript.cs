using System.Collections.Generic;
using System.Data.Common;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Represents a SQL script that can be executed
    /// </summary>
    public interface ISqlScript
    {
        /// <summary>
        /// Raised when a batch has successfully completed
        /// </summary>
        event SqlScriptBatchCompletedEventHandler BatchCompleted;

        /// <summary>
        /// The SQL that makes up the script
        /// </summary>
        string Content { get; }

        /// <summary>
        /// The ScriptContent broken down into batches that can be sent to SQL Server as individual commands
        /// </summary>
        IList<string> Batches { get; }

        /// <summary>
        /// Executes the script on the given connection
        /// </summary>
        void Execute(DbConnection connection);

        /// <summary>
        /// Executes the script on the given connection with the given transaction
        /// </summary>
        void Execute(DbCommand command);
    }
}