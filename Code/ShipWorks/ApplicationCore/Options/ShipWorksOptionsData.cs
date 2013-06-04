using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Simple class for transfering non-global properties into\out off the options window.
    /// </summary>
    public class ShipWorksOptionsData
    {
        bool showQatBelowRibbon;
        bool minimizeRibbon;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksOptionsData(bool showQatBelowRibbon, bool minimizeRibbon)
        {
            this.showQatBelowRibbon = showQatBelowRibbon;
            this.minimizeRibbon = minimizeRibbon;
        }

        /// <summary>
        /// Indicates that the Quick Access Toolbar is shown under the ribbon
        /// </summary>
        public bool ShowQatBelowRibbon
        {
            get { return showQatBelowRibbon; }
            set { showQatBelowRibbon = value; }
        }

        /// <summary>
        /// Indicates if the ribbon is show minimized
        /// </summary>
        public bool MinimizeRibbon
        {
            get { return minimizeRibbon; }
            set { minimizeRibbon = value; }
        }
    }
}
