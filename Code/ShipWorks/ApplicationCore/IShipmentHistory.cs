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
        /// Update the history
        /// </summary>
        void Update();

        /// <summary>
        /// Save the grid column state
        /// </summary>
        void SaveGridColumnState();
    }
}