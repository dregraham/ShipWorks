using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// EventArgs for the PageShown event
    /// </summary>
    public class WizardPageShownEventArgs : EventArgs
    {
        bool firstTime;

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardPageShownEventArgs(bool firstTime)
        {
            this.firstTime = firstTime;
        }

        /// <summary>
        /// Indicates if this is the first time the page is being shown
        /// </summary>
        public bool FirstTime
        {
            get { return firstTime; }
        }
    }
}
