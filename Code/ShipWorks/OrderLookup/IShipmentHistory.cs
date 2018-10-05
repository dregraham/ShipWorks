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
        void Activate(Divelements.SandRibbon.Button voidButton);

        /// <summary>
        /// Unload any components
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Save the grid column state
        /// </summary>
        void SaveGridColumnState();
    }
}