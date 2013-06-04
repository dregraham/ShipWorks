using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email
{
    /// <summary>
    /// Event handler for the EmailCommunicationComplete event
    /// </summary>
    public delegate void EmailCommunicationCompleteEventHandler(object sender, EmailCommunicationCompleteEventArgs e);

    /// <summary>
    /// EventArgs for the EmailCommunicationComplete event
    /// </summary>
    public class EmailCommunicationCompleteEventArgs : EventArgs
    {
        bool hasErrors;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailCommunicationCompleteEventArgs(bool hasErrors)
        {
            this.hasErrors = hasErrors;
        }

        /// <summary>
        /// Indicates if any errors occurred during download
        /// </summary>
        public bool HasErrors
        {
            get { return hasErrors; }
        }
     }
}
