﻿using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore;
using TD.SandDock;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Register the rating panel
    /// </summary>
    public class RatingPanelRegistration : IMainFormElementRegistration
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
            RatingPanel panelRating = new RatingPanel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(1, 1),
                Name = "panelRating",
                Size = new Size(376, 168),
                TabIndex = 1
            };

            new DockableWindow(dockManager, panelRating, "Rates")
            {
                BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat,
                Guid = DockPanelIdentifiers.RatingPanelGuid,
                Location = new Point(0, 25),
                Name = "dockableWindowRating",
                ShowOptions = false,
                Size = new Size(378, 170),
                TabImage = Properties.Resources.table_dollar_sign_16_16,
                TabIndex = 0
            };
        }
    }
}
