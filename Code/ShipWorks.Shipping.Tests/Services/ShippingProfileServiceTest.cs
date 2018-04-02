using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShippingProfileServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        
        public ShippingProfileServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void GetAvailableHotkeys_ReturnsExistingHotkeysAndHotkeyOfCurrentProfile()
        {
            mock.Mock<IShortcutManager>()
                .Setup(m => m.GetAvailableHotkeys())
                .Returns(new List<KeyboardShortcutData>
                {
                    new KeyboardShortcutData(null, VirtualKeys.A,
                        KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift)
                });

            ShortcutEntity shortcut = new ShortcutEntity
            {
                VirtualKey = VirtualKeys.B,
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift
            };
            
            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.GetAvailableHotkeys(CreateShippingProfile(null, shortcut)).ToList();

            Assert.Equal(2, result.Count());
            Assert.Contains(VirtualKeys.A, result.Select(r=>r.ActionKey));
            Assert.Contains(VirtualKeys.B, result.Select(r => r.ActionKey));
        }

        private ShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut)
        {
            var shippingProfile = mock.Create<ShippingProfile>();
            shippingProfile.ShippingProfileEntity = profile;
            shippingProfile.Shortcut = shortcut;
            return shippingProfile;
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, 1)]
        [InlineData(ShipmentTypeCode.Asendia, 0)]
        public void GetConfiguredShipmentTypeProfiles_ShippingProfileContainsShipmentType_WhenShipmentTypeConfigured(ShipmentTypeCode configuredType, int expectedShipmentProfileCount)
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.Amazon
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileRepository>().Setup(s => s.GetAll()).Returns(new List<ShippingProfile>() { profile });

            mock.Mock<IShipmentTypeManager>().SetupGet(s => s.ConfiguredShipmentTypeCodes)
                .Returns(new[] { configuredType });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Equal(expectedShipmentProfileCount, testObject.GetConfiguredShipmentTypeProfiles().Count());
        }

        [Fact]
        public void GetConfiguredShipmentTypeProfiles_ShippingProfilesDoesNotContainBestRate_WhenShipmentTypeNotAllowed()
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.BestRate
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            mock.Mock<IShipmentTypeManager>().SetupGet(s => s.ConfiguredShipmentTypeCodes).Returns(new[] { ShipmentTypeCode.Usps });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Empty(testObject.GetConfiguredShipmentTypeProfiles());
        }


        [Fact]
        public void Constructor_ShippingProfilesDoesNotContainNone()
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.None
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileService>().Setup(s => s.GetConfiguredShipmentTypeProfiles()).Returns(new List<ShippingProfile>() { profile });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Empty(testObject.GetConfiguredShipmentTypeProfiles());
        }

        [Fact]
        public void Get_DelegatesToShippingProfileRepository()
        {
            var testObject = mock.Create<ShippingProfileService>().Get(123);
            mock.Mock<IShippingProfileRepository>().Verify(s => s.Get(123));
        }

        [Fact]
        public void Delete_DelegatesToShippingProfileRepository()
        {
            var profile = mock.Mock<IShippingProfile>();
            var testObject = mock.Create<ShippingProfileService>().Delete(profile.Object);
            mock.Mock<IShippingProfileRepository>().Verify(s => s.Delete(profile.Object));
        }

        [Fact]
        public void Save_DelegatesToShippingProfileRepository()
        {
            var profile = mock.Mock<IShippingProfile>();
            var testObject = mock.Create<ShippingProfileService>().Save(profile.Object);
            mock.Mock<IShippingProfileRepository>().Verify(s => s.Save(profile.Object));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}