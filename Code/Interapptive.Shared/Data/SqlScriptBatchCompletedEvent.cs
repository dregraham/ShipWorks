using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Delegate for the SqlScriptBatchCompletedEvent
    /// </summary>
    public delegate void SqlScriptBatchCompletedEventHandler(object sender, SqlScriptBatchCompletedEventArgs args);

    /// <summary>
    /// EventArgs for the SqlScriptBatchCompletedEventHandler
    /// </summary>
    public class SqlScriptBatchCompletedEventArgs : EventArgs
    {
        int batch;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlScriptBatchCompletedEventArgs(int batch)
        {
            this.batch = batch;
        }

        /// <summary>
        /// The batch number that was just completed
        /// </summary>
        public int Batch
        {
            get { return batch; }
        }
    }
}
