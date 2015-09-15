using System;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.ApplicationCore;
using TD.SandDock;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Register the rates panel
    /// </summary>
    public class RatingPanelRegistration : IRegisterDockableWindow
    {
        /// <summary>
        /// Register a panel with the dock manager
        /// </summary>
        public void Register(SandDockManager dockManager)
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

            DockableWindow dockableWindowRates = new DockableWindow(dockManager, panelRating, "Rates")
            {
                BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat,
                Guid = new Guid("68C7B550-A30D-406E-9FE8-12224489FB2B"),
                Location = new Point(0, 25),
                Name = "dockableWindowRates",
                ShowOptions = false,
                Size = new Size(378, 170),
                TabImage = Properties.Resources.add16,
                TabIndex = 0
            };
        }

    }
}
