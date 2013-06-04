using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// Defines the possible states of the control.
    /// </summary>
    public enum HtmlReadyState
    {
        /// <summary>Document is Uninitialized or not created</summary>
        UnInitialized,
        /// <summary>Document is loading</summary>
        Loading,
        /// <summary>Document has finished loading</summary>
        Loaded,
        /// <summary>Document is in interactive mode (some elements have not yet finished loading)</summary>
        Interactive,
        /// <summary>Document is Complete</summary>
        Complete
    }
}
