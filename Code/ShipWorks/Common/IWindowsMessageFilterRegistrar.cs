using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Common
{
    /// <summary>
    /// Registrar for Windows message filters
    /// </summary>
    [Service]
    public interface IWindowsMessageFilterRegistrar
    {
        /// <summary>
        /// Add a message filter
        /// </summary>
        void AddMessageFilter(IMessageFilter messageFilter);

        /// <summary>
        /// Remove a message filter
        /// </summary>
        void RemoveMessageFilter(IMessageFilter messageFilter);
    }
}
