using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using Moq;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Tests.Shipping.Profiles
{
    public class EditableShippingProfileTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IShippingProfileManager> shippingProfileManagerMock;
        private readonly Mock<IShortcutManager> shortcutManagerMock;
        private Mock<ISecurityContext> securityContext;

        public EditableShippingProfileTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingProfileManagerMock = mock.Mock<IShippingProfileManager>();
            shortcutManagerMock = mock.Mock<IShortcutManager>();

            securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, It.IsAny<long>())).Returns(true);
        }

        [Fact]
        public void EditableShippingProfile_ShipmentTypeDescriptionIsShipmentTypeDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity
            {
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
            };

            var testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, EnumHelper.GetDescription(profile.ShipmentType));
        }

        [Fact]
        public void EditableShippingProfile_ShortcutKey_IsShortcutKeyDescription()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            ShortcutEntity shortcut = new ShortcutEntity
            {
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
            };

            var testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal("Ctrl+Shift+0", testObject.ShortcutKey);
        }

        [Fact]
        public void EditableShippingProfile_ShortcutKeyIsBlank_WhenShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            var testObject = CreateShippingProfile(profile, null);
            Assert.Equal(testObject.ShortcutKey, string.Empty);
        }

        [Fact]
        public void EditableShippingProfile_ShipmentTypeDescriptionIsBlank_WhenShipmentTypeIsNull()
        {
            ShortcutEntity shortcut = new ShortcutEntity
            {
                ModifierKeys = KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift,
                VirtualKey = VirtualKeys.N0
            };

            ShippingProfileEntity profile = new ShippingProfileEntity
            {
                ShipmentType = null
            };

            var testObject = CreateShippingProfile(profile, shortcut);
            Assert.Equal(testObject.ShipmentTypeDescription, string.Empty);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenProfileNameIsEmpty()
        {
            var testObject = CreateShippingProfile(new ShippingProfileEntity(), new ShortcutEntity());
            var result = Validate(testObject);

            Assert.True(result.Failure);
            Assert.Equal("Enter a name for the profile.", result.Message);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenAProfileWithSameNameExists()
        {
            shippingProfileManagerMock
                .SetupGet(m => m.Profiles)
                .Returns(new[] { new ShippingProfileEntity { Name = "same" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShippingProfileID = 5, Name = "same" },
                new ShortcutEntity());

            var result = Validate(testObject);

            Assert.True(result.Failure);
            Assert.Equal("A profile with the chosen name already exists.", result.Message);
        }

        [Fact]
        public void Validate_ReturnsTrue_WhenBarcodeIsBlank()
        {
            shortcutManagerMock
                .SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity { Barcode = "" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShippingProfileID = 5, Name = "name" },
                new ShortcutEntity { Barcode = "" });

            var result = Validate(testObject);

            Assert.True(result.Success);
        }

        [Fact]
        public void Validate_ReturnsFailure_WhenAShortcutWithSameBarcodeExists()
        {
            shortcutManagerMock
                .SetupGet(m => m.Shortcuts)
                .Returns(new[] { new ShortcutEntity { Barcode = "same" } });

            var testObject = CreateShippingProfile(new ShippingProfileEntity { Name = "blah" }, new ShortcutEntity
            {
                ShortcutID = 42,
                Barcode = "same"
            });

            var result = Validate(testObject);

            Assert.True(result.Failure);
            Assert.Equal("The barcode \"same\" is already in use.", result.Message);
        }

        [Theory]
        [InlineData(null, ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.Usps, null)]
        [InlineData(ShipmentTypeCode.FedEx, ShipmentTypeCode.UpsOnLineTools)]
        public void ChangeProvider_ChangesShipmentType(ShipmentTypeCode? initialShipmentType, ShipmentTypeCode? newShipmentType)
        {
            var testObject = CreateShippingProfile(new ShippingProfileEntity { ShipmentType = initialShipmentType },
                new ShortcutEntity());

            testObject.ChangeProvider(newShipmentType);

            Assert.Equal(newShipmentType, testObject.ShippingProfileEntity.ShipmentType);
        }

        [Fact]
        public void ChangeProvider_ClearsPackages()
        {
            var profile = new ShippingProfileEntity();
            profile.Packages.Add(new UpsProfilePackageEntity());

            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeProvider(ShipmentTypeCode.Amazon);
            Assert.Empty(profile.Packages);
        }

        [Fact]
        public void ChangeProvider_DelegatesToProfileRepository()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity();
            ShortcutEntity shortcut = new ShortcutEntity();
            var testObject = CreateShippingProfile(profile, shortcut);

            testObject.ChangeProvider(ShipmentTypeCode.Endicia);

            // The initial set
            mock.Mock<IEditableShippingProfileRepository>().Verify(l => l.Load(testObject, true), Times.Once);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(new KeyboardShortcutData(KeyboardShortcutCommand.ApplyWeight, VirtualKeys.A, KeyboardShortcutModifiers.Alt, null), "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(VirtualKeys.A, testObject.Shortcut.VirtualKey);
            Assert.Equal(KeyboardShortcutModifiers.Alt, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues_WhenKeyboardShortcutValuesAreNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(new KeyboardShortcutData(null, null, null, null), "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_SetsShortcutEntityValues_WhenKeyboardShortcutIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(null, "abcd");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        [Fact]
        public void ChangeShortcut_TrimsBarcode()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity { ShipmentType = ShipmentTypeCode.Amazon };
            var testObject = CreateShippingProfile(profile, new ShortcutEntity());

            testObject.ChangeShortcut(null, "           abcd  ");

            Assert.Equal("abcd", testObject.Shortcut.Barcode);
            Assert.Equal(null, testObject.Shortcut.VirtualKey);
            Assert.Equal(null, testObject.Shortcut.ModifierKeys);
            Assert.Equal(KeyboardShortcutCommand.ApplyProfile, testObject.Shortcut.Action);
        }

        private EditableShippingProfile CreateShippingProfile(ShippingProfileEntity profile, ShortcutEntity shortcut) =>
            mock.Create<EditableShippingProfile>(TypedParameter.From(profile), TypedParameter.From(shortcut));

        private Result Validate(EditableShippingProfile testObject)
        {
            return testObject.Validate(shippingProfileManagerMock.Object, shortcutManagerMock.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
