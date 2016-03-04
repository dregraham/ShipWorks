using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using TD.SandDock;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Register the shipping ribbon
    /// </summary>
    public class ShippingRibbonRegistration : IMainFormElementRegistration
    {
        readonly IObservable<IShipWorksMessage> messages;
        readonly ComponentResourceManager resources;
        Button buttonCreateLabel;
        Button buttonVoid;
        Button buttonReturn;
        Button buttonReprint;
        Button buttonShipAgain;

        public ShippingRibbonRegistration(IObservable<IShipWorksMessage> messages)
        {
            this.messages = messages;
            resources = new ComponentResourceManager(typeof(MainForm));
        }

        /// <summary>
        /// Register elements with the ribbon
        /// </summary>
        public void Register(SandDockManager dockManager, Ribbon ribbon)
        {
            ribbon.SuspendLayout();

            buttonCreateLabel = new Button
            {
                Guid = new Guid("ec40e12c-fa12-4b2b-8b81-0fed6863162e"),
                Image = GetImgeResource("buttonCreateLabel.Image"),
                Padding = new WidgetEdges(10, 2, 10, 2),
                Text = "Create\r\nLabel",
                TextContentRelation = TextContentRelation.Underneath,
            };

            buttonVoid = new Button
            {
                Guid = new Guid("b477925d-b26f-47d7-91ee-619685bf1c7e"),
                Image = GetImgeResource("buttonVoid.Image"),
                Text = "Void",
                TextContentRelation = TextContentRelation.Underneath,
            };

            buttonReturn = new Button
            {
                Guid = new Guid("33800ee1-71e4-4940-b1c6-a4496e33ff91"),
                Image = Properties.Resources.document_out1,
                Text = "Return",
                TextContentRelation = TextContentRelation.Underneath,
            };

            buttonReprint = new Button
            {
                Guid = new Guid("ccc7cca3-4a1e-4975-a736-7a6449ece5c1"),
                Image = Properties.Resources.printer_preferences,
                Text = "Reprint",
            };

            buttonShipAgain = new Button
            {
                Guid = new Guid("8584db42-473a-4adf-a089-047e781d8728"),
                Image = GetImgeResource("buttonShipAgain.Image"),
                Text = "Ship Again",
            };

            StripLayout stripLayoutReprint = new StripLayout
            {
                Items = { buttonReprint, buttonShipAgain },
                LayoutDirection = LayoutDirection.Vertical,
            };

            RibbonChunk shippingShippingChunk = new RibbonChunk
            {
                Items = { buttonVoid, buttonReturn, stripLayoutReprint },
                Text = "Shipping",
            };

            RibbonChunk shippingOutputChunk = new RibbonChunk
            {
                Items = { buttonCreateLabel },
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

            messages.OfType<OrderSelectionChangedMessage>()
                .Subscribe(HandleOrderSelectionChanged);
        }

        /// <summary>
        /// Handle order selection changed message
        /// </summary>
        private void HandleOrderSelectionChanged(OrderSelectionChangedMessage message)
        {
            IEnumerable<ICarrierShipmentAdapter> shipments = message.LoadedOrderSelection
                .OfType<LoadedOrderSelection>()
                .SelectMany(y => y.ShipmentAdapters);

            if (shipments.Any())
            {
                buttonCreateLabel.Enabled = shipments.Any(y => !y.Shipment.Processed);
                return;
            }

            buttonCreateLabel.Enabled = message.LoadedOrderSelection.Skip(1).Any();
        }

        /// <summary>
        /// Get an image resource with the specified name
        /// </summary>
        private Image GetImgeResource(string name)
        {
            return (Image) resources.GetObject("buttonCreateLabel.Image");
        }
    }
}
