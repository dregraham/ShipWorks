namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Permissions for AutoPrint
    /// </summary>
    public interface IAutoPrintPermissions
    {
        /// <summary>
        /// Whether or not auto print is permitted in the current state
        /// </summary>
        bool AutoPrintPermitted();

        /// <summary>
        /// Whether or not auto weigh is turned on
        /// </summary>
        bool AutoWeighOn();
    }
}