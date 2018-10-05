using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interface for a OrderLookup
    /// </summary>
    public interface IOrderLookup
    {
        /// <summary>
        /// The Order Lookup Control
        /// </summary>
        UserControl Control { get; }

        /// <summary>
        /// Unload the order
        /// </summary>
        void Unload();
    }
}