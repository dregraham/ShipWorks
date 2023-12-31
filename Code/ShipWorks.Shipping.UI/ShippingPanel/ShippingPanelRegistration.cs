﻿using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore;
using TD.SandDock;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Register the shipment panel
    /// </summary>
    public class ShippingPanelRegistration : IMainFormElementRegistration
    {
        /// <summary>
        /// Register a panel with the dock manager
        /// </summary>
        [SuppressMessage("SonarQube", "S1848:Objects should not be created to be dropped immediately without being used",
            Justification = "The DockableWindow is used by the dockManager")]
        [SuppressMessage("SonarQube", "S2930:\"IDisposables\" should be disposed",
            Justification = "The dock manager owns the panel, so we can't dispose it")]
        public void Register(SandDockManager dockManager, Ribbon ribbon)
        {
            ShippingPanel panelShipment = new ShippingPanel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(1, 1),
                Name = "panelShipment",
                Size = new Size(376, 168),
                TabIndex = 1
            };

            new DockableWindow(dockManager, panelShipment, "Shipping")
            {
                BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat,
                Guid = DockPanelIdentifiers.ShippingPanelGuid,
                Location = new Point(0, 25),
                Name = "dockableWindowShipment",
                ShowOptions = false,
                Size = new Size(378, 170),
                TabImage = Properties.Resources.box_next,
                TabIndex = 0
            };
        }
    }
}
