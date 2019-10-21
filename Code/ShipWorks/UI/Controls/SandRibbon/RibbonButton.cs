using Divelements.SandRibbon;
using Interapptive.Shared.Metrics;

namespace ShipWorks.UI.Controls.SandRibbon
{
    /// <summary>
    /// Interface implementation of the SandRibbon button
    /// </summary>
    /// <remarks>The purpose of this is to help test methods that use these buttons</remarks>
    public class RibbonButton : Button, IRibbonButton
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
        /// On activate dropdown event handler
        /// </summary>
        protected override WidgetBase OnActivateDropdown(bool keyboard)
        {
            Telemetry.TrackButtonClick(TelemetryEventName, string.Empty);
            return base.OnActivateDropdown(keyboard);
        }

        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        public string TelemetryEventName { get; set; }
    }
}
