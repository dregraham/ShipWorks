namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    public interface ISingleScanAutomationSettings
    {
        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        bool IsAutoWeighEnabled { get; }

        /// <summary>
        /// Should shipments be auto created
        /// </summary>
        bool AutoCreateShipments { get; }

        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        bool IsAutoPrintEnabled();
    }
}