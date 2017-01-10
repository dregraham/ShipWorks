using System.ComponentModel;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Possible states of the scanner device
    /// </summary>
    public enum ScannerState
    {
        /// <summary>
        /// There is no scanner registered
        /// </summary>
        [Description("Not registered")]
        NotRegistered,

        /// <summary>
        /// There is a scanner registered, but it is not present
        /// </summary>
        [Description("Not attached")]
        Detached,

        /// <summary>
        /// There is a scanner registered, and it is present
        /// </summary>
        [Description("Ready to use")]
        Attached,
    }
}