using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Factory for creating message filters
    /// </summary>
    [Service]
    public interface IScannerMessageFilterFactory
    {
        /// <summary>
        /// Create a scanner message filter
        /// </summary>
        IScannerMessageFilter CreateMessageFilter();
    }
}
