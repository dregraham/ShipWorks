using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common.IO.Hardware.Scanner
{
    /// <summary>
    /// Message filter used by scanner service
    /// </summary>
    [Service]
    public interface IScannerMessageFilter : IMessageFilter
    {
    }
}