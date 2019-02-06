namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Simple class for transferring non-global properties into\out off the options window.
    /// </summary>
    public class ShipWorksSettingsData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksSettingsData(bool showQatBelowRibbon, bool minimizeRibbon)
        {
            ShowQatBelowRibbon = showQatBelowRibbon;
            MinimizeRibbon = minimizeRibbon;
        }

        /// <summary>
        /// Indicates that the Quick Access Toolbar is shown under the ribbon
        /// </summary>
        public bool ShowQatBelowRibbon { get; set; }

        /// <summary>
        /// Indicates if the ribbon is show minimized
        /// </summary>
        public bool MinimizeRibbon { get; set; }
    }
}
