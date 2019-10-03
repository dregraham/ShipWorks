﻿using System;
using System.Windows.Forms;
using Divelements.SandRibbon;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.UI.Controls.SandRibbon;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Wrapper for the create label button in the main grid ribbon
    /// </summary>
    public class CreateLabelButtonWrapper : UserControl, IRibbonButton
    {
        private readonly RibbonButton actualCreateLabelButton;
        private readonly IShortcutManager shortcutManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelButtonWrapper(RibbonButton actualCreateLabelButton, IShortcutManager shortcutManager)
        {
            this.actualCreateLabelButton = actualCreateLabelButton;
            this.shortcutManager = shortcutManager;
            actualCreateLabelButton.HostControl.HandleCreated += AddToolTip;
        }

        /// <summary>
        /// Add tool tip to the button.
        /// </summary>
        /// <remarks>
        /// This is done here instead of at registration because we need to access the database for the hotkey to
        /// show. During registration, the shortcut manager is not loaded up.
        /// </remarks>
        private void AddToolTip(object sender, EventArgs e)
        {
            (string Title, string Description) toolTipText = shortcutManager.GetCreateLabelToolTipText();
            
            actualCreateLabelButton.ToolTip = new SuperToolTip(toolTipText.Title,
                                                               toolTipText.Description,
                                                               null, false);
        }
        
        /// <summary>
        /// Perform the create label action associated with this button.
        /// </summary>
        public void CreateLabel()
        {
            Activate?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The event name to send to telemetry
        /// </summary>
        public string TelemetryEventName
        {
            get => actualCreateLabelButton.TelemetryEventName;
            set { }
        }

        /// <summary>
        /// Event handler for button click
        /// </summary>
        public event EventHandler Activate;
    }
}