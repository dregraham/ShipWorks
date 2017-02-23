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
    }
}