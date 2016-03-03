using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
    public class ShippingPanelRegistration : IRegisterDockableWindow
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

            new DockableWindow(dockManager, panelShipment, "Shipment")
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


            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));


            RibbonTab ribbonTabShipping = new RibbonTab();
            RibbonChunk shippingOutputChunk = new RibbonChunk();
            Divelements.SandRibbon.Button buttonCreateLabel = new Divelements.SandRibbon.Button();
            RibbonChunk shippingShippingChunk = new RibbonChunk();
            Divelements.SandRibbon.Button buttonVoid = new Divelements.SandRibbon.Button();
            Divelements.SandRibbon.Button buttonReturn = new Divelements.SandRibbon.Button();
            Divelements.SandRibbon.Button buttonReprint = new Divelements.SandRibbon.Button();
            Divelements.SandRibbon.Button buttonShipAgain = new Divelements.SandRibbon.Button();
            StripLayout stripLayoutReprint = new Divelements.SandRibbon.StripLayout();










            ribbon.Controls.Add(ribbonTabShipping);

            //
            // ribbonTabShipping
            //
            ribbonTabShipping.Chunks.AddRange(new WidgetBase[] {
            shippingOutputChunk,
            shippingShippingChunk});
            ribbonTabShipping.EditingContextReference = "SHIPPINGMENU";
            ribbonTabShipping.Location = new Point(1, 53);
            ribbonTabShipping.Manager = ribbon.Manager;
            ribbonTabShipping.Name = "ribbonTabShipping";
            ribbonTabShipping.Size = new Size(967, 90);
            ribbonTabShipping.TabIndex = 7;
            ribbonTabShipping.Text = "Shipping";
            //
            // shippingOutputChunk
            //
            shippingOutputChunk.Items.AddRange(new WidgetBase[] {
            buttonCreateLabel});
            shippingOutputChunk.Text = "Output";
            //
            // buttonCreateLabel
            //
            buttonCreateLabel.Guid = new Guid("ec40e12c-fa12-4b2b-8b81-0fed6863162e");
            buttonCreateLabel.Image = ((Image) (resources.GetObject("buttonCreateLabel.Image")));
            buttonCreateLabel.Padding = new WidgetEdges(10, 2, 10, 2);
            buttonCreateLabel.Text = "Create\r\nLabel";
            buttonCreateLabel.TextContentRelation = TextContentRelation.Underneath;
            buttonCreateLabel.Activate += (s, e) => panelShipment.CreateLabel();
            //
            // shippingShippingChunk
            //
            shippingShippingChunk.Items.AddRange(new WidgetBase[] {
            buttonVoid,
            buttonReturn,
            stripLayoutReprint});
            shippingShippingChunk.Text = "Shipping";
            //
            // buttonVoid
            //
            buttonVoid.Guid = new Guid("b477925d-b26f-47d7-91ee-619685bf1c7e");
            buttonVoid.Image = ((Image) (resources.GetObject("buttonVoid.Image")));
            buttonVoid.Text = "Void";
            buttonVoid.TextContentRelation = TextContentRelation.Underneath;
            //
            // buttonReturn
            //
            buttonReturn.Guid = new Guid("33800ee1-71e4-4940-b1c6-a4496e33ff91");
            buttonReturn.Image = Properties.Resources.document_out1;
            buttonReturn.Text = "Return";
            buttonReturn.TextContentRelation = TextContentRelation.Underneath;

            //
            // stripLayoutReprint
            //
            stripLayoutReprint.Items.AddRange(new WidgetBase[] {
            buttonReprint,
            buttonShipAgain});
            stripLayoutReprint.LayoutDirection = LayoutDirection.Vertical;
            //
            // buttonReprint
            //
            buttonReprint.Guid = new Guid("ccc7cca3-4a1e-4975-a736-7a6449ece5c1");
            buttonReprint.Image = Properties.Resources.printer_preferences;
            buttonReprint.Text = "Reprint";
            //
            // buttonShipAgain
            //
            buttonShipAgain.Guid = new Guid("8584db42-473a-4adf-a089-047e781d8728");
            buttonShipAgain.Image = ((Image) (resources.GetObject("buttonShipAgain.Image")));
            buttonShipAgain.Text = "Ship Again";




        }
    }
}
