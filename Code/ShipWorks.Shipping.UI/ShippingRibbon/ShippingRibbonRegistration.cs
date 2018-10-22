using System;
using System.Drawing;
using Divelements.SandRibbon;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls.SandRibbon;
using TD.SandDock;

namespace ShipWorks.Shipping.UI.ShippingRibbon
{
    /// <summary>
    /// Register the shipping ribbon
    /// </summary>
    [Component]
    public class ShippingRibbonRegistration : IMainFormElementRegistration, IShippingRibbonActions, IDisposable
    {
        private readonly IShippingRibbonService shippingRibbonService;
        private readonly IShortcutManager shortcutManager;
        private CreateLabelButtonWrapper createLabelButton;
        private RibbonButton actualCreateLabelButton;
        private RibbonButton voidButton;
        private RibbonButton returnButton;
        private RibbonButton reprintButton;
        private RibbonButton shipAgainButton;
        private IDisposable profileMenuDisposable;
        private ApplyProfileButtonWrapper applyProfileButton;
        private RibbonButton manageProfilesButton;
        private ShipmentTypeCode? currentShipmentType;
        private readonly IProfilePopupService profilePopupService;

        public ShippingRibbonRegistration(
            IShippingRibbonService shippingRibbonService,
            IProfilePopupService profilePopupService,
            IShortcutManager shortcutManager)
        {
            this.profilePopupService = profilePopupService;
            this.shippingRibbonService = shippingRibbonService;
            this.shortcutManager = shortcutManager;
        }

        /// <summary>
        /// Register elements with the ribbon
        /// </summary>
        public void Register(SandDockManager dockManager, Ribbon ribbon)
        {
            ribbon.SuspendLayout();

            RegisterCreateLabelButton();

            voidButton = new RibbonButton
            {
                Guid = new Guid("b477925d-b26f-47d7-91ee-619685bf1c7e"),
                Image = Properties.Resources.box_void_32_32,
                Text = "Void",
                TextContentRelation = TextContentRelation.Underneath,
            };

            returnButton = new RibbonButton
            {
                Guid = new Guid("33800ee1-71e4-4940-b1c6-a4496e33ff91"),
                Image = Properties.Resources.box_previous_32_32,
                Text = "Return",
                TextContentRelation = TextContentRelation.Underneath,
            };

            reprintButton = new RibbonButton
            {
                Guid = new Guid("ccc7cca3-4a1e-4975-a736-7a6449ece5c1"),
                Image = Properties.Resources.printer_redo_16_16,
                Text = "Reprint",
            };

            shipAgainButton = new RibbonButton
            {
                Guid = new Guid("8584db42-473a-4adf-a089-047e781d8728"),
                Image = Properties.Resources.box_closed_redo_16_16,
                Text = "Ship Again",
            };

            RibbonButton actualApplyProfileButton = new RibbonButton
            {
                DropDownStyle = DropDownStyle.Integral,
                Guid = new Guid("D2AF2859-5B48-4CA1-AA6B-649A033462BB"),
                Image = Properties.Resources.document_out1,
                Text = "Apply",
                TextContentRelation = TextContentRelation.Underneath
            };

            applyProfileButton = new ApplyProfileButtonWrapper(actualApplyProfileButton);
            profileMenuDisposable = profilePopupService.BuildMenu(
                actualApplyProfileButton,
                new Guid("7DC9AA2C-DB1A-447E-B717-DB252CC39338"),
                () => currentShipmentType,
                applyProfileButton.ApplyProfile);

            manageProfilesButton = new RibbonButton
            {
                Guid = new Guid("0E7A63DD-0BDB-4AF4-BC24-05666022EF75"),
                Image = Properties.Resources.box_closed_with_label_32_32,
                Text = "Manage",
                TextContentRelation = TextContentRelation.Underneath
            };

            StripLayout stripLayoutReprint = new StripLayout
            {
                Items = { reprintButton, shipAgainButton },
                LayoutDirection = LayoutDirection.Vertical,
            };

            RibbonChunk profilesChunk = new RibbonChunk
            {
                FurtherOptions = false,
                ItemJustification = ItemJustification.Stretch,
                Items = { actualApplyProfileButton, manageProfilesButton },
                Text = "Profiles",
            };

            RibbonChunk shippingShippingChunk = new RibbonChunk
            {
                FurtherOptions = false,
                ItemJustification = ItemJustification.Stretch,
                Items = { actualCreateLabelButton, voidButton, returnButton, stripLayoutReprint },
                Text = "Shipping",
            };

            RibbonTab ribbonTabShipping = new RibbonTab
            {
                Chunks = { shippingShippingChunk, profilesChunk },
                EditingContextReference = "SHIPPINGMENU",
                Location = new Point(1, 53),
                Manager = ribbon.Manager,
                Name = "ribbonTabShipping",
                Size = new Size(967, 90),
                TabIndex = 7,
                Text = "Shipping",
            };

            ribbon.Controls.Add(ribbonTabShipping);

            // This needs to be done after the button is added to the ribbon because it needs to hook in to the
            // host controls loaded event.
            createLabelButton = new CreateLabelButtonWrapper(actualCreateLabelButton, shortcutManager);

            ribbon.ResumeLayout();

            shippingRibbonService.Register(this);
        }

        /// <summary>
        /// Creates the label button
        /// </summary>
        private void RegisterCreateLabelButton()
        {
            actualCreateLabelButton = new RibbonButton
            {
                Guid = new Guid("ec40e12c-fa12-4b2b-8b81-0fed6863162e"),
                Image = Properties.Resources.box_next_32_32,
                Padding = new WidgetEdges(10, 2, 10, 2),
                Text = "Create\r\nLabel",
                TextContentRelation = TextContentRelation.Underneath
            };

            actualCreateLabelButton.Activate += (s, evt) => createLabelButton.CreateLabel();
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
        /// Apply a profile to the given shipment
        /// </summary>
        public IRibbonButton ApplyProfile => applyProfileButton;

        /// <summary>
        /// Manage profiles
        /// </summary>
        public IRibbonButton ManageProfiles => manageProfilesButton;

        /// <summary>
        /// Set the type of the currently selected shipment, if any
        /// </summary>
        public void SetCurrentShipmentType(ShipmentTypeCode? shipmentType)
        {
            currentShipmentType = shipmentType;
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
            applyProfileButton?.Dispose();
            manageProfilesButton?.Dispose();
            profileMenuDisposable?.Dispose();
        }
    }
}
