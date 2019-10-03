using Divelements.SandRibbon;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Microsoft.ApplicationInsights.DataContracts;

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
            TrackButtonClick();
            return base.OnActivate(e);
        }

        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        public string TelemetryEventName { get; set; }

        /// <summary>
        /// Track any telemetry
        /// </summary>
        public void TrackButtonClick(string postfix = "")
        {
            postfix = postfix.IsNullOrWhiteSpace() ? postfix : $".{postfix}".Replace(" ", string.Empty);
            Telemetry.TrackEvent(new EventTelemetry($"ShipWorks.Button.Click.{TelemetryEventName}{postfix}"));
        }
    }
}
