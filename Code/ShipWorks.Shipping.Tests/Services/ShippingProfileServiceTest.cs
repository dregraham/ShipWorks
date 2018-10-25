using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
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
                        KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null)
                });

            ShortcutEntity shortcut = new ShortcutEntity
            {
                VirtualKey = VirtualKeys.B,
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift
            };

            var testObject = mock.Create<ShippingProfileService>();
            var result = testObject.GetAvailableHotkeys(CreateShippingProfile(null, shortcut)).ToList();

            Assert.Equal(2, result.Count());
            Assert.Contains(VirtualKeys.A, result.Select(r => r.ActionKey));
            Assert.Contains(VirtualKeys.B, result.Select(r => r.ActionKey));
        }

        private EditableShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut) =>
            mock.Create<EditableShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, 1)]
        [InlineData(ShipmentTypeCode.Asendia, 0)]
        public void GetEditableConfiguredShipmentTypeProfiles_ShippingProfileContainsShipmentType_WhenShipmentTypeConfigured(ShipmentTypeCode configuredType, int expectedShipmentProfileCount)
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.Amazon
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IEditableShippingProfileRepository>().Setup(s => s.GetAll()).Returns(new[] { profile });

            mock.Mock<IShipmentTypeManager>().SetupGet(s => s.ConfiguredShipmentTypeCodes)
                .Returns(new[] { configuredType });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Equal(expectedShipmentProfileCount, testObject.GetEditableConfiguredShipmentTypeProfiles().Count());
        }

        [Fact]
        public void GetEditableConfiguredShipmentTypeProfiles_ShippingProfilesDoesNotContainBestRate_WhenShipmentTypeNotAllowed()
        {
            var profileEntity = new ShippingProfileEntity()
            {
                ShippingProfileID = 42,
                ShipmentType = ShipmentTypeCode.BestRate
            };

            var shortcut = new ShortcutEntity() { RelatedObjectID = 42, VirtualKey = null, ModifierKeys = null };
            var profile = CreateShippingProfile(profileEntity, shortcut);

            mock.Mock<IShippingProfileService>().Setup(s => s.GetEditableConfiguredShipmentTypeProfiles()).Returns(new[] { profile });

            mock.Mock<IShipmentTypeManager>().SetupGet(s => s.ConfiguredShipmentTypeCodes).Returns(new[] { ShipmentTypeCode.Usps });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Empty(testObject.GetEditableConfiguredShipmentTypeProfiles());
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

            mock.Mock<IShippingProfileService>().Setup(s => s.GetEditableConfiguredShipmentTypeProfiles()).Returns(new[] { profile });

            var testObject = mock.Create<ShippingProfileService>();

            Assert.Empty(testObject.GetEditableConfiguredShipmentTypeProfiles());
        }

        [Fact]
        public void Get_DelegatesToShippingProfileRepository()
        {
            var testObject = mock.Create<ShippingProfileService>().GetEditable(123);
            mock.Mock<IEditableShippingProfileRepository>().Verify(s => s.Get(123));
        }

        [Fact]
        public void Delete_DelegatesToShippingProfileRepository()
        {
            var profile = mock.Mock<IEditableShippingProfile>();
            var testObject = mock.Create<ShippingProfileService>().Delete(profile.Object);
            mock.Mock<IEditableShippingProfileRepository>().Verify(s => s.Delete(profile.Object));
        }

        [Fact]
        public void Save_DelegatesToShippingProfileRepository()
        {
            var profile = mock.Mock<IEditableShippingProfile>();
            var testObject = mock.Create<ShippingProfileService>().Save(profile.Object);
            mock.Mock<IEditableShippingProfileRepository>().Verify(s => s.Save(profile.Object));
        }

        [Fact]
        public void CreateEmptyShippingProfile_DelegatesToShippingProfileFactory()
        {
            var testObject = mock.Create<ShippingProfileService>().CreateEmptyShippingProfile();
            mock.Mock<IShippingProfileFactory>().Verify(s => s.CreateEditable(), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}