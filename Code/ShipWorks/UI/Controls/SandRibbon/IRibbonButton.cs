using System;

namespace ShipWorks.Core.UI.SandRibbon
{
    /// <summary>
    /// Interface to make testing button usage easier
    /// </summary>
    public interface IRibbonButton
    {
        /// <summary>
        /// Is the button enabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Activate handler
        /// </summary>
        event EventHandler Activate;
    }
}