using System;
using System.Drawing;
using System.Windows.Forms;
using ShipWorks.ApplicationCore;
using TD.SandDock;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Register the shipment panel
    /// </summary>
    public class ShipmentPanelRegistration : IRegisterDockableWindow
    {
        /// <summary>
        /// Register a panel with the dock manager
        /// </summary>
        public void Register(SandDockManager dockManager)
        {
            ShipmentPanel panelShipment = new ShipmentPanel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(1, 1),
                Name = "panelShipment",
                Size = new Size(376, 168),
                TabIndex = 1
            };

            DockableWindow dockableWindowShipment = new DockableWindow(dockManager, panelShipment, "Shipment")
            {
                BorderStyle = TD.SandDock.Rendering.BorderStyle.Flat,
                Guid = new Guid("574C96CC-5D02-4689-9463-4FB4DBCE22AD"),
                Location = new Point(0, 25),
                Name = "dockableWindowShipment",
                ShowOptions = false,
                Size = new Size(378, 170),
                TabImage = Properties.Resources.add16,
                TabIndex = 0
            };
        }

    }
}
