using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Delegate for the BrowserPrintingError event
    /// </summary>
    public delegate void BrowserErrorEventHandler(object sender, BrowserErrorEventArgs e);

    /// <summary>
    /// EventArgs for the BrowserPrintingError event
    /// </summary>
    public class BrowserErrorEventArgs : EventArgs
    {
        string message;
        string line;

        /// <summary>
        /// Constructor
        /// </summary>
        public BrowserErrorEventArgs(string message, string line)
        {
            this.message = message;
            this.line = line;
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// The line number on which the error occurred
        /// </summary>
        public string Line
        {
            get { return line; }
        }
    }
}
