using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.UI.SandRibbon;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
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
        readonly IShippingProfileManager profileManager;
        RibbonButton createLabelButton;
        RibbonButton voidButton;
        RibbonButton returnButton;
        RibbonButton reprintButton;
        RibbonButton shipAgainButton;
        ApplyProfileButtonWrapper applyProfileButton;
        RibbonButton manageProfilesButton;
        Popup applyProfilePopup;
        Menu applyProfileMenu;
        ShipmentTypeCode? currentShipmentType;

        public ShippingRibbonRegistration(IShippingRibbonService shippingRibbonService, IShippingProfileManager profileManager)
        {
            resources = new ComponentResourceManager(typeof(MainForm));
            this.shippingRibbonService = shippingRibbonService;
            this.profileManager = profileManager;
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
                Image = Properties.Resources.box_next_32_32,
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

            applyProfileMenu = new Menu
            {
                Text = "Profile List",
                Guid = new Guid("7DC9AA2C-DB1A-447E-B717-DB252CC39338")
            };

            applyProfilePopup = new Popup
            {
                Items = { applyProfileMenu }
            };

            applyProfilePopup.BeforePopup += new BeforePopupEventHandler(OnApplyProfileBeforePopup);

            RibbonButton actualApplyProfileButton = new RibbonButton
            {
                DropDownStyle = DropDownStyle.Integral,
                Guid = new Guid("D2AF2859-5B48-4CA1-AA6B-649A033462BB"),
                Image = Properties.Resources.document_out1,
                PopupWidget = applyProfilePopup,
                Text = "Apply",
                TextContentRelation = TextContentRelation.Underneath
            };

            applyProfileButton = new ApplyProfileButtonWrapper(actualApplyProfileButton);

            manageProfilesButton = new RibbonButton
            {
                Guid = new Guid("0E7A63DD-0BDB-4AF4-BC24-05666022EF75"),
                Image = Properties.Resources.graphics_tablet,
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
                Items = { createLabelButton, voidButton, returnButton, stripLayoutReprint },
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

            ribbon.ResumeLayout();

            shippingRibbonService.Register(this);
        }

        /// <summary>
        /// Load the profile menu list
        /// </summary>
        private void OnApplyProfileBeforePopup(object sender, BeforePopupEventArgs e)
        {
            applyProfileMenu.Items.Clear();

            if (!currentShipmentType.HasValue)
            {
                return;
            }

            WidgetBase[] menuItems = profileManager.GetProfilesFor(currentShipmentType.Value)
                .OrderBy(x => x.ShipmentTypePrimary)
                .ThenBy(x => x.Name)
                .Select(x => CreateMenuItem(x, x.ShipmentTypePrimary ? "Default" : "Custom"))
                .ToArray();

            if (!menuItems.Any())
            {
                menuItems = new[] { new MenuItem { Text = "(None)", Enabled = false } };
            }

            applyProfileMenu.Items.AddRange(menuItems);
        }

        /// <summary>
        /// Create a menu item from the given profile
        /// </summary>
        private WidgetBase CreateMenuItem(ShippingProfileEntity profile, string groupName)
        {
            MenuItem menuItem = new MenuItem(profile.Name);
            menuItem.GroupName = groupName;
            menuItem.Activate += (s, evt) => applyProfileButton.ApplyProfile(profile);
            return menuItem;
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
        /// Get an image resource with the specified name
        /// </summary>
        private Image GetImgeResource(string name)
        {
            return (Image) resources.GetObject(name);
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
            applyProfilePopup?.Dispose();
            applyProfileMenu?.Dispose();
        }
    }
}
