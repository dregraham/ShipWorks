using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// An interface intended to facilitate opening the Odbc control panel(odbcad32)
    /// </summary>
    public interface IOdbcControlPanel
    {
        /// <summary>
        /// Launch the Odbc control panel
        /// </summary>
        /// <param name="callbackAction">the action to invoice when the panel exits</param>
        /// <remarks>
        /// Invokes the callbackAction when the control panel exits
        /// </remarks>
        void Launch(Action callbackAction);
    }
}
