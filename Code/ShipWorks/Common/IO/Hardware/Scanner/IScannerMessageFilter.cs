using System.Windows.Forms;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Message filter used by scanner service
    /// </summary>
    public interface IScannerMessageFilter : IMessageFilter
    {
        /// <summary>
        /// Disable ScannerMessageFilter
        /// </summary>
        void Disable();

        /// <summary>
        /// Enable filter used by scanner service
        /// </summary>
        void Enable();
    }
}