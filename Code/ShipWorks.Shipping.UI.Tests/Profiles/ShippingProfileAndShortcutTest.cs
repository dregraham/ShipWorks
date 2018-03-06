using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.Profiles;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Profiles
{
    public class ShippingProfileAndShortcutTest
    {
        [Fact]
        public void ShippingProfileAndShortcut_ShipmentTypeDescriptionIsShipmentTypeDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileAndShortcut testObject = new ShippingProfileAndShortcut(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, EnumHelper.GetDescription(profile.ShipmentType));
        }

        [Fact]
        public void ShippingProfileAndShortcut_ShortcutKey_IsShortcutKeyDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileAndShortcut testObject = new ShippingProfileAndShortcut(profile, shortcut);
            Assert.Equal(testObject.ShortcutKey, EnumHelper.GetDescription(shortcut.Hotkey));
        }

        [Fact]
        public void ShippingProfileAndShortcut_ShortcutKeyIsBlank_WhenShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShippingProfileAndShortcut testObject = new ShippingProfileAndShortcut(profile, null);
            Assert.Equal(testObject.ShortcutKey, string.Empty);
        }

        [Fact]
        public void ShippingProfileAndShortcut_ShipmentTypeDescriptionIsBlank_WhenShipmentTypeIsNull()
        {
            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = null
            };

            ShippingProfileAndShortcut testObject = new ShippingProfileAndShortcut(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, string.Empty);
        }
    }
}
