namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Simple class for transferring non-global properties into\out off the options window.
    /// </summary>
    public class ShipWorksSettingsData
    {
        private bool showQatBelowRibbon;
        private bool minimizeRibbon;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksSettingsData(bool showQatBelowRibbon, bool minimizeRibbon)
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
