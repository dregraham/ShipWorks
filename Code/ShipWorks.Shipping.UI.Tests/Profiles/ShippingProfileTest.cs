using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Profiles
{
    public class ShippingProfileTest : IDisposable
    {
        private readonly AutoMock mock;

        public ShippingProfileTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsShipmentTypeDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, EnumHelper.GetDescription(profile.ShipmentType));
        }

        [Fact]
        public void ShippingProfile_ShortcutKey_IsShortcutKeyDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShortcutKey, EnumHelper.GetDescription(shortcut.Hotkey));
        }

        [Fact]
        public void ShippingProfile_ShortcutKeyIsBlank_WhenShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShippingProfile testObject = CreateShippingProfile(profile, null);
            Assert.Equal(testObject.ShortcutKey, string.Empty);
        }

        [Fact]
        public void ShippingProfile_ShipmentTypeDescriptionIsBlank_WhenShipmentTypeIsNull()
        {
            ShortcutEntity shortcut = new ShortcutEntity()
            {
                Hotkey = IO.KeyboardShortcuts.Hotkey.CtrlShift0
            };

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                ShipmentType = null
            };

            ShippingProfile testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, string.Empty);
        }

        private ShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut)
        {
            return new ShippingProfile(profile, shortcut, mock.Mock<IShippingProfileManager>().Object,
                mock.Mock<IShortcutManager>().Object, mock.Mock<IShippingProfileLoader>().Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
