using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Event delegate for the PrintActionCompleted event
    /// </summary>
    public delegate void PrintActionCompletedEventHandler(object sender, PrintActionCompletedEventArgs e);

    /// <summary>
    /// EventArgs for the PrintActionCompleted event
    /// </summary>
    public class PrintActionCompletedEventArgs : AsyncCompletedEventArgs
    {
        PrintAction action;

        /// <summary>
        /// Constrctor
        /// </summary>
        public PrintActionCompletedEventArgs(PrintAction action, Exception error, bool canceled, object userState)
            : base(error, canceled, userState)
        {
            this.action = action;
        }
        
        /// <summary>
        /// The action that was executed and is now completed.
        /// </summary>
        public PrintAction PrintAction
        {
            get { return action; }
        }
    }
}
