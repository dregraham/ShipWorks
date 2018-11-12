using System;
using System.Windows.Forms;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Shipment history
    /// </summary>
    public interface IShipmentHistory : IDisposable
    {
        /// <summary>
        /// Control to display the history
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Refresh the history, load any components
        /// </summary>
        void Activate(Divelements.SandRibbon.Button voidButton, Divelements.SandRibbon.Button shipAgainButton);

        /// <summary>
        /// Unload any components
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Save the grid column state
        /// </summary>
        void SaveGridColumnState();

        /// <summary>
        /// Number of rows in the grid
        /// </summary>
        long RowCount { get; }
    }
}