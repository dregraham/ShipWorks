using System.Windows.Forms;

namespace ShipWorks.Common
{
    /// <summary>
    /// Registrar for Windows message filters
    /// </summary>
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
