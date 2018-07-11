using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;
using static Interapptive.Shared.Utility.Functional;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Represents a SQL script that can be executed
    /// </summary>
    public class SqlScriptWithMetadata : ISqlScript
    {
        private readonly ISqlScript script;
        private readonly XDocument metadata;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlScriptWithMetadata(ISqlScript script, string metadataText)
        {
            this.script = script;
            script.BatchCompleted += OnScriptBatchCompleted;
            metadata = XDocument.Parse(metadataText);
        }

        /// <summary>
        /// The SQL that makes up the script
        /// </summary>
        public string Content => script.Content;

        /// <summary>
        /// The ScriptContent broken down into batches that can be sent to SQL Server as individual commands
        /// </summary>
        public IList<string> Batches => script.Batches;

        /// <summary>
        /// Raised when a batch has successfully completed
        /// </summary>
        public event SqlScriptBatchCompletedEventHandler BatchCompleted;

        /// <summary>
        /// Executes the script on the given connection
        /// </summary>
        public void Execute(DbConnection connection) =>
            Execute(() => script.Execute(connection));

        /// <summary>
        /// Executes the script on the given command
        /// </summary>
        public void Execute(DbCommand command) =>
            Execute(() => script.Execute(command));

        /// <summary>
        /// Execute the given action
        /// </summary>
        public void Execute(Action func)
        {
            try
            {
                func();
            }
            catch (SqlScriptException ex)
            {
                if (IsRequired())
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Is the script required
        /// </summary>
        private bool IsRequired() =>
            ParseBool(metadata.Document.Element("required")?.Value).Match(x => x, _ => true);

        /// <summary>
        /// Handle the batch completed event
        /// </summary>
        private void OnScriptBatchCompleted(object sender, SqlScriptBatchCompletedEventArgs args) =>
            BatchCompleted?.Invoke(sender, args);
    }
}