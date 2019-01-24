using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Divelements.SandRibbon;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Profiles
{

    /// <summary>
    /// Service for managing profile popup menus
    /// </summary>
    [Component]
    public class ProfilePopupService : IProfilePopupService
    {
        private readonly IShippingProfileService profileService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfilePopupService(IShippingProfileService profileService)
        {
            this.profileService = profileService;
        }

        /// <summary>
        /// Build the profile popup menu
        /// </summary>
        public IDisposable BuildMenu(
                Button profileButton,
                Guid menuGuid,
                Func<ShipmentTypeCode?> getCurrentShipmentType,
                Action<IShippingProfile> onSelection) =>
            new ProfilePopup(profileButton, menuGuid, getCurrentShipmentType, onSelection, profileService);

        /// <summary>
        /// Profile popup
        /// </summary>
        private class ProfilePopup : IDisposable
        {
            private readonly Popup popup;
            private readonly Menu menu;
            private readonly Func<ShipmentTypeCode?> getCurrentShipmentType;
            private readonly Action<IShippingProfile> onSelection;
            private readonly IShippingProfileService profileService;

            public ProfilePopup(Button profileButton,
                Guid menuGuid,
                Func<ShipmentTypeCode?> getCurrentShipmentType,
                Action<IShippingProfile> onSelection,
                IShippingProfileService profileService)
            {
                this.profileService = profileService;
                this.onSelection = onSelection;
                this.getCurrentShipmentType = getCurrentShipmentType;

                menu = new Menu
                {
                    Text = "Profile List",
                    Guid = menuGuid
                };

                popup = new Popup
                {
                    Items = { menu }
                };

                popup.BeforePopup += OnApplyProfileBeforePopup;

                profileButton.PopupWidget = popup;
            }

            /// <summary>
            /// Load the profile menu list
            /// </summary>
            private void OnApplyProfileBeforePopup(object sender, BeforePopupEventArgs e)
            {
                menu.Items.Clear();

                List<WidgetBase> menuItems = new List<WidgetBase>();
                IEnumerable<IGrouping<ShipmentTypeCode?, IShippingProfile>> profileGroups = profileService
                    .GetConfiguredShipmentTypeProfiles()
                    .Where(p => p.IsApplicable(getCurrentShipmentType()))
                    //.Select(s => s.ShippingProfileEntity).Cast<IShippingProfileEntity>()
                    .GroupBy(p => p.ShippingProfileEntity.ShipmentType)
                    .OrderBy(g => g.Key.HasValue ? ShipmentTypeManager.GetSortValue(g.Key.Value) : -1);

                foreach (IGrouping<ShipmentTypeCode?, IShippingProfile> profileGroup in profileGroups)
                {
                    string groupName = "Global";
                    if (profileGroup.Key.HasValue)
                    {
                        groupName = profileGroup.ToString();
                        MenuItem carrierLabel = new MenuItem(EnumHelper.GetDescription(profileGroup.Key.Value))
                        {
                            Font = new Font(new FontFamily("Tahoma"), 6.5f, FontStyle.Bold),
                            Padding = new WidgetEdges(28, -1, 0, -1),
                            GroupName = groupName,
                            Enabled = false
                        };
                        menuItems.Add(carrierLabel);
                    }

                    menuItems.AddRange(
                        profileGroup
                            .OrderByDescending(p => p.ShippingProfileEntity.ShipmentTypePrimary)
                            .ThenBy(p => p.ShippingProfileEntity.Name)
                            .Select(profile => CreateMenuItem(profile, groupName)));
                }

                if (menuItems.None())
                {
                    menuItems = new List<WidgetBase> { new MenuItem { Text = "(None)", Enabled = false } };
                }

                menu.Items.AddRange(menuItems.ToArray());
            }

            /// <summary>
            /// Create a menu item from the given profile
            /// </summary>
            private WidgetBase CreateMenuItem(IShippingProfile profile, string groupName)
            {
                MenuItem menuItem = new MenuItem(profile.ShippingProfileEntity.Name) { GroupName = groupName };
                menuItem.Activate += (s, evt) => onSelection(profile);
                return menuItem;
            }

            public void Dispose()
            {
                menu.Dispose();
                popup.Dispose();
            }
        }
    }
}
