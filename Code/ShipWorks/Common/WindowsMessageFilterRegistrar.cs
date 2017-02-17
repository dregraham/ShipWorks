using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Common
{
    /// <summary>
    /// Registrar for Windows message filters
    /// </summary>
    [Component]
    public class WindowsMessageFilterRegistrar : IWindowsMessageFilterRegistrar
    {
        /// <summary>
        /// Add a message filter
        /// </summary>
        public void AddMessageFilter(IMessageFilter messageFilter) =>
            Application.AddMessageFilter(messageFilter);

        /// <summary>
        /// Remove a message filter
        /// </summary>
        public void RemoveMessageFilter(IMessageFilter messageFilter) =>
            Application.RemoveMessageFilter(messageFilter);
    }
}
