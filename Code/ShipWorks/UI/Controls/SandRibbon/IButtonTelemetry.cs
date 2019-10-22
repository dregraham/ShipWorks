namespace ShipWorks.UI.Controls.SandRibbon
{
    /// <summary>
    /// Interface for button telemetry
    /// </summary>
    public interface IButtonTelemetry
    {
        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        string TelemetryEventName { get; set; }
    }
}
