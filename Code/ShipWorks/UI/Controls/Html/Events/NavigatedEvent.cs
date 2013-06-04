using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// Delegate for the Navigated event
    /// </summary>
    public delegate void NavigatedEventHandler(object sender, NavigatedEventArgs e);

    /// <summary>
    /// Event args for the Navigated event
    /// </summary>
    public class NavigatedEventArgs
    {
        string target;

        /// <summary>
        /// Constructor
        /// </summary>
        public NavigatedEventArgs(string target)
        {
            this.target = target;
        }

        /// <summary>
        /// The URL target being navigated to
        /// </summary>
        public string Target
        {
            get { return target; }
        }
    }
}
