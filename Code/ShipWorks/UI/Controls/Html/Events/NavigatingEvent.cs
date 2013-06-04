using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.UI.Controls.Html
{
    public delegate void NavigatingEventHandler(object sender, NavigatingEventArgs e);

    /// <summary>
    /// Used for the Navigating Event
    /// </summary>
    public class NavigatingEventArgs : CancelEventArgs
    {
        string target;
        string newTarget;

        /// <summary>
        /// Constructor
        /// </summary>
        public NavigatingEventArgs(string target)
        {
            this.target = target;
            this.target = target;
        }

        /// <summary>
        /// Gets the URL that will be navigated to.
        /// </summary>
        [Description("Gets the URL that will be navigated to.")]
        public string Target
        {
            get { return target; }
        }

        /// <summary>
        /// Gets/Sets the revised URL that will be used to navigate.
        /// </summary>
        [Description("Gets/Sets the revised URL that will be used to navigate.")]
        public string NewTarget
        {
            get { return newTarget; }
            set { newTarget = value; }
        }
    }
}
