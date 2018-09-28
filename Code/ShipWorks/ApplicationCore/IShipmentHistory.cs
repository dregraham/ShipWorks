using System.Windows.Forms;

namespace ShipWorks
{
    /// <summary>
    /// Shipment history
    /// </summary>
    public interface IShipmentHistory
    {
        /// <summary>
        /// Control to display the history
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Refresh the history
        /// </summary>
        void ReloadShipmentData();

        /// <summary>
        /// Save the grid column state
        /// </summary>
        void SaveGridColumnState();
    }
}