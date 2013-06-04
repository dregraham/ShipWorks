using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// Delegate for the ReadyStateChanged event
    /// </summary>
    public delegate void ReadyStateChangedEventHandler(object sender, ReadyStateChangedEventArgs e);

    /// <summary>
    /// EventArgs for the ReadySTateChanged event
    /// </summary>
    public class ReadyStateChangedEventArgs : EventArgs
    {
        HtmlReadyState readyState;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReadyStateChangedEventArgs(HtmlReadyState readyState)
        {
            this.readyState = readyState;
        }

        /// <summary>
        /// The ReadyState of the document
        /// </summary>
        public HtmlReadyState ReadyState
        {
            get { return readyState; }
        }
    }
}
