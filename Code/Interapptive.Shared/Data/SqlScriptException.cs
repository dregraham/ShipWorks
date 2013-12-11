using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Exception that is thrown when an error occurs while processing a SQL Script
    /// </summary>
    public class SqlScriptException : Exception
    {
        // The name of the script in which the error occurred
        string scriptName = "";

        // The batch number within the script in which the error occurred.
        int batch;

        // The failure happened during execution
        bool executionException = false;

        /// <summary>
        /// Indicates if script details (if available) will be shown
        /// </summary>
        bool showScriptDetails = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlScriptException(string name, int batch, Exception inner)
            : base(inner.Message, inner)
        {
            this.scriptName = name;
            this.batch = batch;
            this.executionException = true;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlScriptException(string name, Exception inner)
            : base(inner.Message, inner)
        {
            this.scriptName = name;
            this.executionException = true;
        }

        /// <summary>
        /// Constructore for creation a non-execution failure
        /// </summary>
        public SqlScriptException(string message)
            : base(message)
        {
            executionException = false;
        }

        /// <summary>
        /// Indicates if script details (if available) will be shown
        /// </summary>
        public bool ShowScriptDetails
        {
            get { return showScriptDetails; }
            set { showScriptDetails = value; }
        }

        /// <summary>
        /// Format the message of the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                if (executionException && showScriptDetails)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append(base.Message + "\n\n");

                    if (!string.IsNullOrEmpty(ScriptName))
                    {
                        message.AppendFormat("Script: {0}\n", ScriptName);
                    }

                    message.AppendFormat("Batch: {0}", Batch);

                    SqlException ex = InnerException as SqlException;
                    if (ex != null)
                    {
                        message.Append("\nLine: " + ex.LineNumber);
                    }

                    return message.ToString();
                }
                else
                {
                    return base.Message;
                }
            }
        }

        /// <summary>
        /// The name of the script in which the error occurred.
        /// </summary>
        public string ScriptName
        {
            get
            {
                return scriptName;
            }
        }

        /// <summary>
        /// The batch number within the script in which the error occurred.
        /// </summary>
        public int Batch
        {
            get { return batch; }
        }
    }
}