using Divelements.SandRibbon;
using Interapptive.Shared.Metrics;
using MenuItem = Divelements.SandRibbon.MenuItem;

namespace ShipWorks.UI.Controls.SandRibbon
{
    /// <summary>
    /// Interface implementation of the SandRibbon MenuItem
    /// </summary>
    public class RibbonMenuItem : MenuItem, IButtonTelemetry
    {
        /// <summary>
        /// On click event handler
        /// </summary>
        protected override bool OnActivate(ActivateEventArgs e)
        {
            Telemetry.TrackButtonClick(TelemetryEventName, string.Empty);
            return base.OnActivate(e);
        }

        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        public string TelemetryEventName { get; set; }
    }
}
