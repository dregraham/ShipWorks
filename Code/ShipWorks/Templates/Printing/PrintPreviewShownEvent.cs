using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Delegate for the PreviewShown event
    /// </summary>
    public delegate void PrintPreviewShownEventHandler(object sender, PrintPreviewShownEventArgs e);

    /// <summary>
    /// EventArgs for the PreviewShown event
    /// </summary>
    public class PrintPreviewShownEventArgs : EventArgs
    {
        object userState;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintPreviewShownEventArgs(object userState)
        {
            this.userState = userState;
        }

        /// <summary>
        /// Stateful object provided by the consumer
        /// </summary>
        public object UserState
        {
            get { return userState; }
        }
    }
}
