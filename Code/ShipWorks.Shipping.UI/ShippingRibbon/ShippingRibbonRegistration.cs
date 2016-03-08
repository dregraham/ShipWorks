using System;
using System.ComponentModel;
using System.Drawing;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI.SandRibbon;
using TD.SandDock;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Register the shipping ribbon
    /// </summary>
    public class ShippingRibbonRegistration : IMainFormElementRegistration, IShippingRibbonActions, IDisposable
    {
        readonly ComponentResourceManager resources;
        readonly IShippingRibbonService shippingRibbonService;
        RibbonButton createLabelButton;
        RibbonButton voidButton;
        RibbonButton returnButton;
        RibbonButton reprintButton;
        RibbonButton shipAgainButton;

        public ShippingRibbonRegistration(IShippingRibbonService shippingRibbonService)
        {
            resources = new ComponentResourceManager(typeof(MainForm));
            this.shippingRibbonService = shippingRibbonService;
        }

        /// <summary>
        /// Register elements with the ribbon
        /// </summary>
        public void Register(SandDockManager dockManager, Ribbon ribbon)
        {
            ribbon.SuspendLayout();

            createLabelButton = new RibbonButton
            {
                Guid = new Guid("ec40e12c-fa12-4b2b-8b81-0fed6863162e"),
                Image = GetImgeResource("buttonCreateLabel.Image"),
                Padding = new WidgetEdges(10, 2, 10, 2),
                Text = "Create\r\nLabel",
                TextContentRelation = TextContentRelation.Underneath,
            };

            voidButton = new RibbonButton
            {
                Guid = new Guid("b477925d-b26f-47d7-91ee-619685bf1c7e"),
                Image = GetImgeResource("buttonVoid.Image"),
                Text = "Void",
                TextContentRelation = TextContentRelation.Underneath,
            };

            returnButton = new RibbonButton
            {
                Guid = new Guid("33800ee1-71e4-4940-b1c6-a4496e33ff91"),
                Image = Properties.Resources.document_out1,
                Text = "Return",
                TextContentRelation = TextContentRelation.Underneath,
            };

            reprintButton = new RibbonButton
            {
                Guid = new Guid("ccc7cca3-4a1e-4975-a736-7a6449ece5c1"),
                Image = Properties.Resources.printer_preferences,
                Text = "Reprint",
            };

            shipAgainButton = new RibbonButton
            {
                Guid = new Guid("8584db42-473a-4adf-a089-047e781d8728"),
                Image = GetImgeResource("buttonShipAgain.Image"),
                Text = "Ship Again",
            };

            StripLayout stripLayoutReprint = new StripLayout
            {
                Items = { reprintButton, shipAgainButton },
                LayoutDirection = LayoutDirection.Vertical,
            };

            RibbonChunk shippingShippingChunk = new RibbonChunk
            {
                Items = { voidButton, returnButton, stripLayoutReprint },
                Text = "Shipping",
            };

            RibbonChunk shippingOutputChunk = new RibbonChunk
            {
                Items = { createLabelButton },
                Text = "Output"
            };

            RibbonTab ribbonTabShipping = new RibbonTab
            {
                Chunks = { shippingOutputChunk, shippingShippingChunk },
                EditingContextReference = "SHIPPINGMENU",
                Location = new Point(1, 53),
                Manager = ribbon.Manager,
                Name = "ribbonTabShipping",
                Size = new Size(967, 90),
                TabIndex = 7,
                Text = "Shipping",
            };

            ribbon.Controls.Add(ribbonTabShipping);

            ribbon.ResumeLayout();

            shippingRibbonService.Register(this);
        }

        /// <summary>
        /// Create label action
        /// </summary>
        public IRibbonButton CreateLabel => createLabelButton;

        /// <summary>
        /// Void label action
        /// </summary>
        public IRibbonButton Void => voidButton;

        /// <summary>
        /// Create return shipment action
        /// </summary>
        public IRibbonButton Return => returnButton;

        /// <summary>
        /// Reprint shipment action
        /// </summary>
        public IRibbonButton Reprint => reprintButton;

        /// <summary>
        /// Ship again action
        /// </summary>
        public IRibbonButton ShipAgain => shipAgainButton;

        /// <summary>
        /// Get an image resource with the specified name
        /// </summary>
        private Image GetImgeResource(string name)
        {
            return (Image) resources.GetObject("buttonCreateLabel.Image");
        }

        /// <summary>
        /// Dispose held resources
        /// </summary>
        public void Dispose()
        {
            createLabelButton?.Dispose();
            voidButton?.Dispose();
            returnButton?.Dispose();
            reprintButton?.Dispose();
            shipAgainButton?.Dispose();
        }
    }
}
